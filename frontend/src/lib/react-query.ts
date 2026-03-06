import { QueryClient } from "@tanstack/react-query";

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 1, // 1 minuto
      refetchOnWindowFocus: false, // No refetch al enfocar la ventana
      retry: 2, // Reintentar 2 veces en caso de error
    },
  },
});
