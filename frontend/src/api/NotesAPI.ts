import { handleApiError } from "@/helpers/handleApiError";
import { validateApiRes } from "@/helpers/validateApiRes";
import { api } from "@/lib/axios";
import {
  CreateNoteFormData,
  Note,
  NoteSchema,
  NotesSchema,
  UpdateNoteFormData,
} from "@/schemas/notes";

// recurso de la API para notas, con función para generar la URL con el ID de tarea
const RESOURCE = (id: Note["taskId"]) => `/tasks/${id}/notes`;

// ==============================================
// Funciones basicas del CRUD para notas
// ==============================================

/**
 * @description Obtiene las notas asociadas a una tarea específica por su ID desde la API, valida la respuesta y maneja errores
 * @param taskId El ID de la tarea para la cual se desean obtener las notas, debe ser un UUID válido según el esquema de nota
 * @returns Una promesa que resuelve con un array de notas validadas o rechaza con un error manejado
 */
export async function getNotesByTaskId(
  taskId: Note["taskId"],
): Promise<Note[]> {
  try {
    // Realiza la solicitud GET al recurso de notas para la tarea específica
    const { data } = await api.get(RESOURCE(taskId));
    // Valida la respuesta de la API contra el esquema de notas y devuelve los datos validados
    return validateApiRes(data, NotesSchema);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud o validación
    handleApiError(err);
  }
}

/**
 * @description Obtiene una nota específica por su ID y el ID de la tarea asociada desde la API, valida la respuesta y maneja errores
 * @param taskId El ID de la tarea a la que pertenece la nota, debe ser un UUID válido según el esquema de nota
 * @param noteId El ID de la nota a obtener, debe ser un UUID válido según el esquema de nota
 * @returns Una promesa que resuelve con la nota validadas o rechaza con un error manejado
 */
export async function getNoteById(
  taskId: Note["taskId"],
  noteId: Note["id"],
): Promise<Note> {
  try {
    // Realiza la solicitud GET al recurso de notas para la tarea específica y el ID de nota
    const { data } = await api.get(`${RESOURCE(taskId)}/${noteId}`);
    // Valida la respuesta de la API contra el esquema de nota y devuelve los datos validados
    return validateApiRes(data, NoteSchema);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud o validación
    handleApiError(err);
  }
}

/**
 * @description Crea una nueva nota asociada a una tarea específica enviando los datos a la API, valida la respuesta y maneja errores
 * @param taskId El ID de la tarea a la que se asociará la nueva nota, debe ser un UUID válido según el esquema de nota
 * @param noteData Los datos de la nueva nota a crear, debe cumplir con el esquema de CreateNoteFormData
 * @returns Una promesa que resuelve con la nota creada y validada o rechaza con un error manejado
 */
export async function createNote(
  taskId: Note["taskId"],
  noteData: CreateNoteFormData,
): Promise<Note> {
  try {
    // Realiza la solicitud POST al recurso de notas para la tarea específica con los datos de la nueva nota
    const { data } = await api.post(RESOURCE(taskId), noteData);
    // Valida la respuesta de la API contra el esquema de nota y devuelve los datos validados
    return validateApiRes(data, NoteSchema);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud o validación
    handleApiError(err);
  }
}

/**
 * @description Actualiza una nota existente asociada a una tarea específica enviando los datos a la API, valida la respuesta y maneja errores
 * @param taskId El ID de la tarea a la que pertenece la nota, debe ser un UUID válido según el esquema de nota
 * @param noteData Los datos de la nota a actualizar, debe cumplir con el esquema de UpdateNoteFormData
 * @returns Una promesa que resuelve con la nota actualizada y validada o rechaza con un error manejado
 */
export async function updateNote(
  taskId: Note["taskId"],
  noteData: UpdateNoteFormData,
): Promise<Note> {
  // Extraemos el ID de la nota del objeto de datos para usarlo en la URL, y el resto de los datos para enviarlos en el cuerpo de la solicitud
  const { id, ...updateData } = noteData;
  try {
    // Realiza la solicitud PUT al recurso de notas para la tarea específica y el ID de nota con los datos de actualización
    const { data } = await api.put(`${RESOURCE(taskId)}/${id}`, updateData);
    // Valida la respuesta de la API contra el esquema de nota y devuelve los datos validados
    return validateApiRes(data, NoteSchema);
  } catch (err) {
    handleApiError(err);
  }
}

/**
 * @description Elimina una nota específica asociada a una tarea específica enviando la solicitud a la API, maneja errores
 * @param taskId El ID de la tarea a la que pertenece la nota, debe ser un UUID válido según el esquema de nota
 * @param noteId El ID de la nota a eliminar, debe ser un UUID válido según el esquema de nota
 * @returns Una promesa que resuelve sin valor si la eliminación es exitosa o rechaza con un error manejado
 */
export async function deleteNote(
  taskId: Note["taskId"],
  noteId: Note["id"],
): Promise<void> {
  try {
    // Realiza la solicitud DELETE al recurso de notas para la tarea específica y el ID de nota
    await api.delete(`${RESOURCE(taskId)}/${noteId}`);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud
    handleApiError(err);
  }
}
