# Sistema de Cursos Online 

Aplicación de consola en **.NET 8 / C#** que gestiona un sistema de cursos, instructores, estudiantes e inscripciones, integrando cuatro paradigmas de programación.

---

## Descripción del sistema

El sistema permite:
- Crear y administrar **instructores** y **estudiantes**.
- Crear **cursos** con **módulos** propios.
- **Inscribir** estudiantes en cursos respetando cupo máximo.
- Consultar estadísticas del sistema con estilo funcional puro.

---

## Estructura del proyecto

```
SistemaCursosOnline/
├── Models/             # Clases de dominio (POO)
│   ├── Usuario.cs
│   ├── Estudiante.cs
│   ├── Instructor.cs
│   ├── Curso.cs
│   ├── Modulo.cs
│   └── Inscripcion.cs
├── Interfaces/         # Contratos de servicio (POO + AOP)
│   ├── ICrud.cs
│   └── IServices.cs
├── Services/           # Implementaciones concretas (POO + Eventos)
│   ├── EstudianteService.cs
│   ├── InstructorService.cs
│   ├── CursoService.cs
│   └── InscripcionService.cs
├── Aspects/            # Interceptores Castle DynamicProxy (AOP)
│   ├── LoggingInterceptor.cs
│   └── ErrorHandlingInterceptor.cs
├── Events/             # Eventos de dominio (Eventos)
│   └── DominioEventos.cs
├── Functional/         # Funciones puras, record y LINQ (Funcional)
│   └── CursoQueryFunctions.cs
├── Infrastructure/     # Contenedor Castle Windsor (AOP)
│   └── WindsorContainerConfig.cs
└── Program.cs          # Punto de entrada — demuestra los 4 paradigmas
```

---

## Decisiones de diseño por paradigma

### 1. Orientado a Objetos (POO)

| Relación | Clases involucradas | Justificación |
|---|---|---|
| **Herencia** | `Estudiante : Usuario`, `Instructor : Usuario` | Ambos comparten Id, Nombre y Email pero tienen comportamiento distinto |
| **Composición** | `Curso` tiene `List<Modulo>` | Los módulos no tienen sentido sin el curso; si el curso desaparece, sus módulos también |
| **Agregación** | `Curso` guarda `InstructorId` | El instructor existe independientemente del curso |
| **Asociación** | `Inscripcion` conecta `EstudianteId` con `CursoId` | Ambos existen por sí solos; la inscripción solo materializa su relación |

**Polimorfismo:** `MostrarInfo()` es abstracto en `Usuario`. Una `List<Usuario>` que contiene `Estudiante` e `Instructor` llama la versión correcta en tiempo de ejecución sin condiciones.

**Interfaz:** `ICrud<T>` define el contrato CRUD genérico que todos los servicios implementan. `IEstudianteService`, `ICursoService`, etc. extienden ese contrato con operaciones propias.

### 2. Paradigma de Aspectos (AOP)

- **Castle Windsor** actúa como contenedor de Inyección de Dependencias.
- Los servicios se registran y resuelven **únicamente por su interfaz**; el contenedor inyecta un proxy transparente.
- **`LoggingInterceptor`**: registra automáticamente el inicio, los argumentos, el fin y el tiempo de cada método de servicio sin que los servicios escriban una sola línea de log.
- **`ErrorHandlingInterceptor`**: captura cualquier excepción, la muestra con contexto (clase + método) y la re-lanza — manejo centralizado sin `try/catch` en cada servicio.

> Los interceptores se aplican en orden: primero `LoggingInterceptor`, luego `ErrorHandlingInterceptor`.

### 3. Programación Funcional

- **`record ResumenCurso`**: tipo inmutable — una vez creado no puede mutarse.
- **LINQ `Where`**: filtra cursos usando un `Func<Curso, bool>` recibido como parámetro (función de alto orden).
- **LINQ `Select`**: proyecta `Curso` → `ResumenCurso` sin mutar los originales.
- **LINQ `Aggregate`**: reduce la lista completa a un único entero (total de horas).
- **`Func<>` y `Action<>`**: `AplicarSobreResumenes` recibe un `Action<ResumenCurso>` y decide cómo imprimir externamente. `CombinarFiltros` compone dos predicados en uno nuevo.
- Todas las funciones en `CursoQueryFunctions` son **puras**: mismo input → mismo output, sin efectos secundarios.

### 4. Programación Orientada a Eventos

| Evento | `EventArgs` | Se dispara cuando… |
|---|---|---|
| `CursoCreado` | `CursoCreadoEventArgs` | Se crea un curso exitosamente |
| `EstudianteInscrito` | `EstudianteInscritoEventArgs` | Un estudiante se inscribe en un curso |
| `CupoAgotado` | `CupoAgotadoEventArgs` | El número de inscritos alcanza el cupo máximo |

Los eventos son **semánticamente significativos**: reflejan cambios de estado reales del dominio, no eventos técnicos. El `EventBus` estático actúa como broker central de publicación/suscripción.

---

## Principios SOLID aplicados

- **S** — Cada clase tiene una única responsabilidad (los servicios no hacen logging; los interceptores no conocen el dominio).
- **O** — `ICrud<T>` es extensible sin modificar la interfaz base.
- **L** — `Estudiante` e `Instructor` son sustituibles por `Usuario` sin romper el sistema.
- **I** — Interfaces pequeñas y específicas (`IEstudianteService`, `ICursoService`, etc.).
- **D** — Los servicios dependen de abstracciones (`ICursoService`), no de implementaciones concretas.