"use client"; // Error boundaries siempre deben ser de cliente

import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";
import { useEffect } from "react";

export default function ErrorPage({
  error,
  reset,
}: Readonly<{
  error: Error & { digest?: string };
  reset: () => void;
}>) {
  useEffect(() => {
    // Aquí podrías enviar el error a Sentry, Datadog, etc.
    console.error("Error fatal interceptado por Next.js:", error);
  }, [error]);

  return (
    <div className="flex flex-col items-center justify-center min-h-screen p-8 text-center animate-in fade-in zoom-in duration-300">
      <div className="p-4 mb-6 bg-red-100 rounded-full ring-8 ring-red-50">
        <ExclamationTriangleIcon className="w-12 h-12 text-red-600" />
      </div>

      <h2 className="mb-2 text-3xl font-black text-slate-800">
        ¡Ups! Algo salió mal
      </h2>
      <p className="max-w-md mb-8 text-lg text-slate-500">
        Tuvimos un problema inesperado al procesar tu solicitud. La conexión con
        el servidor podría estar fallando.
      </p>

      <button
        onClick={() => reset()} // Intenta re-renderizar la ruta actual
        className="px-8 py-3 text-lg font-bold text-white transition-all duration-200 bg-purple-600 rounded-xl hover:bg-purple-700 shadow-sm hover:shadow active:scale-95"
      >
        Intentar de nuevo
      </button>
    </div>
  );
}
