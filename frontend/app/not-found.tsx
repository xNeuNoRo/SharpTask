import Link from "next/link";
import { DocumentMagnifyingGlassIcon } from "@heroicons/react/24/outline";

export default function NotFound() {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen p-8 text-center animate-in fade-in zoom-in duration-300">
      <div className="p-4 mb-6 bg-slate-100 rounded-full ring-8 ring-slate-50">
        <DocumentMagnifyingGlassIcon className="w-12 h-12 text-slate-400" />
      </div>
      
      <h2 className="mb-2 text-3xl font-black text-slate-800">Página no encontrada</h2>
      <p className="max-w-md mb-8 text-lg text-slate-500">
        Parece que te has perdido. La ruta que estás buscando no existe o fue movida a otra ubicación.
      </p>
      
      <Link
        href="/"
        className="px-8 py-3 text-lg font-bold text-white transition-all duration-200 bg-purple-600 rounded-xl hover:bg-purple-700 shadow-sm hover:shadow active:scale-95"
      >
        Volver al Tablero
      </Link>
    </div>
  );
}