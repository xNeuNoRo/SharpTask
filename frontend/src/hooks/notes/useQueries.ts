import { getNoteById, getNotesByTaskId } from "@/api/NotesAPI";
import { noteKeys } from "@/lib/query-keys";
import { Note } from "@/schemas/notes";
import { Task } from "@/schemas/task";
import { useQuery, useQueryClient } from "@tanstack/react-query";

/**
 * @description Hook para obtener la lista de notas de una tarea específica por su ID. Utiliza React Query para manejar el estado de la consulta.
 * @param taskId El ID de la tarea para la cual se desean obtener las notas. La consulta solo se ejecutará si el ID es válido (no nulo o indefinido).
 * @returns Un objeto con la información de la consulta, incluyendo los datos de las notas, el estado de carga y cualquier error.
 */
export function useNotes(taskId?: Task["id"]) {
  // Aseguramos que el taskId sea una cadena válida para evitar errores en la consulta
  const validTaskId = taskId ?? "";

  return useQuery({
    queryKey: noteKeys.lists(validTaskId),
    queryFn: () => getNotesByTaskId(validTaskId),
    enabled: !!taskId, // Solo ejecutar la consulta si el taskId es válido
    staleTime: 1000 * 60 * 5, // 5 minutos
  });
}

/**
 * @description Hook para obtener los detalles de una nota específica por su ID y el ID de la tarea a la que pertenece. Utiliza React Query para manejar el estado de la consulta.
 * @param taskId El ID de la tarea a la que pertenece la nota. La consulta solo se ejecutará si el ID es válido (no nulo o indefinido).
 * @param noteId El ID de la nota que se desea obtener. La consulta solo se ejecutará si el ID es válido (no nulo o indefinido).
 * @returns Un objeto con la información de la consulta, incluyendo los datos de la nota, el estado de carga y cualquier error.
 */
export function useNote(taskId?: Task["id"], noteId?: Note["id"]) {
  // Obtenemos el cliente de consultas de React Query para poder acceder a
  // la cache de consultas y proporcionar datos de placeholder mientras se carga la nota
  const queryClient = useQueryClient();

  // Aseguramos que el taskId y noteId sean cadenas válidas para evitar errores en la consulta
  const validTaskId = taskId ?? "";
  const validNoteId = noteId ?? "";

  return useQuery({
    queryKey: noteKeys.detail(validTaskId, validNoteId),
    queryFn: () => getNoteById(validTaskId, validNoteId),
    enabled: !!taskId && !!noteId, // Solo ejecutar la consulta si el taskId y noteId son válidos
    placeholderData: () => {
      // Intentamos obtener la lista de notas para el taskId específico de la cache de consultas
      const notes = queryClient.getQueryData<Note[]>(
        noteKeys.lists(validTaskId),
      );
      // Si encontramos la lista de notas en la cache, intentamos encontrar la nota específica por su ID
      return notes?.find((note) => note.id === validNoteId);
    },
  });
}
