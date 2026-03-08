import { QueryClient } from "@tanstack/react-query";
import { cache } from "react";

// 'cache' asegura que el QueryClient sea único por cada request del servidor
const getQueryClient = cache(
  () =>
    new QueryClient({
      defaultOptions: {
        queries: {
          // En el servidor, normalmente queremos un staleTime mayor a 0
          // para evitar que el cliente haga refetch inmediatamente después de hidratar
          staleTime: 60 * 1000,
        },
      },
    }),
);

export default getQueryClient;
