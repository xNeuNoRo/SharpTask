using System.Text.Json;

namespace SharpTask.Infrastructure.Repositories.Base;

public abstract class JsonBaseRepo<T>
    where T : class // Restringimos T a ser una clase (referencia) para evitar problemas con tipos primitivos
{
    // Ruta del archivo JSON
    protected readonly string _filePath;

    // Cache para almacenar las categorias en memoria
    // y ahorrar lecturas al disco cuando se realizan multiples operaciones seguidas
    protected List<T>? _cache;

    // Opciones de serializacion JSON
    protected readonly JsonSerializerOptions _options;

    /// <summary>
    /// Constructor de la clase base para repositorios que manejan archivos JSON.
    /// </summary>
    /// <param name="filePath">La ruta del archivo JSON.</param>
    protected JsonBaseRepo(string filePath)
    {
        // Inicializa la ruta del archivo
        _filePath = filePath;
        // Configuramos las opciones para que el JSON sea indentado (mas legible)
        _options = new JsonSerializerOptions
        {
            WriteIndented = true, // Indenta el JSON para que sea mas legible
            PropertyNameCaseInsensitive = true, // Ignora mayusculas/minusculas en los nombres de las propiedades
            AllowTrailingCommas = true, // Permite comas al final de los objetos/arrays en el JSON
        };

        // Asegura que el archivo exista
        EnsureFile();
    }

    /// <summary>
    /// Metodo para asegurar que el archivo JSON exista antes de intentar leerlo o escribirlo.
    /// </summary>
    private void EnsureFile()
    {
        // Obtiene el directorio del archivo
        var directory = Path.GetDirectoryName(_filePath);

        // Si hay un directorio en la ruta y no existe, lo crea
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Si el archivo no existe o esta vacio, lo crea con un array vacío
        if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
            File.WriteAllText(_filePath, "[]");
    }

    /// <summary>
    /// Metodo para cargar todos los items del archivo JSON.
    /// </summary>
    /// <returns>Una lista de objetos T cargados desde el archivo JSON.</returns>
    public async Task<List<T>> LoadAsync()
    {
        // Si la cache no es nula, retorna la cache para evitar leer el archivo nuevamente
        if (_cache != null)
            return _cache;

        // Asegura que el archivo exista antes de intentar cargarlo
        EnsureFile();

        // Lee el contenido del archivo JSON
        string json = await File.ReadAllTextAsync(_filePath);

        // Si el contenido esta vacío, retorna una lista vacia
        if (string.IsNullOrWhiteSpace(json))
        {
            _cache = new List<T>();
            return _cache;
        }

        try
        {
            // Deserializamos el contenido completo a una lista de objetos T
            // Si la deserializacion falla por X o Y razon, retornamos una lista vacia
            _cache = JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
            return _cache;
        }
        catch (JsonException)
        {
            // Si el JSON se corrompio, no se puede deserializar, o algo raro idk, retornamos una lista vacia
            _cache = new List<T>();
            return _cache;
        }
    }

    /// <summary>
    /// Metodo para encontrar todos los items que cumplan con la condicion dada por el callback.
    /// </summary>
    /// <param name="cb">El callback que define la condición a cumplir.</param>
    /// <returns>Una lista de items que cumplen con la condición dada.</returns>
    public async Task<List<T>> FindManyAsync(Func<T, bool> cb)
    {
        // Cargamos todos los items
        var items = await LoadAsync();

        // Filtramos los items que cumplen con la condición del callback
        return items.Where(cb).ToList();
    }

    /// <summary>
    /// Metodo para encontrar el primer item que cumpla con la condicion dada por el callback.
    /// Ej: X => X.Id == 5, se busca el primer item que tenga Id igual a 5.
    /// </summary>
    /// <param name="cb">El callback que define la condición a cumplir.</param>
    /// <returns>El primer item que cumpla con la condición, o null si no se encuentra ninguno.</returns>
    public async Task<T?> FindAsync(Func<T, bool> cb)
    {
        // Buscamos el primer objeto que cumpla la condición del delegado
        return (await LoadAsync()).FirstOrDefault(cb);
    }

    /// <summary>
    /// Metodo para guardar una lista completa de items en el archivo JSON, sobrescribiendo lo que haya.
    /// </summary>
    /// <param name="items">La lista de items a guardar.</param>
    /// <returns>Una tarea asincronica que representa la operacion de guardado.</returns>
    public async Task SaveAllAsync(List<T> items)
    {
        // Serializamos la lista completa a formato JSON
        string json = JsonSerializer.Serialize(items, _options);
        // Escribimos el JSON en el archivo (sobrescribiendo lo que haya)
        await File.WriteAllTextAsync(_filePath, json);
        // Actualizamos la cache con los items guardados
        _cache = items;
    }

    /// <summary>
    /// Metodo para agregar un nuevo item al archivo JSON sin necesidad de cargar y guardar toda la lista manualmente.
    /// </summary>
    /// <param name="item">El item a agregar.</param>
    /// <returns>Una tarea asincronica que representa la operacion de agregado.</returns>
    public async Task AppendAsync(T item)
    {
        // Cargamos todos los items existentes
        var items = await LoadAsync();

        // Agregamos el nuevo item a la lista
        items.Add(item);

        // Guardamos nuevamente toda la lista en el archivo
        await SaveAllAsync(items);
    }

    /// <summary>
    /// Metodo para actualizar un item que cumpla con la condicion dada por el callback, reemplazandolo por un nuevo item dado.
    /// </summary>
    /// <param name="cb">El callback que define la condición a cumplir.</param>
    /// <param name="newItem">El nuevo item con el que se reemplazará el item encontrado.</param>
    /// <returns>True si se actualizó correctamente, false si no se encontró ningún item que cumpla con la condición.</returns>
    public async Task<bool> UpdateAsync(Func<T, bool> cb, T newItem)
    {
        // Cargamos todos los items
        var items = await LoadAsync();

        // Buscamos el indice del primer item que cumple con el callback
        int index = items.FindIndex(new Predicate<T>(cb));

        // Si no se encontro ningun item que cumpla con el callback, retorna false
        if (index == -1)
            return false;

        // Reemplaza el item en el indice encontrado con el nuevo item dado
        items[index] = newItem;

        // Guarda todos los items actualizados en el archivo
        await SaveAllAsync(items);

        // Retorna true indicando que se actualizo correctamente
        return true;
    }

    /// <summary>
    /// Metodo para eliminar items que cumplan con la condicion dada por el callback.
    /// EJ: Si cb es x => x.Id == 5, se eliminan todos los items que tengan Id igual a 5.
    /// </summary>
    /// <param name="cb">El callback que define la condición a cumplir.</param>
    /// <returns>True si se eliminó al menos un item, false si no se eliminó ninguno.</returns>
    public async Task<bool> DeleteAsync(Func<T, bool> cb)
    {
        // Cargamos todos los items
        var items = await LoadAsync();

        // Cuenta cuantos items habia inicialmente
        int initialCount = items.Count;

        // Filtramos los items que NO cumplen con el callback
        // EJ: Si cb es x => x.Id == 5, se quedan todos los que NO tienen Id 5
        var remainingItems = items.Where(x => !cb(x)).ToList();

        // Si la cantidad de items restantes es diferente a la inicial, significa que se elimino al menos uno
        if (remainingItems.Count != initialCount)
        {
            // Guardamos la lista actualizada sin los items eliminados
            await SaveAllAsync(remainingItems);
            return true;
        }

        // Retornamos false indicando que no se elimino ningun item
        return false;
    }

    // =================================
    // Metodos extra al repo
    // =================================

    /// <summary>
    /// Metodo para encontrar un item con su lista de items relacionados, usando un repo relacionado y una logica de inclusion inyectada.
    /// EJ: Si tenemos un repo de Tareas y un repo de notas, podemos usar este metodo para encontrar una tarea con su lista de notas relacionadas, inyectando la logica de inclusion que separe las notas por Id de tarea.
    /// </summary>
    /// <typeparam name="TRelated"></typeparam>
    /// <param name="cb">El callback que define la condición a cumplir para encontrar el item principal.</param>
    /// <param name="relatedRepo">El repo de los items relacionados.</param>
    /// <param name="includeLogic">La lógica de inclusión que se aplica al item principal con los items relacionados.</param>
    /// <returns>El item principal encontrado con sus items relacionados incluidos, o null si no se encontró el item principal.</returns>
    public async Task<T?> FindWithIncludeAsync<TRelated>(
        Func<T, bool> cb, // El callback para encontrar el item principal
        JsonBaseRepo<TRelated> relatedRepo, // El repo de los items relacionados
        Action<T, List<TRelated>> includeLogic // La logica de inclusion que se aplica al item principal con los items relacionados
    )
        where TRelated : class
    {
        // Encontramos el item principal usando el callback dado
        var item = await FindAsync(cb);

        // Si no se encuentra el item principal, retornamos null
        // sin intentar cargar los relacionados ni aplicar la logica de inclusion
        if (item == null)
            return null;

        // Cargamos todos los items relacionados usando el repo relacionado dado
        var relatedItems = await relatedRepo.LoadAsync();

        // Aplicamos la logica de inclusion inyectada al item principal con los items relacionados cargados
        // EJ: (note, tasks) => note.Task = tasks.FirstOrDefault(t => t.Id == note.TaskId)
        includeLogic(item, relatedItems);

        return item;
    }

    /// <summary>
    /// Metodo para cargar todos los items con su lista de items relacionados,
    /// usando un repo relacionado y una logica de inclusion inyectada.
    /// EJ: Si tenemos un repo de Tareas y un repo de notas, podemos usar este metodo para cargar
    /// todas las tareas con su lista de notas relacionadas, inyectando la logica de inclusion
    /// que separe las notas por Id de tarea.
    /// </summary>
    /// <typeparam name="TRelated">El tipo de los items relacionados.</typeparam>
    /// <param name="relatedRepo">El repo de los items relacionados.</param>
    /// <param name="includeLogic">La logica de inclusion que se aplica a cada item principal con sus items relacionados.</param>
    /// <returns>La lista de items principales con sus items relacionados incluidos.</returns>
    public async Task<List<T>> LoadWithIncludeAsync<TRelated>(
        JsonBaseRepo<TRelated> relatedRepo,
        Action<T, List<TRelated>> includeLogic
    )
        where TRelated : class
    {
        // Cargamos todos los items principales y relacionados usando sus respectivos repos
        var items = await LoadAsync();
        var relatedItems = await relatedRepo.LoadAsync();

        // Por cada item principal, aplicamos la logica de inclusion inyectada con los items relacionados cargados
        foreach (var item in items)
        {
            includeLogic(item, relatedItems);
        }

        // Retornamos la lista de items principales con sus items relacionados incluidos
        return items;
    }

    /// <summary>
    /// Metodo para encontrar todos los items que cumplan con la condicion dada por el callback,
    /// con su lista de items relacionados, usando un repo relacionado y una logica de inclusion inyectada.
    /// EJ: Si tenemos un repo de Tareas y un repo de notas, podemos usar este metodo
    /// para encontrar todas las tareas que esten pendientes, con su lista de notas relacionadas,
    /// inyectando la logica de inclusion que separe las notas por Id de tarea.
    /// </summary>
    /// <typeparam name="TRelated">El tipo de los items relacionados.</typeparam>
    /// <param name="cb">El callback que define la condicion para filtrar los items principales.</param>
    /// <param name="relatedRepo">El repo de los items relacionados.</param>
    /// <param name="includeLogic">La logica de inclusion que se aplica a cada item principal con sus items relacionados.</param>
    /// <returns>La lista de items principales que cumplen con la condicion dada por el callback, con sus items relacionados incluidos.</returns>
    public async Task<List<T>> FindManyWithIncludeAsync<TRelated>(
        Func<T, bool> cb, // Condicion para filtrar (Ej: x => x.Status == Pending)
        JsonBaseRepo<TRelated> relatedRepo, // El repo de los items relacionados
        Action<T, List<TRelated>> includeLogic // La logica de inclusion que se aplica a cada item principal con sus items relacionados
    )
        where TRelated : class
    {
        // Cargamos todos los items principales usando el repo actual
        var allItems = await LoadAsync();
        // Filtramos los items principales que cumplen con la condicion dada por el callback
        var filteredItems = allItems.Where(cb).ToList();

        // Si no se encuentra ningun item principal que cumpla con la condicion, retornamos una lista vacia
        if (!filteredItems.Any())
            return filteredItems;

        // Cargamos todos los items relacionados usando el repo relacionado dado
        var relatedItems = await relatedRepo.LoadAsync();

        // Por cada item principal filtrado, aplicamos la logica de inclusion inyectada con los items relacionados cargados
        foreach (var item in filteredItems)
        {
            includeLogic(item, relatedItems);
        }

        return filteredItems;
    }
}
