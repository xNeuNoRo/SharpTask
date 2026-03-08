import React from "react";
import Modal from "../shared/Modal";
import { ArrowPathIcon } from "@heroicons/react/24/outline";

type LoadingTaskModalProps = {
  show: boolean;
  closeModal: () => void;
};

export default function LoadingTaskModal({
  show,
  closeModal,
}: Readonly<LoadingTaskModalProps>) {
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
