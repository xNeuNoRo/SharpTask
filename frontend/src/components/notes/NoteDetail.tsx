"use client";

import { useEffect, useRef, useState } from "react";
import { useRouter } from "next/navigation";
import { AnimatePresence } from "framer-motion";
import {
  CheckIcon,
  PencilIcon,
  TrashIcon,
  UserCircleIcon,
  XMarkIcon,
} from "@heroicons/react/20/solid";
import MotionElement from "@/components/shared/MotionElement";
import { Note } from "@/schemas/notes";
import { Task } from "@/schemas/task";
import { useQueryString } from "@/hooks/shared/useQueryString";
import { useDeleteNote, useUpdateNote } from "@/hooks/notes/useMutations";
import { formatDate } from "@/helpers/date";

type NoteDetailProps = {
  note: Note;
  taskId: Task["id"];
};

export default function NoteDetail({
  note,
  taskId,
}: Readonly<NoteDetailProps>) {
  // Estado local para manejar el contenido editable de la nota, se inicializa con el contenido actual de la nota
  const [noteContent, setNoteContent] = useState(note.content);

  // Hook para manejar la navegación y manipulación de URLs con query strings
  const router = useRouter();
  const { searchParams, createUrl } = useQueryString();

  // Obtenemos los parámetros de la URL para determinar si estamos en modo edición y cuál es la nota que se está editando
  const noteIdFromUrl = searchParams.get("noteId");

  // Variables booleanas para determinar si estamos en modo edición de esta nota específica,
  // y si hay alguna nota en modo edición (para deshabilitar acciones mientras se edita)
  const isEditing = noteIdFromUrl === note.id;
  const hasEditingNote = Boolean(noteIdFromUrl);

  // Referencia al elemento del DOM del contenido de la nota,
  // para manejar el enfoque y selección del texto al entrar en modo edición
  const contentRef = useRef<HTMLSpanElement>(null);

  // Función para navegar a la URL de edición de esta nota,
  // que incluye los parámetros necesarios para activar el modo edición
  const editNote = () => {
    router.push(createUrl({ noteId: note.id }), {
      scroll: false,
    });
  };

  // Función para cerrar el modo de edición, que navega a la URL sin los parámetros de edición
  const closeEditNote = () => {
    router.push(createUrl({ noteId: null }), { scroll: false });
  };

  // Efecto para manejar el enfoque y selección del texto cuando se entra en modo edición.
  useEffect(() => {
    // Solo cuando entra en modo edición
    if (isEditing && contentRef.current) {
      const el = contentRef.current;
      el.focus();
      const sel = globalThis.getSelection();
      sel?.selectAllChildren(el);
      sel?.collapseToEnd();
    }
  }, [isEditing]);

  // Hooks para manejar las mutaciones de actualización y eliminación de la nota,
  // que se ejecutarán al confirmar la edición o eliminar la nota respectivamente.
  const { mutate: updateNote } = useUpdateNote(taskId);
  const { mutate: deleteNote } = useDeleteNote(taskId);

  // Función para manejar la confirmación de la edición de la nota, que compara el contenido actual con el original,
  // y si hay cambios, llama a la mutación de actualización con el nuevo contenido, luego cierra el modo edición.
  const handleEditNote = () => {
    if (note.content === noteContent) return closeEditNote();
    updateNote({ id: note.id, content: noteContent });
    closeEditNote();
  };
  // Función para manejar la eliminación de la nota, que llama a la mutación de eliminación con el ID de la nota,
  // y luego cierra el modo edición.
  const handleDeleteNote = () => {
    deleteNote(note.id);
    closeEditNote();
  };
  // Función para manejar la cancelación de la edición, que cierra el
  // modo edición y restablece el contenido editable al valor original de la nota.
  const handleCancelEdit = () => {
    closeEditNote();
    if (contentRef.current) contentRef.current.textContent = note.content;
    setNoteContent(note.content);
  };

  return (
    <div className="flex flex-col p-3 sm:items-center gap-y-2 sm:flex-row sm:justify-between sm:gap-x-12">
      <div className="flex items-center flex-1 min-w-0 gap-2">
        <div className="flex items-center flex-1 min-w-0 gap-2">
          <div className="flex flex-col flex-1 min-w-0 leading-tight ">
            <div className="flex items-center gap-2">
              <div className="flex items-center justify-center bg-gray-100 rounded-full w-7 h-7">
                <UserCircleIcon className="w-5 h-5 text-gray-500" />
              </div>

              <span className="text-sm font-medium text-gray-800">
                {/* Reemplazo temporal hasta que el backend devuelva el autor */}
                Usuario
              </span>
              <span className="text-xs text-gray-500">
                {note.createdAt === note.updatedAt
                  ? formatDate(note.createdAt)
                  : `Editado ${formatDate(note.updatedAt)}`}
              </span>
            </div>

            <span
              contentEditable={isEditing}
              suppressContentEditableWarning
              ref={contentRef}
              onBlur={(e) => setNoteContent(e.currentTarget.textContent || "")}
              className="block text-sm border-b-2 border-transparent text-gray-700 mt-0.5 leading-relaxed outline-none focus:border-b-gray-300 transition-colors w-full wrap-break-word whitespace-pre-wrap"
            >
              {note.content}
            </span>
          </div>
        </div>
      </div>

      <AnimatePresence mode="wait">
        {isEditing ? (
          <MotionElement
            as="div"
            key="edit-mode"
            layout
            className="flex justify-end mt-2 sm:mt-0 gap-x-5"
            initial={{ opacity: 0, scale: 0.5, x: 20 }}
            animate={{ opacity: 1, scale: 1, x: 0 }}
            exit={{ opacity: 0, scale: 0.5, x: 20 }}
            transition={{ duration: 0.2, ease: "easeOut" }}
          >
            <button
              className="p-1 transition-colors bg-red-100 rounded-full group hover:bg-red-500 hover:cursor-pointer"
              onClick={handleCancelEdit}
            >
              <XMarkIcon className="w-5 h-5 font-bold text-red-400 transition-colors group-hover:text-white" />
            </button>
            <button
              className="p-1 transition-colors bg-green-100 rounded-full group hover:bg-green-500 hover:cursor-pointer"
              onClick={handleEditNote}
            >
              <CheckIcon className="w-5 h-5 text-green-400 transition-colors group-hover:text-white" />
            </button>
          </MotionElement>
        ) : (
          <MotionElement
            as="div"
            key="view-mode"
            layout
            className="flex justify-end mt-2 sm:mt-0 gap-x-5"
            initial={{ opacity: 0, scale: 0.5, x: -20 }}
            animate={{ opacity: 1, scale: 1, x: 0 }}
            exit={{ opacity: 0, scale: 0.5, x: -20 }}
            transition={{ duration: 0.2, ease: "easeOut" }}
          >
            <button
              disabled={hasEditingNote}
              onClick={editNote}
              className="p-1 transition-colors bg-gray-100 rounded-full group hover:bg-gray-200 hover:cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <PencilIcon className="w-5 h-5 text-gray-500 transition-colors group-hover:text-black" />
            </button>
            <button
              disabled={hasEditingNote}
              onClick={handleDeleteNote}
              className="p-1 transition-colors bg-red-100 rounded-full group hover:bg-red-500 hover:cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <TrashIcon className="w-5 h-5 text-red-400 transition-colors group-hover:text-white" />
            </button>
          </MotionElement>
        )}
      </AnimatePresence>
    </div>
  );
}
