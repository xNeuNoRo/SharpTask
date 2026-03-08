"use server";

import { createNote, deleteNote, updateNote } from "@/api/NotesAPI";
import { CreateNoteFormData, Note, UpdateNoteFormData } from "@/schemas/notes";
import { revalidatePath } from "next/cache";

/**
 * @description Esta función se encarga de crear una nueva nota asociada a una tarea específica utilizando los datos proporcionados.
 * @param taskId - El ID de la tarea a la que se asociará la nueva nota, debe ser un UUID válido según el esquema de nota.
 * @param noteData - Un objeto que contiene los datos necesarios para crear una nueva nota, como el contenido de la nota, etc., debe cumplir con el esquema de CreateNoteFormData.
 * @returns La nota creada, que puede ser utilizada en la interfaz de usuario para mostrar la nueva nota o para otras operaciones relacionadas. Después de crear la nota, se revalida la ruta para actualizar la interfaz de usuario con la nueva lista de notas asociadas a la tarea.
 */
export async function createNoteAction(
  taskId: Note["taskId"],
  noteData: CreateNoteFormData,
): Promise<Note> {
  // Llamamos a la función de la API para crear la nota con los datos proporcionados
  const note = await createNote(taskId, noteData);
  // Después de crear la nota, revalidamos la ruta para actualizar la lista de notas en la interfaz de usuario
  revalidatePath(`/tasks/${taskId}`, "layout");
  // Devolvemos la nota creada para que pueda ser utilizada en la interfaz de usuario si es necesario
  return note;
}

/**
 * @description Esta función se encarga de actualizar una nota existente asociada a una tarea específica utilizando los datos proporcionados.
 * @param taskId - El ID de la tarea a la que pertenece la nota, debe ser un UUID válido según el esquema de nota.
 * @param noteData - Un objeto que contiene los datos necesarios para actualizar una nota, incluyendo el ID de la nota a actualizar y los campos que se desean modificar, debe cumplir con el esquema de UpdateNoteFormData.
 * @returns La nota actualizada, que puede ser utilizada en la interfaz de usuario para mostrar los cambios realizados o para otras operaciones relacionadas. Después de actualizar la nota, se revalida la ruta para actualizar la interfaz de usuario con la nueva lista de notas asociadas a la tarea.
 */
export async function updateNoteAction(
  taskId: Note["taskId"],
  noteData: UpdateNoteFormData,
): Promise<Note> {
  // Llamamos a la función de la API para actualizar la nota con los datos proporcionados
  const updatedNote = await updateNote(taskId, noteData);
  // Después de actualizar la nota, revalidamos la ruta para actualizar la interfaz de usuario
  revalidatePath(`/tasks/${taskId}`, "layout");
  // Devolvemos la nota actualizada para que pueda ser utilizada en la interfaz de usuario si es necesario
  return updatedNote;
}

/**
 * @description Esta función se encarga de eliminar una nota existente asociada a una tarea específica utilizando su ID. Después de eliminar la nota, se revalida la ruta para actualizar la interfaz de usuario con la nueva lista de notas asociadas a la tarea.
 * @param taskId - El ID de la tarea a la que pertenece la nota, debe ser un UUID válido según el esquema de nota.
 * @param noteId - El ID de la nota que se desea eliminar, debe ser un UUID válido según el esquema de nota.
 */
export async function deleteNoteAction(
  taskId: Note["taskId"],
  noteId: Note["id"],
): Promise<void> {
  // Llamamos a la función de la API para eliminar la nota con el ID proporcionado
  await deleteNote(taskId, noteId);
  // Después de eliminar la nota, revalidamos la ruta para actualizar la interfaz de usuario
  revalidatePath(`/tasks/${taskId}`, "layout");
}
