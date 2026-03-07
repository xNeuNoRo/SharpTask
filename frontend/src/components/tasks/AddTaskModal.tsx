"use client";

import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import Modal from "@/components/shared/Modal";
import TaskForm from "./TaskForm";
import { CreateTaskSchema, type CreateTaskFormData } from "@/schemas/task";
import { useCreateTask } from "@/hooks/tasks/useMutations";
import { useQueryString } from "@/hooks/shared/useQueryString";

export default function AddTaskModal() {
  // Hook para manejar la navegación y manipulación de URLs con query strings
  const router = useRouter();
  const { searchParams, createUrl } = useQueryString();

  // Obtenemos los parámetros de la URL para determinar si el modal debe mostrarse
  const action = searchParams.get("action");
  const show = action === "new-task";

  // Configuramos el formulario usando React Hook Form, con validación basada en el esquema de Zod
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<CreateTaskFormData>({
    resolver: zodResolver(CreateTaskSchema),
    defaultValues: {
      title: "",
      description: "",
      status: "Pending",
    },
  });

  // Hook para manejar la mutación de creación de la tarea, que se ejecutará al enviar el formulario
  const { mutate: createTask, isPending } = useCreateTask();

  // Función para cerrar el modal, que resetea el formulario y navega a la URL sin los parámetros de creación
  const closeModal = () => {
    reset();
    router.push(createUrl({ action: null }), { scroll: false });
  };

  // Función para manejar el envío del formulario, que llama a la mutación de creación con los datos del formulario,
  // y en caso de éxito, resetea el formulario y cierra el modal
  const handleCreateTask = (formData: CreateTaskFormData) => {
    createTask(formData, {
      onSuccess: () => {
        closeModal();
      },
    });
  };

  return (
    <Modal open={show} close={closeModal} title="Nueva Tarea">
      <p className="text-xl font-light text-gray-500">
        Llena el formulario y crea una{" "}
        <span className="font-bold text-fuchsia-600">tarea</span>
      </p>
      <form
        className="mt-10 space-y-8"
        onSubmit={handleSubmit(handleCreateTask)}
        noValidate
      >
        <input type="hidden" {...register("status")} value="Pending" />

        <TaskForm register={register} errors={errors} />

        <input
          type="submit"
          value={isPending ? "Guardando..." : "Guardar Tarea"}
          disabled={isPending}
          className="w-full p-3 font-bold text-white transition-colors shadow-md cursor-pointer bg-fuchsia-600 hover:bg-fuchsia-700 rounded-xl disabled:opacity-50"
        />
      </form>
    </Modal>
  );
}
