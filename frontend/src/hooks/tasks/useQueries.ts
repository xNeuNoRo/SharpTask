import { getTaskById, getTasks, searchTask } from "@/api/TasksAPI";
import { taskKeys } from "@/lib/query-keys";
import { Task } from "@/schemas/task";
import { useQuery } from "@tanstack/react-query";

/**
 * @description Hook para obtener la lista de tareas. Utiliza React Query para manejar el estado de la consulta.
 * @returns Un objeto con la información de la consulta, incluyendo los datos, el estado de carga y cualquier error.
 */
export function useTasks(status?: Task["status"]) {
  return useQuery({
    queryKey: taskKeys.lists(status),
    queryFn: () => getTasks(status),
    staleTime: 1000 * 60 * 5, // 5 minutos
  });
}

/**
 * @description Hook para obtener los detalles de una tarea específica por su ID. Utiliza React Query para manejar el estado de la consulta.
 * @param id El ID de la tarea que se desea obtener. La consulta solo se ejecutará si el ID es válido (no nulo o indefinido).
 * @returns Un objeto con la información de la consulta, incluyendo los datos de la tarea, el estado de carga y cualquier error.
 */
export function useTask(id?: Task["id"]) {
  // Aseguramos que el ID sea una cadena válida para evitar errores en la consulta
  const validId = id ?? "";

  return useQuery({
    queryKey: taskKeys.detail(validId),
    queryFn: () => getTaskById(validId),
    enabled: !!id, // Solo ejecutar la consulta si el ID es válido
  });
}

/**
 * @description Hook para buscar tareas por una palabra clave. Utiliza React Query para manejar el estado de la consulta.
 * @param keyword La palabra clave que se utilizará para buscar tareas. La consulta solo se ejecutará si la palabra clave es válida (no vacía).
 * @returns Un objeto con la información de la consulta, incluyendo los datos de las tareas encontradas, el estado de carga y cualquier error.
 */
export function useSearchTasks(keyword: string) {
  return useQuery({
    queryKey: taskKeys.search(keyword),
    queryFn: () => searchTask(keyword),
    enabled: !!keyword, // Solo ejecutar la consulta si el keyword es válido
  });
}
