import type { Metadata } from "next";
import { Inter } from "next/font/google";
import AppProvider from "@/components/providers/AppProvider";
import "./globals.css";

const inter = Inter({
  subsets: ["latin"],
  variable: "--font-inter",
});

export const metadata: Metadata = {
  title: "SharpTask - Gestión de Tareas",
  description:
    "SharpTask es una aplicación de gestión de tareas que te ayuda a organizar y priorizar tus actividades diarias de manera eficiente.",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="es">
      <body className={`${inter.variable}`}>
        <AppProvider>{children}</AppProvider>
      </body>
    </html>
  );
}
