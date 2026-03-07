"use client";

import { useForm } from "react-hook-form";
import ErrorMessage from "@/components/shared/ErrorMessage";
import { CreateNoteFormData } from "@/schemas/notes";
import { Task } from "@/schemas/task";
import { useCreateNote } from "@/hooks/notes/useMutations";

type AddNoteFormProps = {
  taskId: Task["id"];
};

export default function AddNoteForm({ taskId }: Readonly<AddNoteFormProps>) {
  // Valores iniciales para el formulario de creación de nota, con un campo de contenido vacío
  const initialValues: CreateNoteFormData = {
    content: "",
  };

  // Configuramos el formulario usando React Hook Form, con los valores iniciales definidos anteriormente
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<CreateNoteFormData>({ defaultValues: initialValues });

  // Hook para manejar la mutación de creación de nota, que se ejecutará al enviar el formulario
  const { mutate: createNote, isPending } = useCreateNote(taskId);

  // Función para manejar el envío del formulario, que llama a la mutación de creación con los datos del formulario,
  // y en caso de éxito, resetea el formulario para permitir crear otra nota fácilmente
  const handleAddNote = (formData: CreateNoteFormData) => {
    createNote(formData, {
      onSuccess: () => {
        reset();
      },
    });
  };

  return (
    <form
      onSubmit={handleSubmit(handleAddNote)}
      className="space-y-3"
      noValidate
    >
      <div className="flex flex-col">
        <label
          className="block mb-2 text-sm font-medium text-gray-700"
          htmlFor="content"
        >
          Crear Nota
        </label>
        <textarea
          id="content"
          placeholder="Contenido de la nota"
          className="w-full p-3 border border-gray-300 rounded-md"
          {...register("content", { required: "El contenido es obligatorio" })}
        />

        {errors.content && (
          <ErrorMessage>{errors.content.message as string}</ErrorMessage>
        )}
      </div>

      <input
        type="submit"
        value={isPending ? "Creando..." : "Crear Nota"}
        disabled={isPending}
        className="w-full p-2 font-black text-white transition-colors cursor-pointer bg-fuchsia-600 hover:bg-fuchsia-700 rounded-xl disabled:opacity-50"
      />
    </form>
  );
}
