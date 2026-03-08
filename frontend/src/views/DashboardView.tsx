"use client";

import TaskList from "@/components/tasks/TaskList";
import AddTaskModal from "@/components/tasks/AddTaskModal";
import EditTaskModal from "@/components/tasks/EditTaskModal";
import TaskDetailsModal from "@/components/tasks/TaskDetailsModal";
import { useSearchTasks, useTasks } from "@/hooks/tasks/useQueries";
import { useQueryString } from "@/hooks/shared/useQueryString";
import { Suspense, useCallback } from "react";
import Link from "next/link";
import { Task } from "@/schemas/task";
import { useRouter } from "next/navigation";
import { statusTranslations } from "@/locales/es";
import { useUrlSearch } from "@/hooks/shared/useUrlSearch";

export default function DashboardView() {
  // Hook para manejar la navegación y manipulación de URLs con query strings
  const router = useRouter();
  const { createUrl, searchParams } = useQueryString();

  // Hook personalizado para manejar un valor de búsqueda sincronizado con la URL
  const { keyword, setKeyword, clear } = useUrlSearch({
    paramName: "search",
    delay: 500,
  });

  // Obtenemos el estado de filtro desde la URL (puede ser "pending", "in-progress", "completed" o undefined)
  const statusFromUrl =
    (searchParams.get("status") as Task["status"]) || undefined;
  const searchFromUrl = searchParams.get("search") || "";

  // Consultas para obtener tareas por estado y por búsqueda,
  // utilizando React Query para manejar el estado de carga y errores
  const {
    data: tasksByStatus,
    isLoading: isLoadingTasks,
    isError: isErrorTasks,
  } = useTasks(statusFromUrl || undefined);
  const {
    data: searchResults,
    isLoading: isLoadingSearch,
    isError: isErrorSearch,
  } = useSearchTasks(searchFromUrl);

  // Determinamos qué tareas mostrar, si estamos buscando o no, y el estado de carga/error correspondiente
  const isSearching = searchFromUrl.trim() !== "";
  const displayedTasks = isSearching
    ? searchResults?.filter(
        (task) => !statusFromUrl || task.status === statusFromUrl,
      )
    : tasksByStatus;
  const isLoading = isSearching ? isLoadingSearch : isLoadingTasks;
  const isError = isSearching ? isErrorSearch : isErrorTasks;

  // Función para manejar el cambio de estado desde el select, actualizando la URL y limpiando la búsqueda
  const handleStatusChange = useCallback(
    (val: string) => {
      const url = createUrl({
        status: val || null,
      });
      router.replace(url, { scroll: false });
    },
    [createUrl, router],
  );

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
            className="px-10 py-3 text-xl font-bold text-white transition-colors bg-purple-400 cursor-pointer hover:bg-purple-500 rounded-xl shadow-sm"
            scroll={false}
          >
            Agregar Tarea
          </Link>
        </nav>
      </div>

      {/* Sección de Filtros y Búsqueda sincronizada con URL */}
      <div className="flex flex-col gap-4 mb-8 sm:flex-row">
        <div className="relative flex-1">
          <input
            type="text"
            placeholder="Buscar tareas..."
            value={keyword}
            onChange={(e) => setKeyword(e.target.value)}
            className="w-full p-3 pr-12 transition-all border border-gray-300 rounded-md focus:border-purple-500 focus:ring-1 focus:ring-purple-500 outline-none"
          />
          {keyword && (
            <button
              onClick={clear}
              className="absolute right-3 top-3 text-gray-400 hover:text-gray-600 hover:cursor-pointer hover:bg-gray-200 rounded-full transition-colors px-2"
            >
              ✕
            </button>
          )}
        </div>

        <select
          value={statusFromUrl || ""}
          onChange={(e) => handleStatusChange(e.target.value)}
          className="p-3 transition-all bg-white border border-gray-300 rounded-md sm:w-64 focus:border-purple-500 focus:ring-1 focus:ring-purple-500 outline-none cursor-pointer"
        >
          <option value="">Todos los estados</option>
          {Object.entries(statusTranslations).map(([key, value]) => (
            <option key={key} value={key}>
              {value}
            </option>
          ))}
        </select>
      </div>

      {/* Tablero Kanban */}
      {isLoading ? (
        <div className="p-20 text-center text-gray-500">
          <div className="w-10 h-10 mx-auto mb-4 border-t-2 border-b-2 border-purple-500 rounded-full animate-spin"></div>
          <p className="animate-pulse">Sincronizando tareas...</p>
        </div>
      ) : (
        <TaskList tasks={displayedTasks ?? []} canEdit={true} />
      )}

      {/* Modales */}
      <Suspense fallback={null}>
        <AddTaskModal />
        <EditTaskModal />
        <TaskDetailsModal />
      </Suspense>
    </>
  );
}
