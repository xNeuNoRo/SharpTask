import { Note } from "@/schemas/notes";
import { Task } from "@/schemas/task";

// Este archivo define las query keys para las consultas con React Query
// (Asi centralizamos las keys para evitar errores de tipeo y facilitar su mantenimiento)

// Query keys para tareas
export const taskKeys = {
  all: ["tasks"] as const,
  listsBase: () => [...taskKeys.all, "lists"] as const,
  lists: (status?: Task["status"]) =>
    [...taskKeys.listsBase(), status] as const,
  detail: (id: Task["id"]) => [...taskKeys.all, "detail", id] as const,
  search: (query: string) => [...taskKeys.all, "search", query] as const,
};

// Query keys para notas
export const noteKeys = {
  all: ["notes"] as const,
  byTask: (taskId: Task["id"]) => [...noteKeys.all, taskId] as const,
  lists: (taskId: Task["id"]) => [...noteKeys.byTask(taskId), "lists"] as const,
  detail: (taskId: Task["id"], noteId: Note["id"]) =>
    [...noteKeys.byTask(taskId), "detail", noteId] as const,
};
