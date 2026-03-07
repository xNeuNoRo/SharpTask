"use client";

import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import Modal from "@/components/shared/Modal";
import TaskForm from "./TaskForm";
import { UpdateTaskSchema, type UpdateTaskFormData } from "@/schemas/task";
import { useTask } from "@/hooks/tasks/useQueries";
import { useUpdateTask } from "@/hooks/tasks/useMutations";
import { useQueryString } from "@/hooks/shared/useQueryString";

export default function EditTaskModal() {
  // Hook para manejar la navegación y manipulación de URLs con query strings
  const router = useRouter();
  const { searchParams, createUrl } = useQueryString();

  // Obtenemos los parámetros de la URL para determinar si el modal debe mostrarse y qué tarea se está editando
  const action = searchParams.get("action");
  const taskId = searchParams.get("taskId");
  const show = action === "edit-task" && !!taskId;

  // Obtenemos los datos de la tarea a editar usando un hook personalizado que hace una consulta a la API
  const { data: task, isError } = useTask(taskId ?? "");

  // Configuramos el formulario usando React Hook Form, con validación basada en el esquema de Zod
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<UpdateTaskFormData>({
    resolver: zodResolver(UpdateTaskSchema),
    values:
      task && taskId
        ? {
            id: taskId,
            title: task.title,
            description: task.description,
            status: task.status,
          }
        : undefined,
  });

  // Hook para manejar la mutación de actualización de la tarea, que se ejecutará al enviar el formulario
  const { mutate: updateTask, isPending } = useUpdateTask();

  // Función para cerrar el modal, que resetea el formulario y navega a la URL sin los parámetros de edición
  const closeModal = () => {
    reset();
    router.push(createUrl({ action: null, taskId: null }), { scroll: false });
  };

  // Función para manejar el envío del formulario, que llama a la mutación de actualización con los datos del formulario,
  // y en caso de éxito, cierra el modal.
  const handleUpdateTask = (formData: UpdateTaskFormData) => {
    updateTask(formData, {
      onSuccess: () => {
        closeModal();
      },
    });
  };

  // Si hay un error al obtener la tarea, cerramos el modal para evitar mostrar un estado inconsistente,
  // esto puede ocurrir si el ID de la tarea no es válido o si hay un problema con la consulta.
  if (isError) {
    closeModal();
    return null;
  }

  return (
    <Modal open={show} close={closeModal} title="Editar Tarea">
      <p className="text-xl font-light text-gray-500">
        Realiza cambios en una{" "}
        <span className="font-bold text-fuchsia-600">tarea</span>
      </p>
      <form
        className="mt-10 space-y-8"
        onSubmit={handleSubmit(handleUpdateTask)}
        noValidate
      >
        <input type="hidden" {...register("id")} />

        <TaskForm register={register} errors={errors} />

        <input
          type="submit"
          value={isPending ? "Guardando Cambios..." : "Guardar Cambios"}
          disabled={isPending}
          className="w-full p-3 font-bold text-white transition-colors shadow-md cursor-pointer bg-fuchsia-600 hover:bg-fuchsia-700 rounded-xl disabled:opacity-50"
        />
      </form>
    </Modal>
  );
}
