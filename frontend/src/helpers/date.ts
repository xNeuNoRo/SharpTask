export function formatDate(isoString: string): string {
  const date = new Date(isoString);
  const formatter = new Intl.DateTimeFormat("es-ES", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });
  return formatter.format(date);
}

export function formatDateShort(isoString: string): string {
  const date = new Date(isoString);
  const formatter = new Intl.DateTimeFormat("es-ES", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
  return formatter.format(date);
}
