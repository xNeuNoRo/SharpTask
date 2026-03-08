# ✅ SharpTask - Enterprise Task Management & Kanban System

![License](https://img.shields.io/badge/license-MIT-blue.svg) ![Status](https://img.shields.io/badge/status-production--ready-green.svg) ![Frontend](https://img.shields.io/badge/frontend-Next.js%2016-black) ![Backend](https://img.shields.io/badge/backend-.NET%2010-purple)

**SharpTask** es una solución profesional de gestión de proyectos diseñada bajo los principios de **Clean Architecture** y **Domain-Driven Design (DDD)**. El sistema ofrece una experiencia de alta interactividad mediante un tablero Kanban dinámico, permitiendo una gestión fluida de tareas con trazabilidad completa de cambios y un sistema de colaboración mediante notas.

Este proyecto destaca por su enfoque en la **resiliencia de la interfaz**, la **seguridad de frontera** y una arquitectura backend desacoplada que garantiza escalabilidad y facilidad de mantenimiento.

---

## 🚀 Características Principales

### 📊 Gestión de Flujos (Functional)
* **Tablero Kanban Interactivo:** Organización de tareas en 5 estados críticos (*Pending, OnHold, InProgress, UnderReview, Completed*).
* **Drag & Drop de Alto Rendimiento:** Implementación con `@dnd-kit` que permite reordenar y mover tareas con transiciones fluidas.
* **Historial de Auditoría (Audit Log):** Seguimiento automático de cada transición de estado, registrando fecha y hora exacta del cambio.
* **Sistema de Notas Integrado:** Gestión de comentarios y anotaciones por tarea para centralizar la información del equipo.
* **Búsqueda y Deep Linking:** Filtrado avanzado por palabras clave y estados, persistido y sincronizado directamente con la URL.

### 🛠️ Excelencia en Ingeniería (Technical)
* **Clean Architecture Backend:** Separación estricta de responsabilidades en 4 capas (API, Application, Infrastructure, Domain).
* **Actualizaciones Optimistas:** La UI se actualiza instantáneamente tras una acción, con lógica de **rollback automático** gestionada por TanStack Query en caso de fallo del servidor.
* **Seguridad de Frontera (UUID Gatekeeper):** Validación exhaustiva de identificadores mediante esquemas Zod (Frontend) y tipos fuertemente tipados (Backend) para prevenir ataques de *Path Traversal*.
* **Persistencia Atómica en JSON:** Sistema de almacenamiento en archivos con **File Locking** concurrente para evitar la corrupción de datos.
* **Optimización de Renders:** Agrupamiento de tareas O(n) mediante acumuladores mutables controlados en `useMemo` para evitar basura en el Garbage Collector.

---

## 🏗️ Arquitectura del Sistema

### 🖥️ Frontend (`/frontend`)
Arquitectura SPA moderna basada en **Next.js 16** y **React 19**:
* **Framework:** Next.js (App Router).
* **Gestión de Estado:** TanStack Query v5 (Server State) + Zustand (UI State).
* **Validación:** Zod (Validación de esquemas) + React Hook Form.
* **Estilos:** Tailwind CSS v4 + Headless UI.
* **UX:** Loading Skeletons reutilizables y Error Boundaries para una navegación sin interrupciones.

### 🔙 Backend (`/backend`)
Arquitectura desacoplada en **.NET 10 (C#)**:
* **SharpTask.Domain:** Modelos de dominio, entidades base, enums de estado y excepciones.
* **SharpTask.Application:** Casos de uso, servicios de comando/consulta y validadores de FluentValidation.
* **SharpTask.Infrastructure:** Persistencia en archivos JSON, gestión de bloqueos y proveedores de tiempo.
* **SharpTask.API:** Endpoints RESTful, Middlewares de excepción global y documentación con Swagger.

---

## 📸 Capturas de Pantalla
| Dashboard | Tarea |
| :---: | :---: |
| ![Dashboard](/preview/dashboard_preview.png) *Vista principal* | ![Tarea](/preview/tarea_preview.png) *Detalles de una Tarea* | 
![DragAndDrop](/preview/drag_and_drop_preview.png) *Drag And Drop* | ![Nota](/preview/gestion_de_notas_preview.png) *Gestion de Notas* |

---

## 🛠️ Requisitos Previos

* **Runtime JS:** [Bun](https://bun.sh/) (recomendado) o Node.js v22+.
* **SDK .NET:** [.NET SDK 10.0](https://dotnet.microsoft.com/download) o superior.
* **Herramientas:** [mise-en-place](https://mise.jdx.dev/) (opcional para setup automático).

### ⚙️ Variables de Entorno
Configura tu archivo `/frontend/.env.local`:

| Variable | Descripción | Valor sugerido |
| :--- | :--- | :--- |
| `NEXT_PUBLIC_API_URL` | URL de la API Backend | `http://localhost:4000` |

---

## ⚡ Guía de Inicio Rápido

### Opción A: Usando `mise` (Recomendado) ✨
```bash
# 1. Instalar dependencias y preparar el entorno
mise run install

# 2. Ejecutar todo el sistema (Frontend + Backend) en paralelo
mise run dev
```

### Opción B: Setup Manual (Tradicional)

**1. Configurar el Backend:**
```bash
cd backend
dotnet restore
dotnet watch run --project SharpTask.API
```

**2. Configurar el Frontend:**
```bash
cd frontend
bun install
bun run dev
```

---

## 🛡️ Decisiones de Diseño y Seguridad

* **Abstracción del Tiempo:** El sistema utiliza un `IDateTimeProvider` inyectado. Esto permite que las validaciones de fechas (como fechas límite) sean deterministas y fácilmente testeables sin depender del reloj del servidor.
* **Resiliencia de Animación:** Se implementó un control de estado global para desactivar animaciones de DnD durante la sincronización con el servidor (`onSettled`), evitando desincronización visual.
* **Sanitización de URLs:** Todos los parámetros inyectados en las URLs de la API pasan por una función validadora de UUID antes de ser procesados, bloqueando cualquier cadena maliciosa.
* **UX Predictiva:** Los modales de edición e información utilizan el componente `LoadingTaskModal` para evitar parpadeos de datos vacíos mientras se resuelve la consulta de servidor.

---

## 💾 Datos de Prueba (Seed Data)

El repositorio incluye archivos con datos de ejemplo (`tasks.json` y `notes.json`) ubicados en la raíz de la carpeta `backend/SharpTask.API`.

**Para utilizarlos:**
1. Inicia el backend al menos una vez para que se cree la estructura de carpetas automáticamente.
2. Copia los archivos `.json` provistos en `backend/SharpTask.API`.
3. Pégalos en la carpeta de persistencia activa (revisa la pregunta *"¿Dónde se guardan mis datos?"* en la sección siguiente) reemplazando los archivos vacíos.
4. Reinicia el backend para ver las tareas y notas reflejadas en el Dashboard.

---

## ❓ Preguntas Frecuentes

**¿Dónde se guardan físicamente mis datos?**
Los datos persisten en `backend/SharpTask.API/bin/Debug/net10.0/Data/tasks.json`. El sistema crea esta carpeta y archivo automáticamente al iniciar.

**¿El sistema soporta múltiples cambios de estado rápidos?**
Sí. Gracias a TanStack Query y la invalidación de caché inteligente, puedes mover múltiples tareas rápidamente. El backend gestiona las escrituras de forma segura mediante bloqueos de archivo.

**¿Puedo extender el sistema para usar SQL Server?**
Absolutamente. Gracias a la **Inversión de Dependencias** en la capa de Infrastructure, solo necesitas crear una nueva implementación de `ITaskRepository` para Entity Framework Core y registrarla en el contenedor de DI.