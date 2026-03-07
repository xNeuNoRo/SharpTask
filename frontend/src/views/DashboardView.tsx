"use client";

import TaskList from "@/components/tasks/TaskList";
import AddTaskModal from "@/components/tasks/AddTaskModal";
import EditTaskModal from "@/components/tasks/EditTaskModal";
import TaskDetailsModal from "@/components/tasks/TaskDetailsModal";
import { useTasks } from "@/hooks/tasks/useQueries";
import { useQueryString } from "@/hooks/shared/useQueryString";
import { Suspense } from "react";
import Link from "next/link";

export default function DashboardView() {
  // Hook para manejar la navegación y manipulación de URLs con query strings
  const { createUrl } = useQueryString();

  // Hook personalizado para obtener la lista de tareas desde la API,
  // que nos proporciona el estado de carga, error y los datos de las tareas
  const { data: tasks, isLoading, isError } = useTasks();

  // Si hay un error al cargar las tareas, mostramos un mensaje de error
  if (isError)
    return (
      <div className="p-10 text-center text-red-500">
        Error al cargar las tareas
      </div>
    );

  return (
    <>
      <div className="flex flex-col items-start gap-4 lg:gap-16 lg:flex-row sm:items-center">
        <div className="flex-1">
          <h1 className="text-4xl font-black">Mis Tareas</h1>
          <p className="mt-2 text-xl font-light text-gray-500">
            Administra y organiza tu tablero de trabajo
          </p>
        </div>

        <nav className="flex flex-col gap-4 my-8 shrink-0 sm:flex-row">
          <Link
            href={createUrl({ action: "new-task" })}
            className="px-10 py-3 text-xl font-bold text-white transition-colors bg-purple-400 cursor-pointer hover:bg-purple-500 rounded-xl"
          >
            Agregar Tarea
          </Link>
        </nav>
      </div>

      {/* El Tablero Kanban */}
      {isLoading ? (
        <div className="p-10 text-center text-gray-500">
          {/* div spinner */}
          <div className="animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-gray-500 mx-auto mb-4"></div>
          Cargando tareas...
        </div>
      ) : (
        <TaskList tasks={tasks ?? []} canEdit={true} />
      )}

      {/* Modales gestionados por la URL */}
      <Suspense fallback={null}>
        <AddTaskModal />
        <EditTaskModal />
        <TaskDetailsModal />
      </Suspense>
    </>
  );
}
