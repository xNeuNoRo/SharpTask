import {
  completeTaskAction,
  createTaskAction,
  deleteTaskAction,
  updateTaskAction,
  updateTaskStatusAction,
} from "@/actions/task-actions";
import { noteKeys, taskKeys } from "@/lib/query-keys";
import { Task, TaskDetail } from "@/schemas/task";
import {
  QueryClient,
  useMutation,
  useQueryClient,
} from "@tanstack/react-query";
import { toast } from "react-toastify";

/**
 * @description Hook para crear una nueva tarea. Utiliza React Query para manejar el estado de la mutación y actualizar la cache de consultas después de crear la tarea.
 * @returns Un objeto con la información de la mutación, incluyendo el estado de carga, cualquier error y una función para ejecutar la mutación de creación de tarea.
 */
export function useCreateTask() {
  // Obtenemos el cliente de consultas de React Query para poder manipular la cache de consultas
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createTaskAction,
    onSuccess: (data) => {
      // Mostramos un mensaje de éxito al usuario despues de crear la tarea
      toast.success("Tarea creada exitosamente");

      // Hacemos un Pessimistic Update antes de invalidar la cache para que la nueva tarea aparezca inmediatamente
      //  en la lista de tareas sin tener que esperar a que se vuelva a cargar la lista desde el servidor
      queryClient.setQueryData(
        taskKeys.lists(),
        (oldData: Task[] | undefined) => {
          // Si no hay datos previos, retornamos un nuevo array con la tarea creada
          if (!oldData) return [data];
          // Si la tarea ya existe en la cache (lo cual no debería pasar pero por si acaso), no la agregamos de nuevo
          if (oldData.some((task) => task.id === data.id)) return oldData;
          // Agregamos la nueva tarea al inicio de la lista de tareas en la cache
          return [data, ...oldData];
        },
      );

      // Creamos un nuevo objeto de tarea detallada con las notas inicializadas como un array vacío
      const newTask: TaskDetail = {
        ...data,
        notes: [], // Inicializamos las notas como un array vacío para mantener la consistencia con el esquema de TaskDetail
      };

      // Tambien actualizamos la tarea en los detalles de la tarea en la cache para mantenerla sincronizada con la lista de tareas
      // Esto es importante para que si el usuario va a los detalles de la tarea recién creada después de crearla,
      // vea los datos actualizados sin tener que esperar a que se vuelva a cargar los detalles desde el servidor
      queryClient.setQueryData(
        taskKeys.detail(data.id),
        (oldData: Task | undefined) => {
          // Si no hay datos previos, retornamos la tarea creada como nuevo dato en la cache
          if (!oldData) return newTask;
          // Actualizamos la tarea en la cache con los nuevos datos (aunque en este caso no debería haber datos previos)
          return { ...oldData, ...newTask };
        },
      );

      // Invalidamos la cache de la todas las query keys de tareas para que se vuelvan a cargar
      // desde el servidor y se mantenga sincronizada con el backend
      queryClient.invalidateQueries({ queryKey: taskKeys.all });
    },
    onError: (error) => {
      // Mostramos el mensaje de error al usuario si la creación de la tarea falla
      toast.error(error.message);
    },
  });
}

// Función auxiliar para actualizar una tarea en la cache de consultas después de actualizarla en el servidor
const updateTaskInCache = (queryClient: QueryClient, data: Task) => {
  // Hacemos un Pessimistic Update antes de invalidar la cache para que la tarea actualizada aparezca inmediatamente
  // en la lista de tareas sin tener que esperar a que se vuelva a cargar la lista desde el servidor
  queryClient.setQueryData(
    taskKeys.detail(data.id),
    (oldData: Task | undefined) => {
      // Si no hay datos previos, retornamos la tarea actualizada como nuevo dato en la cache
      if (!oldData)
        return {
          ...data,
          notes: [], // Inicializamos las notas como un array vacío para mantener la consistencia con el esquema de TaskDetail
        };
      // Actualizamos la tarea en la cache con los nuevos datos
      return { ...oldData, ...data };
    },
  );

  // Tambien actualizamos la tarea en la lista de tareas en la cache para mantenerla sincronizada con los detalles de la tarea
  // Esto es importante para que si el usuario vuelve a la lista de tareas después de actualizar una tarea,
  // vea los datos actualizados sin tener que esperar a que se vuelva a cargar la lista desde el servidor
  queryClient.setQueryData(taskKeys.lists(), (oldData: Task[] | undefined) => {
    // Si no hay datos previos, retornamos solamente un arr con la tarea actualizada
    if (!oldData) return [data];
    // Actualizamos la tarea en la lista de tareas en la cache con los nuevos datos
    return oldData.map((task) =>
      task.id === data.id ? { ...task, ...data } : task,
    );
  });

  // Invalidamos la cache de la todas las query keys de tareas para que se vuelvan a cargar
  // desde el servidor y se mantenga sincronizada con el backend
  queryClient.invalidateQueries({ queryKey: taskKeys.all });
};

/**
 * @description Hook para actualizar una tarea. Utiliza React Query para manejar el estado de la mutación y actualizar la cache de consultas después de actualizar la tarea.
 * @returns Un objeto con la información de la mutación, incluyendo el estado de carga, cualquier error y una función para ejecutar la mutación de actualización de tarea.
 */
