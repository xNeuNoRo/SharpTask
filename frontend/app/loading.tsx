import { ArrowPathIcon } from "@heroicons/react/24/outline";

export default function Loading() {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen p-10 text-gray-500">
      <div className="relative flex items-center justify-center w-16 h-16 mb-6">
        <div className="absolute inset-0 border-t-4 border-b-4 border-purple-500 rounded-full animate-spin"></div>
        <ArrowPathIcon className="w-6 h-6 text-purple-400 animate-pulse" />
      </div>
      
      <h2 className="text-2xl font-bold text-slate-700">Preparando tu espacio...</h2>
      <p className="mt-2 font-medium text-slate-500 animate-pulse">
        Cargando tablero y sincronizando tareas
      </p>
    </div>
  );
}