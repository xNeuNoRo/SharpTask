"use client";

import { ReactNode, useState } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { ToastContainer } from "react-toastify";

type AppProviderProps = {
  children: ReactNode;
};

export default function AppProvider({ children }: Readonly<AppProviderProps>) {
  const [queryClient] = useState(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            staleTime: 1000 * 60 * 1, // 1 minuto
            refetchOnWindowFocus: false, // No refetch al enfocar la ventana
            retry: 2, // Reintentar 2 veces en caso de error
          },
        },
      }),
  );

  return (
    <QueryClientProvider client={queryClient}>
      {children}
      <ReactQueryDevtools initialIsOpen={false} />
      <ToastContainer pauseOnHover={true} pauseOnFocusLoss={false} stacked />
    </QueryClientProvider>
  );
}
