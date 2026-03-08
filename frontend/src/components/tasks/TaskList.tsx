"use client";

import {
  DndContext,
  DragOverlay,
  defaultDropAnimation,
  type DragEndEvent,
} from "@dnd-kit/core";
import {
  restrictToFirstScrollableAncestor,
  restrictToWindowEdges,
} from "@dnd-kit/modifiers";
import { Task } from "@/schemas/task";
import TaskCard from "@/components/tasks/TaskCard";
import { statusTranslations } from "@/locales/es";
import DropTask from "./DropTask";
import ActiveTaskCard from "./ActiveTaskCard";
import { useAppStore } from "@/stores/useAppStore";
import { useUpdateTaskStatus } from "@/hooks/tasks/useMutations";
import { useId, useMemo } from "react";

type TaskListProps = {
  tasks: Task[];
  canEdit: boolean;
};

// Tipo para las tareas agrupadas por estado, asi tenemos un tipo claro y centralizado para esa estructura
type GroupedTasks = {
  [key: string]: Task[];
};

// Estilos para los bordes de cada estado, asi lo tenemos centralizado y fácil de modificar
const statusStyles: { [key: string]: string } = {
  Pending: "border-t-slate-500",
  OnHold: "border-t-red-500",
  InProgress: "border-t-blue-500",
  UnderReview: "border-t-amber-500",
  Completed: "border-t-emerald-500",
};

export default function TaskList({ tasks, canEdit }: Readonly<TaskListProps>) {
  // Obtenemos el estado de animación y la función para cambiarlo desde el store global
  const { disableAnimation, setDisableAnimation } = useAppStore();
  // Hook para mutar el estado de una tarea
  const { mutate: updateTaskStatus } = useUpdateTaskStatus();

  // Generamos un ID único fijo para este contexto de DnD (asi no hay desync entre el server y cliente)
  const dndId = useId();

  // Agrupamos las tareas por su estado, usando reduce para construir un objeto con arrays de tareas por cada estado
  const groupedTasks = useMemo(() => {
    // Creamos un objeto inicial con las claves de cada estado y arrays vacíos
    const groups: GroupedTasks = {
      Pending: [],
      OnHold: [],
      InProgress: [],
      UnderReview: [],
      Completed: [],
    };

    // Iteramos sobre las tareas y las agregamos al array correspondiente según su estado
    tasks.forEach((task) => {
      if (groups[task.status]) {
        groups[task.status].push(task);
      }
    });

    // Devolvemos el objeto con las tareas agrupadas por estado
    return groups;
  }, [tasks]);

  // Función que se ejecuta al finalizar un drag, recibe el evento con la información de la tarea arrastrada y el destino
  const handleDragEnd = (e: DragEndEvent) => {
    // Extraemos la información del evento, especialmente el ID de la tarea arrastrada y el estado destino
    const { over, active } = e;

    // Si hay un destino válido (es decir, se soltó sobre un área que acepta drops), actualizamos el estado de la tarea
    if (over?.id) {
      // Convertimos el ID de la tarea a string (ya que el DnD puede manejar IDs de diferentes tipos)
      // y obtenemos el nuevo estado del ID del destino
      const taskId = active.id.toString();

      // El nuevo estado se obtiene del ID del destino,
      // que en este caso es el mismo que el estado de la tarea (Pending, OnHold, etc)
      const status = over.id as Task["status"];

      // Deshabilitamos la animación temporalmente para evitar que se ejecute durante la actualización del estado,
      setDisableAnimation(true);

      // Llamamos a la mutación para actualizar el estado de la tarea, pasando el ID y el nuevo estado,
      updateTaskStatus(
        { id: taskId, status },
        {
          onSuccess: () => {
            // Una vez que la actualización es exitosa, reactivamos la animación para que se ejecute en futuros drags
            setDisableAnimation(false);
          },
        },
      );
    }
  };

  return (
    <>
      <h2 className="my-10 text-3xl font-black">Tareas</h2>

      <div className="flex max-h-screen gap-5 pb-32 pr-2 overflow-x-scroll rounded-xl 2xl:overflow-auto">
        <DndContext
          id={dndId}
          onDragEnd={handleDragEnd}
          modifiers={[restrictToFirstScrollableAncestor, restrictToWindowEdges]}
        >
          {Object.entries(groupedTasks).map(([status, groupTasks]) => (
            <div key={status} className="min-w-75 2xl:min-w-0 2xl:w-1/5">
              <h3
                className={`capitalize text-xl font-light border border-slate-300 bg-white p-3 border-t-8 rounded-xl ${statusStyles[status]}`}
              >
                {statusTranslations[status]}
              </h3>
              <DropTask status={status} />
              <ul className="mt-5 space-y-5">
                {groupTasks.length === 0 ? (
                  <li className="pt-3 text-center text-gray-500">
                    No hay tareas
                  </li>
                ) : (
                  groupTasks.map((task) => (
                    <TaskCard key={task.id} task={task} canEdit={canEdit} />
                  ))
                )}
              </ul>
            </div>
          ))}
          <DragOverlay
            dropAnimation={
              disableAnimation
                ? null
                : {
                    ...defaultDropAnimation,
                    duration: 500,
                    easing: "ease-out",
                  }
            }
          >
            <ActiveTaskCard tasks={tasks} canEdit={canEdit} />
          </DragOverlay>
        </DndContext>
      </div>
    </>
  );
}
