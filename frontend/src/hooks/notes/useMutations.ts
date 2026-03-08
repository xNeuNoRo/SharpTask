import {
  createNoteAction,
  deleteNoteAction,
  updateNoteAction,
} from "@/actions/note-actions";
import { noteKeys, taskKeys } from "@/lib/query-keys";
import { CreateNoteFormData, Note, UpdateNoteFormData } from "@/schemas/notes";
import { Task, TaskDetail } from "@/schemas/task";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-toastify";

/**
 * @description Hook para crear una nueva nota en una tarea específica. Utiliza React Query para manejar el estado de la mutación y actualizar la cache de consultas relacionada después de crear la nota.
 * @param taskId El ID de la tarea a la que se desea agregar la nota. Es necesario para asociar la nota con la tarea correcta y para actualizar la cache de consultas relacionada después de crear la nota.
 * @returns Un objeto con la información de la mutación, incluyendo el estado de carga, cualquier error y una función para ejecutar la mutación. La función para ejecutar la mutación se puede utilizar en la interfaz de usuario para iniciar el proceso de creación de la nota cuando el usuario lo desee.
 */
export function useCreateNote(taskId: Task["id"]) {
  // Obtenemos el cliente de consultas de React Query para poder invalidar las consultas relacionadas después de crear una nota
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (noteData: CreateNoteFormData) =>
      createNoteAction(taskId, noteData),
    onSuccess: (data) => {
      // Mostramos un mensaje de éxito al usuario después de crear la nota
      toast.success("Nota creada exitosamente");

      // Pessimistic Update
      // Actualizamos la cache de consultas para que la nueva nota aparezca inmediatamente
      // sin necesidad de esperar a que se vuelva a cargar desde el servidor
      queryClient.setQueryData(
        noteKeys.lists(taskId),
        (oldData: Note[] | undefined) => {
          // Si no hay datos anteriores, devolvemos un nuevo array con la nota creada
          if (!oldData) return [data];

          // Si la nota ya existe en la cache (lo cual no debería pasar pero por si acaso), no la agregamos de nuevo
          if (oldData.some((note) => note.id === data.id)) return oldData;

          // Agregamos la nueva nota al inicio de la lista de notas en la cache
          return [data, ...oldData];
        },
      );

      // Pessimistic Update
      // También actualizamos la cache de la query key de detalle de nota para que
      // si el usuario navega a esa nota, vea los datos actualizados
      queryClient.setQueryData(
        noteKeys.detail(taskId, data.id),
        (oldData: Note | undefined) => {
          // Si no hay datos anteriores, devolvemos la nota creada como nuevo dato en la cache
          if (!oldData) return data;
          // Actualizamos la nota en la cache con los nuevos datos
          return { ...oldData, ...data };
        },
      );

      // Pessimistic Update
      // Actualizamos la cache de la query key de detalle de tarea para reflejar
      // la nueva nota agregada a la lista de notas de la tarea
      queryClient.setQueryData(
        taskKeys.detail(taskId),
        (oldData: TaskDetail | undefined) => {
          // Si no hay datos anteriores, devolvemos el mismo dato sin cambios
          if (!oldData) return oldData;
          // Actualizamos la tarea en la cache para reflejar la nueva nota agregada a la lista de notas de la tarea
          return { ...oldData, notes: [...oldData.notes, data] };
        },
      );

      // Invalidamos la cache de las query keys relacionadas con las notas de esta tarea
      queryClient.invalidateQueries({ queryKey: noteKeys.byTask(taskId) });
      queryClient.invalidateQueries({ queryKey: taskKeys.detail(taskId) });
    },
    onError: (error) => {
      // Mostramos el mensaje de error al usuario si la creación de la nota falla
      toast.error(error.message);
    },
  });
}

/**
 * @description Hook para actualizar una nota existente en una tarea específica. Utiliza React Query para manejar el estado de la mutación y actualizar la cache de consultas relacionada después de actualizar la nota.
 * @param taskId El ID de la tarea a la que pertenece la nota que se desea actualizar. Es necesario para asociar la nota con la tarea correcta y para actualizar la cache de consultas relacionada después de actualizar la nota.
 * @returns Un objeto con la información de la mutación, incluyendo el estado de carga, cualquier error y una función para ejecutar la mutación. La función para ejecutar la mutación se puede utilizar en la interfaz de usuario para iniciar el proceso de actualización de la nota cuando el usuario lo desee.
 */
