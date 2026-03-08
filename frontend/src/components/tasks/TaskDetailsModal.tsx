"use client";

import { CalendarDaysIcon, ClockIcon } from "@heroicons/react/20/solid";
import { ArrowPathIcon } from "@heroicons/react/24/outline";
import { useRouter } from "next/navigation";
import type { ChangeEvent } from "react";
import Modal from "@/components/shared/Modal";
import NotesPanel from "../notes/NotesPanel";
import { statusTranslations, taskTranslations } from "@/locales/es";
import { formatDate } from "@/helpers/date";
import { useTask } from "@/hooks/tasks/useQueries";
import { useUpdateTaskStatus } from "@/hooks/tasks/useMutations";
import { useQueryString } from "@/hooks/shared/useQueryString";
import { Task } from "@/schemas/task";
import classNames from "@/helpers/classNames";

export default function TaskDetailsModal() {
  const router = useRouter();
  const { searchParams, createUrl } = useQueryString();

  const action = searchParams.get("action");
  const taskId = searchParams.get("taskId");
  const show = action === "view-task" && !!taskId;

  const { data: task, isError } = useTask(taskId ?? "");
  const { mutate: updateTaskStatus } = useUpdateTaskStatus();

  const closeModal = () => {
    router.push(createUrl({ action: null, taskId: null }), { scroll: false });
  };

  const handleChange = (e: ChangeEvent<HTMLSelectElement>) => {
    if (task) {
      updateTaskStatus({
        id: task.id,
        status: e.target.value as Task["status"],
      });
    }
  };

  if (isError) {
    closeModal();
    return null;
  }

  if (!task) {
    return (
      <Modal title="Cargando detalles..." open={show} close={closeModal}>
        <div className="flex flex-col items-center justify-center p-10 min-h-[30vh]">
          <div className="relative flex items-center justify-center w-16 h-16 mb-4">
            <div className="absolute inset-0 border-t-4 border-b-4 border-purple-500 rounded-full animate-spin"></div>
            <ArrowPathIcon className="w-6 h-6 text-purple-400 animate-pulse" />
          </div>
          <p className="font-medium text-slate-500 animate-pulse">
            Obteniendo información de la tarea...
          </p>
        </div>
      </Modal>
    );
  }

  return (
    <Modal title={task.title} open={show} close={closeModal}>
      <div className="flex flex-col my-4 gap-x-4 sm:flex-row sm:items-center sm:justify-start sm:gap-y-0 gap-y-2">
        {task.dueDate && (
          <p
            className={classNames(
              "flex items-center w-auto px-2 py-1 text-sm font-semibold transition-shadow duration-300 border rounded-full select-none max-w-max hover:shadow-blur",
              new Date(task.dueDate) < new Date() && task.status !== "Completed"
                ? "text-red-700 bg-red-100 border-red-200" // Vencida
                : "text-blue-700 bg-blue-100 border-blue-200", // A tiempo
            )}
          >
            <span className="shrink-0">
              <CalendarDaysIcon className="inline w-5 h-5 mr-2" />
            </span>
            {taskTranslations.dueDate}: {formatDate(task.dueDate)}
          </p>
        )}
        <p className="flex items-center w-auto px-2 py-1 text-sm font-semibold text-purple-700 transition-shadow duration-300 bg-purple-100 border border-purple-200 rounded-full select-none max-w-max hover:shadow-blur">
          <span className="shrink-0">
            <CalendarDaysIcon className="inline w-5 h-5 mr-2" />
          </span>
          {formatDate(task.createdAt)}
        </p>
        <p className="flex items-center px-2 py-1 text-sm font-semibold transition-shadow duration-300 border rounded-full select-none text-fuchsia-700 bg-fuchsia-100 border-fuchsia-200 max-w-max hover:shadow-blur">
          <span className="shrink-0">
            <ClockIcon className="inline w-5 h-5 mr-2" />
          </span>
          {formatDate(task.updatedAt)}
        </p>
      </div>

      <p className="mt-5 mb-1 text-sm font-medium tracking-wide text-gray-500 uppercase">
        Descripción
      </p>
      <p className="leading-relaxed text-gray-700">{task.description}</p>

      {task.changes && task.changes.length > 0 && (
        <div className="py-4 mt-6 border-gray-100 border-y">
          <h3 className="mb-3 text-sm font-medium tracking-wide text-gray-500 uppercase">
            Historial de Cambios ({task.changes.length})
          </h3>
          <div className="pr-2 overflow-y-auto max-h-64">
            <ol className="pl-3 ml-3 border-l border-gray-200">
              {task.changes.map((change, index) => (
                <li
                  key={`${change.status}-${index}`}
                  className="relative mb-4 ml-4"
                >
                  <span className="absolute w-3 h-3 bg-purple-500 border border-white rounded-full -left-8.5 top-2"></span>
                  <div className="flex flex-col gap-1">
                    <time className="text-sm text-gray-500">
                      {formatDate(change.changedAt)}
                    </time>
                    <p className="text-gray-700">
                      <span className="font-semibold text-gray-900">
                        Estado:
                      </span>{" "}
                      {statusTranslations[change.status]}
                      {/* Removí el `por {change.user?.name}` ya que nuestro backend de C# no lo envía, el resto queda intacto */}
                    </p>
                  </div>
                </li>
              ))}
            </ol>
          </div>
        </div>
      )}

      <div className="my-5 space-y-3">
        <label
          htmlFor="status"
          className="block mb-2 text-sm font-medium text-gray-700"
        >
          Estado Actual:
        </label>
        <select
          id="status"
          className="w-full px-3 py-2 text-sm text-gray-900 transition-colors border border-gray-200 rounded-md bg-gray-50 hover:border-purple-400 hover:bg-white focus:border-purple-500 focus:ring-2 focus:ring-purple-200"
          value={task.status}
          onChange={handleChange}
        >
          {Object.entries(statusTranslations).map(([key, value]) => (
            <option key={key} value={key}>
              {value}
            </option>
          ))}
        </select>
      </div>

      <NotesPanel notes={task.notes} taskId={task.id} />
    </Modal>
  );
}
