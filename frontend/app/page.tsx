import { getTasks } from "@/api/TasksAPI";
import getQueryClient from "@/lib/getQueryClient";
import { taskKeys } from "@/lib/query-keys";
import DashboardView from "@/views/DashboardView";
import { dehydrate, HydrationBoundary } from "@tanstack/react-query";

export default async function Home() {
  // Obtenemos el QueryClient para hacer prefetch de datos en el servidor
  const queryClient = getQueryClient();

  // Prefetch de la lista de tareas para mejorar la experiencia del usuario al cargar el dashboard
  await queryClient.prefetchQuery({
    queryKey: taskKeys.lists(),
    queryFn: () => getTasks(),
  });

  return (
    <main className="container p-5 mx-auto mt-10">
      <HydrationBoundary state={dehydrate(queryClient)}>
        <DashboardView />
      </HydrationBoundary>
    </main>
  );
}
