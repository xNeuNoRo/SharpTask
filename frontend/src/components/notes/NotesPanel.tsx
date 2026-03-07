"use client";

import { Note } from "@/schemas/notes";
import { Task } from "@/schemas/task";
import AddNoteForm from "./AddNoteForm";
import NoteDetail from "./NoteDetail";

type NotesPanelProps = {
  notes: Note[];
  taskId: Task["id"];
};

export default function NotesPanel({
  notes,
  taskId,
}: Readonly<NotesPanelProps>) {
  return (
    <>
      <AddNoteForm taskId={taskId} />

      <div className="mt-10 space-y-3">
        <p className="my-5 text-sm font-semibold tracking-wide text-gray-500 uppercase">
          Notas
        </p>
        <div className="p-3 rounded-md shadow-sm bg-gray-50">
          {notes.length > 0 ? (
            <div className="overflow-y-auto divide-y divide-gray-200 max-h-64">
              {notes.map((note) => (
                <NoteDetail key={note.id} note={note} taskId={taskId} />
              ))}
            </div>
          ) : (
            <p className="text-sm text-gray-500">
              No hay notas para esta tarea.
            </p>
          )}
        </div>
      </div>
    </>
  );
}