export function useUpdateTask() {
  // Obtenemos el cliente de consultas de React Query para poder manipular la cache de consultas
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: updateTaskAction,
    onSuccess: (data) => {
      // Mostramos un mensaje de éxito al usuario despues de actualizar la tarea
      toast.success("Tarea actualizada exitosamente");
      // Actualizamos la tarea en la cache de consultas para que aparezca inmediatamente
      updateTaskInCache(queryClient, data);
    },
    onError: (error) => {
      // Mostramos el mensaje de error al usuario si la actualización de la tarea falla
      toast.error(error.message);
    },
  });
}

/**
 * @description Hook para actualizar el estado de una tarea. Utiliza React Query para manejar el estado de la mutación y actualizar la cache de consultas después de actualizar el estado de la tarea.
 * @returns Un objeto con la información de la mutación, incluyendo el estado de carga, cualquier error y una función para ejecutar la mutación de actualización del estado de la tarea.
 */
export function useUpdateTaskStatus() {
  // Obtenemos el cliente de consultas de React Query para poder manipular la cache de consultas
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: updateTaskStatusAction,
    onSuccess: (data) => {
      // Mostramos un mensaje de éxito al usuario despues de actualizar el estado de la tarea
      toast.success("Estado de la tarea actualizado exitosamente");

      // Actualizamos la tarea en la cache de consultas para que aparezca inmediatamente
      updateTaskInCache(queryClient, data);
    },
    onError: (error) => {
      // Mostramos el mensaje de error al usuario si la actualización del estado de la tarea falla
      toast.error(error.message);
    },
  });
}

/**
 * @description Hook para completar una tarea. Utiliza React Query para manejar el estado de la mutación y actualizar la cache de consultas después de completar la tarea.
 * @returns Un objeto con la información de la mutación, incluyendo el estado de carga, cualquier error y una función para ejecutar la mutación de completar tarea.
 */
export function useCompleteTask() {
  // Obtenemos el cliente de consultas de React Query para poder manipular la cache de consultas
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: completeTaskAction,
    onSuccess: (data) => {
      // Mostramos un mensaje de éxito al usuario despues de completar la tarea
      toast.success("Tarea completada exitosamente");

      // Actualizamos la tarea en la cache de consultas para que aparezca inmediatamente
      updateTaskInCache(queryClient, data);
    },
    onError: (error) => {
      // Mostramos el mensaje de error al usuario si la actualización del estado de la tarea falla
      toast.error(error.message);
    },
  });
}

/**
 * @description Hook para eliminar una tarea. Utiliza React Query para manejar el estado de la mutación y actualizar la cache de consultas después de eliminar la tarea.
 * @returns Un objeto con la información de la mutación, incluyendo el estado de carga, cualquier error y una función para ejecutar la mutación de eliminación de tarea.
 */
export function useDeleteTask() {
  // Obtenemos el cliente de consultas de React Query para poder manipular la cache de consultas
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: deleteTaskAction,
    onMutate: async (taskId) => {
      // Cancelamos cualquier consulta en curso relacionada con las tareas para evitar conflictos de actualización de la cache
      await queryClient.cancelQueries({ queryKey: taskKeys.all });
      await queryClient.cancelQueries({ queryKey: noteKeys.all });

      // Guardamos una copia de los datos previos de la lista de tareas
      // para poder revertir la cache en caso de que la eliminación falle
      const previousTasks = queryClient.getQueryData<Task[]>(taskKeys.lists());

      // Hacemos un Optimistic Update antes de eliminar la tarea para que desaparezca inmediatamente
      // de la lista de tareas sin tener que esperar a que se vuelva a cargar la lista desde el servidor
      // En caso de que no se haya eliminado realmente del servidor, al invalidar las queries luego
      // se volverá a cargar nuevamente en la cache y aparecerá de nuevo en la lista de tareas
      queryClient.setQueryData(
        taskKeys.lists(),
        (oldData: Task[] | undefined) => {
          // Si no hay datos previos, retornamos un arr vacío
          if (!oldData) return [];
          // Eliminamos la tarea de la lista de tareas en la cache filtrando por su ID
          return oldData.filter((task) => task.id !== taskId);
        },
      );

      // Devolvemos los datos previos de la lista de tareas para que puedan ser utilizados
      // en caso de que la eliminación falle y necesitemos revertir la cache
      return { previousTasks };
    },
    onSuccess: () => {
      // Mostramos un mensaje de éxito al usuario después de eliminar la tarea
      toast.success("Tarea eliminada exitosamente");
    },
    // onSettled siempre se ejecuta sin importar que tenga exito, o falle la mutacion,
    // Es buena practica aplicar las invalidaciones aca cuando hacemos optimistic updates
    // para asegurarnos de que la cache se mantenga sincronizada con el backend incluso si la mutacion falla o tiene exito
    onSettled: () => {
      // Invalidamos la cache de la todas las query keys de tareas y notas para que se vuelvan a cargar
      // desde el servidor y se mantenga sincronizada con el backend
      queryClient.invalidateQueries({ queryKey: taskKeys.all });
      queryClient.invalidateQueries({ queryKey: noteKeys.all });
    },
    onError: (error, taskId, context) => {
      // Mostramos el mensaje de error al usuario si la eliminación de la tarea falla
      toast.error(error.message);

      // Revertimos la cache de la lista de tareas a los datos previos guardados en el contexto en caso de que la eliminación falle
      if (context?.previousTasks) {
        queryClient.setQueryData(taskKeys.lists(), context.previousTasks);
      }
    },
  });
}
