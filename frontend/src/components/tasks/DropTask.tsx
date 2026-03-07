"use client";

import { useDroppable } from "@dnd-kit/core";
import classNames from "@/helpers/classNames";

type DropTaskProps = {
  status: string;
};

export default function DropTask({ status }: Readonly<DropTaskProps>) {
  // Hook para habilitar la funcionalidad de droppable, recibe un ID único (en este caso el nombre del estado)
  const { isOver, setNodeRef } = useDroppable({
    id: status,
  });

  // Clases dinámicas para manejar la opacidad y el estilo del área de drop durante la interacción de drag and drop.
  return (
    <div
      ref={setNodeRef}
      className={classNames(
        isOver
          ? "opacity-40 bg-green-100 border-green-400 text-black"
          : "opacity-100",
        "transition-opacity duration-200 grid p-2 mt-5 text-xs font-semibold uppercase border border-dashed place-content-center text-slate-500 rounded-xl",
      )}
    >
      Soltar tarea aquí
    </div>
  );
}
