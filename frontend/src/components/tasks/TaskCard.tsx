"use client";

import {
  Menu,
  MenuButton,
  MenuItem,
  MenuItems,
  Transition,
} from "@headlessui/react";
import {
  EllipsisVerticalIcon,
  EyeIcon,
  PencilIcon,
  TrashIcon,
} from "@heroicons/react/20/solid";
import { Fragment, useRef } from "react";
import { useRouter } from "next/navigation";
import { useDraggable } from "@dnd-kit/core";
import { Task } from "@/schemas/task";
import { useDeleteTask } from "@/hooks/tasks/useMutations";
import { useQueryString } from "@/hooks/shared/useQueryString";
import classNames from "@/helpers/classNames";

type TaskCardProps = {
  task: Task;
  canEdit: boolean;
  isOverlay?: boolean;
};

export default function TaskCard({
  task,
  canEdit,
  isOverlay = false,
}: Readonly<TaskCardProps>) {
  // Hook para habilitar la funcionalidad de drag and drop, recibe un ID único (en este caso el ID de la tarea)
  const { attributes, listeners, setNodeRef, transform, isDragging } =
    useDraggable({
      id: task.id,
    });

  // Referencia al elemento del DOM de la tarjeta, para poder manipular su estilo (como el z-index) durante la interacción
  const taskCardRef = useRef<HTMLLIElement>(null);

  // Hook personalizado para manejar la generación de URLs con query strings,
  // nos permite crear URLs dinámicamente para navegar a la vista o edición de la tarea
  const router = useRouter();
  const { createUrl } = useQueryString();

  const { mutate: deleteMutate } = useDeleteTask();

  // Funciones para manejar la navegación a la vista o edición de la tarea,
  // al hacer click en el título o en las opciones del menú respectivamente.
  // Estas funciones generan la URL correspondiente con los parámetros necesarios para mostrar el modal adecuado.
  const editTask = () => {
    ensureZIndex();
    router.push(createUrl({ action: "edit-task", taskId: task.id }), {
      scroll: false,
    });
  };
  const viewTask = () => {
    ensureZIndex();
    router.push(createUrl({ action: "view-task", taskId: task.id }), {
      scroll: false,
    });
  };

  // Estilo dinámico para aplicar la transformación de posición durante el drag,
  // si el elemento está siendo arrastrado, se aplica una transformación CSS
  // para moverlo según las coordenadas proporcionadas por el hook de drag and drop
  const style = transform
    ? {
        transform: `translate3d(${transform.x}px, ${transform.y}px, 0)`,
      }
    : undefined;

  // Función para combinar la referencia del nodo del drag and drop con la referencia local de la tarjeta,
  const setCombinedRef = (el: HTMLLIElement | null) => {
    setNodeRef(el);
    taskCardRef.current = el;
  };

  // Función para asegurar que el z-index del elemento se restablezca después de cerrar el menú de opciones,
  // esto es necesario porque durante la interacción con el menú se puede cambiar el z-index 
  // para asegurar que el menú se muestre por encima de otros elementos, 
  // pero al cerrar el menú queremos que la tarjeta vuelva a su estado normal.
  const ensureZIndex = () => {
    const el = taskCardRef.current;
    if (el) el.style.zIndex = "";
  };

  // Clases dinámicas para manejar la opacidad y el estilo de la tarjeta durante el drag and drop.
  const draggingOpacityClass = isDragging
    ? "opacity-0 scale-105 cursor-grabbing transition-none"
    : "opacity-100 scale-100";
  const opacityClass = isOverlay
    ? "opacity-80 border-2 border-dashed"
    : draggingOpacityClass;

  return (
    <li
      {...listeners}
      {...attributes}
      ref={setCombinedRef}
      style={style}
      className={classNames(
        opacityClass,
        "flex justify-between gap-3 p-5 bg-white border border-slate-300 hover:cursor-grab hover:shadow-md rounded-xl transition-opacity duration-200 relative",
      )}
    >
      <div className="flex flex-col min-w-0 gap-y-4">
        <button
          type="button"
          onPointerDown={(e) => e.stopPropagation()}
          className="text-xl font-bold text-left text-slate-600 hover:cursor-pointer hover:underline"
          onClick={viewTask}
        >
          {task.title}
        </button>
        <p className="text-slate-500">{task.description}</p>
      </div>
      <div className="flex shrink-0 gap-x-6">
        <Menu as="div" className="relative flex-none">
          {({ open }) => {
            const el = taskCardRef.current;
            if (el && open) el.style.zIndex = "70";

            return (
              <>
                <MenuButton
                  onPointerDown={(e) => e.stopPropagation()}
                  className="-m-2.5 block p-2.5 text-gray-500 hover:text-gray-900 hover:cursor-pointer focus:outline-none"
                >
                  <span className="sr-only">opciones de la tarea</span>
                  <EllipsisVerticalIcon
                    className="h-9 w-9"
                    aria-hidden="true"
                  />
                </MenuButton>
                <Transition
                  as={Fragment}
                  enter="transition ease-out duration-100"
                  enterFrom="transform opacity-0 scale-95"
                  enterTo="transform opacity-100 scale-100"
                  leave="transition ease-in duration-75"
                  leaveFrom="transform opacity-100 scale-100"
                  leaveTo="transform opacity-0 scale-95"
                  afterLeave={ensureZIndex}
                >
                  <MenuItems className="absolute right-0 w-56 py-2 mt-2 origin-top-right bg-white rounded-md shadow-lg ring-1 ring-gray-900/5 focus:outline-none">
                    <MenuItem>
                      <button
                        type="button"
                        onPointerDown={(e) => e.stopPropagation()}
                        className="flex items-center w-full px-3 py-1 overflow-hidden text-sm font-semibold leading-6 text-left text-gray-900 transition-colors rounded-md hover:cursor-pointer hover:bg-gray-200"
                        onClick={viewTask}
                      >
                        <EyeIcon className="inline w-5 h-5 mr-2" />
                        Ver Tarea
                      </button>
                    </MenuItem>
                    {canEdit && (
                      <>
                        <MenuItem>
                          <button
                            type="button"
                            onPointerDown={(e) => e.stopPropagation()}
                            className="flex items-center w-full px-3 py-1 overflow-hidden text-sm font-semibold leading-6 text-left text-gray-900 transition-colors rounded-md hover:cursor-pointer hover:bg-gray-200"
                            onClick={editTask}
                          >
                            <PencilIcon className="inline w-5 h-5 mr-2" />
                            Editar Tarea
                          </button>
                        </MenuItem>
                        <MenuItem>
                          <button
                            type="button"
                            onPointerDown={(e) => e.stopPropagation()}
                            className="flex items-center w-full px-3 py-1 overflow-hidden text-sm font-semibold leading-6 text-left text-red-500 transition-colors rounded-md hover:cursor-pointer hover:bg-red-500 hover:text-white"
                            onClick={() => deleteMutate(task.id)}
                          >
                            <TrashIcon className="inline w-5 h-5 mr-2" />
                            Eliminar Tarea
                          </button>
                        </MenuItem>
                      </>
                    )}
                  </MenuItems>
                </Transition>
              </>
            );
          }}
        </Menu>
      </div>
    </li>
  );
}
