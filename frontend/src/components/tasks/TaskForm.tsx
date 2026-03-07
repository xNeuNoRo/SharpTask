"use client";

import type {
  FieldErrors,
  FieldValues,
  Path,
  UseFormRegister,
} from "react-hook-form";
import {
  ClipboardDocumentListIcon,
  Bars3BottomLeftIcon,
  GlobeAltIcon,
} from "@heroicons/react/20/solid";
import ErrorMessage from "@/components/shared/ErrorMessage";
import { statusTranslations } from "@/locales/es";

// Tipo de props para el formulario de tarea, 
// con genéricos para permitir flexibilidad en los tipos de formulario
type TaskFormProps<T extends FieldValues> = {
  register: UseFormRegister<T>;
  errors: FieldErrors<T>;
};

// Componente de formulario para crear o editar una tarea, con campos para título, descripción y estado
export default function TaskForm<T extends FieldValues>({
  register,
  errors,
}: Readonly<TaskFormProps<T>>) {
  return (
    <>
      <div className="flex flex-col gap-2">
        <label
          htmlFor="title"
          className="flex items-center text-lg font-normal leading-2"
        >
          <ClipboardDocumentListIcon className="inline w-6 h-6 mr-2 text-gray-400" />
          Nombre de la tarea
        </label>

        <input
          id="title"
          type="text"
          placeholder="Nombre de la tarea"
          className="w-full p-3 border border-gray-300 rounded-md"
          {...register("title" as Path<T>)}
        />

        {errors.title && (
          <ErrorMessage>{errors.title?.message as string}</ErrorMessage>
        )}
      </div>
      <div className="flex flex-col gap-2">
        <label
          htmlFor="description"
          className="flex items-center text-lg font-normal leading-2"
        >
          <Bars3BottomLeftIcon className="inline w-6 h-6 mr-2 text-gray-400" />
          Descripción de la tarea
        </label>

        <textarea
          id="description"
          placeholder="Descripción de la tarea"
          className="w-full p-3 border border-gray-300 rounded-md"
          {...register("description" as Path<T>)}
        />

        {errors.description && (
          <ErrorMessage>{errors.description?.message as string}</ErrorMessage>
        )}
      </div>

      <div className="flex flex-col gap-2">
        <label
          htmlFor="status"
          className="flex items-center text-lg font-normal leading-2"
        >
          <GlobeAltIcon className="inline w-6 h-6 mr-2 text-gray-400" />
          Estado de la tarea
        </label>
        <select
          id="status"
          className="w-full p-3 transition-colors border border-gray-300 rounded-md bg-gray-50 hover:bg-white focus:border-fuchsia-500 focus:ring-2 focus:ring-fuchsia-200"
          {...register("status" as Path<T>)}
        >
          {Object.entries(statusTranslations).map(([key, value]) => (
            <option key={key} value={key}>
              {value}
            </option>
          ))}
        </select>
        {errors.status && (
          <ErrorMessage>{errors.status?.message as string}</ErrorMessage>
        )}
      </div>
    </>
  );
}
