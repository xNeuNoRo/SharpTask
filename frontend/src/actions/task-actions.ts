"use server";

import {
  completeTask,
  createTask,
  deleteTask,
  updateTask,
  updateTaskStatus,
} from "@/api/TasksAPI";
import {
  CreateTaskFormData,
  Task,
  UpdateTaskFormData,
  UpdateTaskStatusFormData,
} from "@/schemas/task";
import { revalidatePath } from "next/cache";

/**
 * @description Esta función se encarga de crear una nueva tarea utilizando los datos proporcionados.
 * Después de crear la tarea, se revalida la ruta para actualizar la interfaz de usuario con la nueva lista de tareas.
 * @param taskData - Un objeto que contiene los datos necesarios para crear una nueva tarea, como el título, la descripción, etc.
 * @returns  La tarea creada, que puede ser utilizada en la interfaz de usuario para mostrar la nueva tarea o para otras operaciones relacionadas.
 */
export async function createTaskAction(
  taskData: CreateTaskFormData,
): Promise<Task> {
  // Llamamos a la función de la API para crear la tarea con los datos proporcionados
  const task = await createTask(taskData);
  // Después de crear la tarea, revalidamos la ruta para actualizar la lista de tareas en la interfaz de usuario
  revalidatePath("/");
  // Devolvemos la tarea creada para que pueda ser utilizada en la interfaz de usuario si es necesario
  return task;
}

/**
 * @description Esta función se encarga de actualizar una tarea existente utilizando los datos proporcionados.
 * @param taskData - Un objeto que contiene los datos necesarios para actualizar una tarea, incluyendo el ID de la tarea a actualizar y los campos que se desean modificar.
 * @returns La tarea actualizada, que puede ser utilizada en la interfaz de usuario para mostrar los cambios realizados o para otras operaciones relacionadas.
 */
export async function updateTaskAction(
  taskData: UpdateTaskFormData,
): Promise<Task> {
  // Llamamos a la función de la API para actualizar la tarea con el ID y los datos de actualización proporcionados
  const updatedTask = await updateTask(taskData);
  // Después de actualizar la tarea, revalidamos la ruta para actualizar la interfaz de usuario
  revalidatePath("/");
  // Devolvemos la tarea actualizada para que pueda ser utilizada en la interfaz de usuario si es necesario
  return updatedTask;
}

/**
 * @description Esta función se encarga de actualizar el estado de una tarea existente utilizando los datos proporcionados.
 * @param taskData - Un objeto que contiene los datos necesarios para actualizar el estado de una tarea, incluyendo el ID de la tarea a actualizar y el nuevo estado que se desea asignar.
 * @returns La tarea actualizada con el nuevo estado, que puede ser utilizada en la interfaz de usuario para mostrar los cambios realizados o para otras operaciones relacionadas.
 */
export async function updateTaskStatusAction(
  taskData: UpdateTaskStatusFormData,
): Promise<Task> {
  // Llamamos a la función de la API para actualizar el estado de la tarea con el ID y los datos de actualización proporcionados
  const updatedTask = await updateTaskStatus(taskData);
  // Después de actualizar el estado de la tarea, revalidamos la ruta para actualizar la interfaz de usuario
  revalidatePath("/");
  // Devolvemos la tarea actualizada para que pueda ser utilizada en la interfaz de usuario si es necesario
  return updatedTask;
}

/**
 * @description Esta función se encarga de completar una tarea existente utilizando su ID.
 * @param taskId - El ID de la tarea que se desea completar.
 * @returns La tarea completada, que puede ser utilizada en la interfaz de usuario para mostrar los cambios realizados o para otras operaciones relacionadas.
 */
export async function completeTaskAction(taskId: Task["id"]): Promise<Task> {
  // Llamamos a la función de la API para actualizar el estado de la tarea a completada utilizando el ID de la tarea
  const updatedTask = await completeTask(taskId);
  // Después de completar la tarea, revalidamos la ruta para actualizar la interfaz de usuario
  revalidatePath("/");
  // Devolvemos la tarea actualizada para que pueda ser utilizada en la interfaz de usuario si es necesario
  return updatedTask;
}

/**
 * @description Esta función se encarga de eliminar una tarea existente utilizando su ID.
 * @param taskId - El ID de la tarea que se desea eliminar.
 */
export async function deleteTaskAction(taskId: Task["id"]): Promise<void> {
  // Llamamos a la función de la API para eliminar la tarea con el ID proporcionado
  await deleteTask(taskId);
  // Después de eliminar la tarea, revalidamos la ruta para actualizar la interfaz de usuario
  revalidatePath("/");
}
