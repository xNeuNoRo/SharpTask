import { z } from "zod";
import { BaseEntitySchema } from ".";

// Esquema para una nota
export const NoteSchema = BaseEntitySchema.extend({
  taskId: z.uuidv4("El ID de tarea debe ser un UUID válido"),
  content: z
    .string()
    .min(1, "El contenido es obligatorio")
    .max(2048, "El contenido no puede tener más de 2048 caracteres"),
});

// Esquema para un array de notas
export const NotesSchema = z.array(NoteSchema);

// Esquema para crear una nota, solo con los campos necesarios
export const CreateNoteSchema = NoteSchema.pick({
  content: true,
});

// Esquema para actualizar una nota, con los mismos campos que para crear
export const UpdateNoteSchema = CreateNoteSchema;

// Inferencia de los tipos TypeScript a partir de los esquemas
export type Note = z.infer<typeof NoteSchema>;
export type CreateNote = z.infer<typeof CreateNoteSchema>;
export type UpdateNote = z.infer<typeof UpdateNoteSchema>;
