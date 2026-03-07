import { z } from "zod";
import { BaseEntitySchema } from ".";
import { NoteSchema } from "./notes";

// ==============================================
// Esquemas estructuras relacionadas con tareas y su estado
// ==============================================

// Esquema para el estado de una tarea
export const TaskStatusEnum = z.enum(
  ["Pending", "OnHold", "InProgress", "UnderReview", "Completed"],
  {
    error: "Estado de tarea no válido",
  },
);

// Esquema para los cambios de estado de una tarea
export const TaskChangeSchema = z.object({
  status: TaskStatusEnum,
  changedAt: z.string().refine((date) => !Number.isNaN(Date.parse(date)), {
    message: "La fecha de cambio debe ser una fecha válida",
  }),
});

// Esquema para una tarea
export const TaskSchema = BaseEntitySchema.extend({
  title: z
    .string()
    .min(1, "El título es obligatorio")
    .max(256, "El título no puede tener más de 256 caracteres"),
  description: z
    .string()
    .min(1, "La descripción es obligatoria")
    .max(1024, "La descripción no puede tener más de 1024 caracteres"),
  status: TaskStatusEnum,
  changes: z.array(TaskChangeSchema).default([]),
});

// Esquema para un array de tareas
export const TasksSchema = z.array(TaskSchema);

// Esquema para los detalles de una tarea, incluyendo sus notas
export const TaskDetailSchema = TaskSchema.extend({
  notes: z.array(NoteSchema).default([]),
});

// ==============================================
// Esquemas para mutacion de tareas
// ==============================================

// Esquema para crear una tarea, solo con los campos necesarios
export const CreateTaskSchema = TaskSchema.pick({
  title: true,
  description: true,
  status: true,
});

// Esquema para actualizar una tarea, con los mismos campos que para crear
export const UpdateTaskSchema = CreateTaskSchema;

// Esquema para actualizar solo el estado de una tarea
export const UpdateTaskStatusSchema = TaskSchema.pick({
  status: true,
});

// ==============================================
// Tipos TypeScript inferidos a partir de los esquemas
// ==============================================

// Inferencia de los tipos TypeScript a partir de los esquemas estructurales
export type Task = z.infer<typeof TaskSchema>;
export type TaskStatus = z.infer<typeof TaskStatusEnum>;
export type TaskDetail = z.infer<typeof TaskDetailSchema>;
export type TaskChange = z.infer<typeof TaskChangeSchema>;

// Inferencia de los tipos TypeScript para las mutaciones
export type CreateTask = z.infer<typeof CreateTaskSchema>;
export type UpdateTask = z.infer<typeof UpdateTaskSchema>;
export type UpdateTaskStatus = z.infer<typeof UpdateTaskStatusSchema>;
