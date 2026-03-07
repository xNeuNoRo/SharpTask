import { handleApiError } from "@/helpers/handleApiError";
import { validateApiRes } from "@/helpers/validateApiRes";
import { api } from "@/lib/axios";
import {
  CreateTaskFormData,
  Task,
  TaskSchema,
  TasksSchema,
  UpdateTaskFormData,
  UpdateTaskStatusFormData,
} from "@/schemas/task";

// recurso de la API para tareas
const RESOURCE = "/tasks";

// ==============================================
// Funciones basicas del CRUD para tareas
// ==============================================

/**
 * @description Obtiene la lista de tareas desde la API, valida la respuesta y maneja errores
 * @returns Una promesa que resuelve con un array de tareas validadas o rechaza con un error manejado
 */
export async function getTasks(): Promise<Task[]> {
  try {
    // Realiza la solicitud GET al recurso de tareas
    const { data } = await api.get(RESOURCE);
    // Valida la respuesta de la API contra el esquema de tareas y devuelve los datos validados
    return validateApiRes(data, TasksSchema);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud o validación
    handleApiError(err);
  }
}

/**
 * @description Obtiene una tarea específica por su ID desde la API, valida la respuesta y maneja errores
 * @param id El ID de la tarea a obtener, debe ser un UUID válido según el esquema de tarea
 * @returns Una promesa que resuelve con la tarea validadas o rechaza con un error manejado
 */
export async function getTaskById(id: Task["id"]): Promise<Task> {
  try {
    // Realiza la solicitud GET al recurso de tareas con el ID específico
    const { data } = await api.get(`${RESOURCE}/${id}`);
    // Valida la respuesta de la API contra el esquema de tarea y devuelve los datos validados
    return validateApiRes(data, TaskSchema);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud o validación
    handleApiError(err);
  }
}

/**
 * @description Crea una nueva tarea enviando los datos a la API, valida la respuesta y maneja errores
 * @param taskData Los datos de la nueva tarea a crear, debe cumplir con el esquema de CreateTaskFormData
 * @returns Una promesa que resuelve con la tarea creada y validada o rechaza con un error manejado
 */
export async function createTask(taskData: CreateTaskFormData): Promise<Task> {
  try {
    // Realiza la solicitud POST al recurso de tareas con los datos de la nueva tarea
    const { data } = await api.post(RESOURCE, taskData);
    // Valida la respuesta de la API contra el esquema de tarea y devuelve los datos validados
    return validateApiRes(data, TaskSchema);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud o validación
    handleApiError(err);
  }
}

/**
 * @description Actualiza una tarea existente enviando los datos a la API, valida la respuesta y maneja errores
 * @param taskData Los datos de la tarea a actualizar, debe incluir el ID y cumplir con el esquema de UpdateTaskFormData
 * @returns Una promesa que resuelve con la tarea actualizada y validada o rechaza con un error manejado
 */
export async function updateTask(taskData: UpdateTaskFormData): Promise<Task> {
  // Extraemos el ID del objeto de datos de la tarea y preparamos los datos de actualización sin el ID
  const { id, ...updateData } = taskData;
  try {
    // Realiza la solicitud PUT al recurso de tareas con el ID específico y los datos de actualización
    const { data } = await api.put(`${RESOURCE}/${id}`, updateData);
    // Valida la respuesta de la API contra el esquema de tarea y devuelve los datos validados
    return validateApiRes(data, TaskSchema);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud o validación
    handleApiError(err);
  }
}

/**
 * @description Elimina una tarea específica por su ID desde la API, valida la respuesta y maneja errores
 * @param id El ID de la tarea a eliminar, debe ser un UUID válido según el esquema de tarea
 * @returns Una promesa que resuelve con void o rechaza con un error manejado
 */
export async function deleteTask(id: Task["id"]): Promise<void> {
  try {
    // Realiza la solicitud DELETE al recurso de tareas con el ID específico
    await api.delete(`${RESOURCE}/${id}`);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud
    handleApiError(err);
  }
}

// ==============================================
// Funciones adicionales para tareas
// ==============================================

/**
 * @description Actualiza solo el estado de una tarea enviando los datos a la API, valida la respuesta y maneja errores
 * @param taskData Los datos de la tarea a actualizar,
 * debe incluir el ID y el nuevo estado, y cumplir con el esquema de UpdateTaskStatusFormData
 * @returns Una promesa que resuelve con la tarea actualizada y validada o rechaza con un error manejado
 */
export async function updateTaskStatus(
  taskData: UpdateTaskStatusFormData,
): Promise<Task> {
  // Extraemos el ID del objeto de datos de la tarea y preparamos los datos de actualización sin el ID
  const { id, ...updateData } = taskData;
  try {
    // Realiza la solicitud PATCH al recurso de tareas con el ID específico y los datos de actualización del estado
    const { data } = await api.patch(`${RESOURCE}/${id}/status`, updateData);
    // Valida la respuesta de la API contra el esquema de tarea y devuelve los datos validados
    return validateApiRes(data, TaskSchema);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud o validación
    handleApiError(err);
  }
}

/**
 * @description Marca una tarea como completada enviando una solicitud a la API, valida la respuesta y maneja errores
 * @param id El ID de la tarea a marcar como completada, debe ser un UUID válido según el esquema de tarea
 * @returns Una promesa que resuelve con la tarea actualizada y validada o rechaza con un error manejado
 */
export async function completeTask(id: Task["id"]): Promise<Task> {
  try {
    // Realiza la solicitud PATCH al recurso de tareas con el ID específico para marcarla como completada
    const { data } = await api.patch(`${RESOURCE}/${id}/complete`);
    // Valida la respuesta de la API contra el esquema de tarea y devuelve los datos validados
    return validateApiRes(data, TaskSchema);
  } catch (err) {
    // Maneja cualquier error que ocurra durante la solicitud o validación
    handleApiError(err);
  }
}