export function useUpdateNote(taskId: Task["id"]) {
  // Obtenemos el cliente de consultas de React Query para poder invalidar las consultas relacionadas después de actualizar una nota
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (noteData: UpdateNoteFormData) =>
      updateNoteAction(taskId, noteData),
    onSuccess: (data) => {
      // Mostramos un mensaje de éxito al usuario después de actualizar la nota
      toast.success("Nota actualizada exitosamente");

      queryClient.setQueryData(
        noteKeys.detail(taskId, data.id),
        (oldData: Note | undefined) => {
          // Si no hay datos anteriores, devolvemos la nota actualizada como nuevo dato en la cache
          if (!oldData) return data;
          // Actualizamos la nota en la cache con los nuevos datos
          return { ...oldData, ...data };
        },
      );

      queryClient.setQueryData(
        noteKeys.lists(taskId),
        (oldData: Note[] | undefined) => {
          // Si no hay datos anteriores, devolvemos un nuevo array con la nota actualizada
          if (!oldData) return [data];
          // Actualizamos la nota en la lista de notas en la cache con los nuevos datos
          return oldData.map((note) =>
            note.id === data.id ? { ...note, ...data } : note,
          );
        },
      );

      queryClient.setQueryData(
        taskKeys.detail(taskId),
        (oldData: TaskDetail | undefined) => {
          // Si no hay datos anteriores, devolvemos el mismo dato sin cambios
          if (!oldData) return oldData;
          // Actualizamos la tarea en la cache para reflejar la nota actualizada en la lista de notas de la tarea
          return {
            ...oldData,
            notes: oldData.notes.map((note) =>
              note.id === data.id ? { ...note, ...data } : note,
            ),
          };
        },
      );

      // Invalidamos la cache de las query keys relacionadas con las notas de esta tarea
      queryClient.invalidateQueries({ queryKey: noteKeys.byTask(taskId) });
      queryClient.invalidateQueries({ queryKey: taskKeys.detail(taskId) });
    },
    onError: (error) => {
      // Mostramos el mensaje de error al usuario si la actualización de la nota falla
      toast.error(error.message);
    },
  });
}

/**
 * @description Hook para eliminar una nota existente en una tarea específica. Utiliza React Query para manejar el estado de la mutación y actualizar la cache de consultas relacionada después de eliminar la nota.
 * @param taskId El ID de la tarea a la que pertenece la nota que se desea eliminar. Es necesario para asociar la nota con la tarea correcta y para actualizar la cache de consultas relacionada después de eliminar la nota.
 * @param noteId El ID de la nota que se desea eliminar. Es necesario para identificar la nota correcta que se desea eliminar y para actualizar la cache de consultas relacionada después de eliminar la nota.
 * @return Un objeto con la información de la mutación, incluyendo el estado de carga, cualquier error y una función para ejecutar la mutación. La función para ejecutar la mutación se puede utilizar en la interfaz de usuario para iniciar el proceso de eliminación de la nota cuando el usuario lo desee.
 */
export function useDeleteNote(taskId: Task["id"]) {
  // Obtenemos el cliente de consultas de React Query para poder invalidar las consultas relacionadas después de eliminar una nota
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (noteId: Note["id"]) => deleteNoteAction(taskId, noteId),
    onMutate: async (noteId) => {
      // Cancelamos cualquier consulta en curso relacionada con las notas para evitar conflictos de actualización de la cache
      await queryClient.cancelQueries({ queryKey: noteKeys.byTask(taskId) });
      await queryClient.cancelQueries({ queryKey: taskKeys.detail(taskId) });

      // Guardamos una copia de los datos anteriores de las notas para poder revertirlos en caso de error
      const previousNotes = queryClient.getQueryData<Note[]>(
        noteKeys.lists(taskId),
      );
      const previousTask = queryClient.getQueryData<TaskDetail>(
        taskKeys.detail(taskId),
      );

      // Actualizamos la cache de consultas para eliminar la nota eliminada de la lista de notas de la tarea
      queryClient.setQueryData(
        noteKeys.lists(taskId),
        (oldData: Note[] | undefined) =>
          oldData ? oldData.filter((note) => note.id !== noteId) : oldData,
      );

      // También actualizamos la cache de la query key de detalle de tarea para reflejar
      // la nota eliminada en la lista de notas de la tarea
      queryClient.setQueryData(
        taskKeys.detail(taskId),
        (oldData: TaskDetail | undefined) => {
          if (!oldData) return oldData;
          return {
            ...oldData,
            notes: oldData.notes.filter((note) => note.id !== noteId),
          };
        },
      );

      // Devolvemos los datos anteriores para poder revertir la cache en caso de error
      return { previousNotes, previousTask };
    },
    onSuccess: () => {
      // Mostramos un mensaje de éxito al usuario después de eliminar la nota
      toast.success("Nota eliminada exitosamente");
    },
    onError: (error, noteId, context) => {
      // Mostramos el mensaje de error al usuario si la eliminación de la nota falla
      toast.error(error.message);

      // Revertimos la cache de consultas a los datos anteriores guardados en el contexto en caso de error
      if (context?.previousNotes) {
        queryClient.setQueryData(noteKeys.lists(taskId), context.previousNotes);
      }
      if (context?.previousTask) {
        queryClient.setQueryData(taskKeys.detail(taskId), context.previousTask);
      }
    },
    onSettled: () => {
      // Invalidamos la cache de las query keys relacionadas con las notas de esta tarea para que se vuelvan a cargar
      queryClient.invalidateQueries({ queryKey: noteKeys.byTask(taskId) });
      queryClient.invalidateQueries({ queryKey: taskKeys.detail(taskId) });
    },
  });
}
