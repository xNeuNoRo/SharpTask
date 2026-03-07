import { isAxiosError } from "axios";

/**
 * @description Maneja los errores de las peticiones a la API. Si el error es un error de Axios con respuesta del servidor, lanza un error con el mensaje específico proporcionado por la API. Si el error es otro tipo de error (como un error de red), lo lanza para que sea manejado por el componente que llamó a esta función.
 * @param err El error que se desea manejar. Puede ser un error de Axios con respuesta del servidor, un error de red u otro tipo de error.
 */
export function handleApiError(err: unknown): never {
  // Si el error es un error de Axios con respuesta del servidor,
  // lanzamos un error con el mensaje específico proporcionado por la API
  if (isAxiosError(err) && err.response) {
    // Si la API respondió con un error, lanzamos el mensaje de error específico
    throw new Error(err.response.data.error.message);
  }
  // Si el error es otro tipo de error (como un error de red), lo lanzamos para que sea manejado por el componente que llamó a esta función
  throw err;
}