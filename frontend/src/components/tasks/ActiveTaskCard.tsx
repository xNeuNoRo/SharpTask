"use client";

import { useDndContext } from "@dnd-kit/core";
import TaskCard from "./TaskCard";
import MotionElement from "@/components/shared/MotionElement";
import { AnimatePresence } from "framer-motion";
import { useAppStore } from "@/stores/useAppStore";
import { Task } from "@/schemas/task";

type ActiveTaskCardProps = {
  tasks: Task[];
  canEdit: boolean;
};

export default function ActiveTaskCard({
  tasks,
  canEdit,
}: Readonly<ActiveTaskCardProps>) {
  // Obtenemos el estado de animación y la función para cambiarlo desde el store global
  const disabledAnimation = useAppStore((state) => state.disableAnimation);

  // Obtenemos el contexto de DnD para saber cuál es la tarea actualmente activa (la que se está arrastrando)
  const { active } = useDndContext();

  // Buscamos la tarea activa en la lista de tareas, si no hay una tarea activa o no se encuentra, el resultado será null
  const task = active ? tasks.find((task) => task.id === active.id) : null;

  return (
    <AnimatePresence mode="wait">
      {task && (
        <MotionElement
          as="div"
          key={`${task.id}-${disabledAnimation ? "noanim" : "anim"}`}
          initial={false}
          animate={{ opacity: 1, scale: 1 }}
          exit={{
            opacity: 0.5,
            scale: 0.95,
            transition: disabledAnimation
              ? { duration: 0 }
              : { duration: 1.5, ease: "easeOut" },
          }}
          transition={
            disabledAnimation
              ? { duration: 0 }
              : { type: "spring", stiffness: 400, damping: 25 }
          }
        >
          <TaskCard task={task} canEdit={canEdit} isOverlay={true} />
        </MotionElement>
      )}
    </AnimatePresence>
  );
}
