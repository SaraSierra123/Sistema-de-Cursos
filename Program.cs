using System;
using System.Collections.Generic;
using Castle.Windsor;
using SistemaCursosOnline.Events;
using SistemaCursosOnline.Functional;
using SistemaCursosOnline.Infrastructure;
using SistemaCursosOnline.Interfaces;
using SistemaCursosOnline.Models;

// ╔══════════════════════════════════════════════════════════════════╗
// ║          SISTEMA DE CURSOS ONLINE — Proyecto Final               ║
// ║  Paradigmas: POO · Aspectos (AOP) · Funcional · Eventos         ║
// ╚══════════════════════════════════════════════════════════════════╝

namespace SistemaCursosOnline
{
    internal class Program
    {
        // Servicios resueltos por interfaz a través de Castle Windsor (AOP)
        static IEstudianteService estudianteService = null!;
        static IInstructorService instructorService = null!;
        static ICursoService cursoService = null!;
        static IInscripcionService inscripcionService = null!;

        static void Main(string[] args)
        {
            // ── ASPECTOS: construcción del contenedor Windsor ──────
            IWindsorContainer container = WindsorContainerConfig.Build();
            estudianteService  = container.Resolve<IEstudianteService>();
            instructorService  = container.Resolve<IInstructorService>();
            cursoService       = container.Resolve<ICursoService>();
            inscripcionService = container.Resolve<IInscripcionService>();

            // ── EVENTOS: suscripción a eventos de dominio ──────────
            // Los manejadores reaccionan automáticamente a cambios de estado
            EventBus.CursoCreado += (sender, e) =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n  [EVENTO] Curso creado: '{e.Curso.Nombre}' el {e.FechaCreacion:dd/MM/yyyy HH:mm}");
                Console.ResetColor();
            };

            EventBus.EstudianteInscrito += (sender, e) =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n  [EVENTO] '{e.Estudiante.Nombre}' se inscribio en '{e.Curso.Nombre}'");
                Console.ResetColor();
            };

            EventBus.CupoAgotado += (sender, e) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n  [EVENTO] Cupo AGOTADO en '{e.Curso.Nombre}' ({e.TotalInscritos}/{e.Curso.CupoMaximo})");
                Console.ResetColor();
            };

            // ── Menú principal ─────────────────────────────────────
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("  ╔══════════════════════════════════════╗");
                Console.WriteLine("  ║     SISTEMA DE CURSOS ONLINE         ║");
                Console.WriteLine("  ╚══════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine("  1. Gestionar Estudiantes");
                Console.WriteLine("  2. Gestionar Instructores");
                Console.WriteLine("  3. Gestionar Cursos");
                Console.WriteLine("  4. Inscripciones");
                Console.WriteLine("  5. Reportes Funcionales");
                Console.WriteLine("  6. Salir");
                Console.Write("\n  Seleccione una opcion: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": MenuEstudiantes(); break;
                    case "2": MenuInstructores(); break;
                    case "3": MenuCursos(); break;
                    case "4": MenuInscripciones(); break;
                    case "5": MenuReportes(); break;
                    case "6": salir = true; break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("  Opcion invalida.");
                        Console.ResetColor();
                        Pausa();
                        break;
                }
            }

            container.Dispose();
            Console.WriteLine("\n  Sistema finalizado. Hasta luego!\n");
        }

        // ══════════════════════════════════════════════════════════
        // MENU ESTUDIANTES — POO: polimorfismo via MostrarInfo()
        // ══════════════════════════════════════════════════════════
        static void MenuEstudiantes()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Titulo("GESTION DE ESTUDIANTES");
                Console.WriteLine("  1. Crear estudiante");
                Console.WriteLine("  2. Listar estudiantes");
                Console.WriteLine("  3. Actualizar estudiante");
                Console.WriteLine("  4. Eliminar estudiante");
                Console.WriteLine("  5. Buscar por ID");
                Console.WriteLine("  0. Volver");
                Console.Write("\n  Opcion: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        Console.Clear();
                        Titulo("CREAR ESTUDIANTE");
                        var est = new Estudiante();
                        Console.Write("  Nombre: ");
                        est.Nombre = Console.ReadLine() ?? "";
                        Console.Write("  Email: ");
                        est.Email = Console.ReadLine() ?? "";
                        Console.Write("  Codigo estudiantil: ");
                        est.CodigoEstudiante = Console.ReadLine() ?? "";
                        try
                        {
                            estudianteService.Crear(est);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n  Estudiante '{est.Nombre}' creado con ID {est.Id}.");
                            Console.ResetColor();
                        }
                        catch { }
                        Pausa();
                        break;

                    case "2":
                        Console.Clear();
                        Titulo("LISTA DE ESTUDIANTES");
                        // Polimorfismo: MostrarInfo() resuelto en tiempo de ejecucion
                        var estudiantes = estudianteService.Listar();
                        if (estudiantes.Count == 0)
                            Console.WriteLine("  No hay estudiantes registrados.");
                        else
                            foreach (var e in estudiantes)
                                Console.WriteLine("  " + e.MostrarInfo());
                        Pausa();
                        break;

                    case "3":
                        Console.Clear();
                        Titulo("ACTUALIZAR ESTUDIANTE");
                        Console.Write("  ID del estudiante a actualizar: ");
                        if (int.TryParse(Console.ReadLine(), out int idActEst))
                        {
                            var editado = new Estudiante { Id = idActEst };
                            Console.Write("  Nuevo nombre: ");
                            editado.Nombre = Console.ReadLine() ?? "";
                            Console.Write("  Nuevo email: ");
                            editado.Email = Console.ReadLine() ?? "";
                            Console.Write("  Nuevo codigo: ");
                            editado.CodigoEstudiante = Console.ReadLine() ?? "";
                            try
                            {
                                estudianteService.Actualizar(editado);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("  Estudiante actualizado.");
                                Console.ResetColor();
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "4":
                        Console.Clear();
                        Titulo("ELIMINAR ESTUDIANTE");
                        Console.Write("  ID del estudiante a eliminar: ");
                        if (int.TryParse(Console.ReadLine(), out int idElimEst))
                        {
                            try
                            {
                                estudianteService.Eliminar(idElimEst);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("  Estudiante eliminado.");
                                Console.ResetColor();
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "5":
                        Console.Clear();
                        Titulo("BUSCAR ESTUDIANTE POR ID");
                        Console.Write("  ID: ");
                        if (int.TryParse(Console.ReadLine(), out int idBusEst))
                        {
                            try
                            {
                                var encontrado = estudianteService.BuscarPorId(idBusEst);
                                if (encontrado != null)
                                    Console.WriteLine("  " + encontrado.MostrarInfo());
                                else
                                    Console.WriteLine("  No encontrado.");
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "0": volver = true; break;
                    default:
                        Console.WriteLine("  Opcion invalida.");
                        Pausa();
                        break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════
        // MENU INSTRUCTORES
        // ══════════════════════════════════════════════════════════
        static void MenuInstructores()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Titulo("GESTION DE INSTRUCTORES");
                Console.WriteLine("  1. Crear instructor");
                Console.WriteLine("  2. Listar instructores");
                Console.WriteLine("  3. Actualizar instructor");
                Console.WriteLine("  4. Eliminar instructor");
                Console.WriteLine("  5. Buscar por ID");
                Console.WriteLine("  0. Volver");
                Console.Write("\n  Opcion: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        Console.Clear();
                        Titulo("CREAR INSTRUCTOR");
                        var inst = new Instructor();
                        Console.Write("  Nombre: ");
                        inst.Nombre = Console.ReadLine() ?? "";
                        Console.Write("  Email: ");
                        inst.Email = Console.ReadLine() ?? "";
                        Console.Write("  Especialidad: ");
                        inst.Especialidad = Console.ReadLine() ?? "";
                        try
                        {
                            instructorService.Crear(inst);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n  Instructor '{inst.Nombre}' creado con ID {inst.Id}.");
                            Console.ResetColor();
                        }
                        catch { }
                        Pausa();
                        break;

                    case "2":
                        Console.Clear();
                        Titulo("LISTA DE INSTRUCTORES");
                        var instructores = instructorService.Listar();
                        if (instructores.Count == 0)
                            Console.WriteLine("  No hay instructores registrados.");
                        else
                            // Polimorfismo: MostrarInfo() de Instructor
                            foreach (var i in instructores)
                                Console.WriteLine("  " + i.MostrarInfo());
                        Pausa();
                        break;

                    case "3":
                        Console.Clear();
                        Titulo("ACTUALIZAR INSTRUCTOR");
                        Console.Write("  ID del instructor a actualizar: ");
                        if (int.TryParse(Console.ReadLine(), out int idActInst))
                        {
                            var editado = new Instructor { Id = idActInst };
                            Console.Write("  Nuevo nombre: ");
                            editado.Nombre = Console.ReadLine() ?? "";
                            Console.Write("  Nuevo email: ");
                            editado.Email = Console.ReadLine() ?? "";
                            Console.Write("  Nueva especialidad: ");
                            editado.Especialidad = Console.ReadLine() ?? "";
                            try
                            {
                                instructorService.Actualizar(editado);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("  Instructor actualizado.");
                                Console.ResetColor();
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "4":
                        Console.Clear();
                        Titulo("ELIMINAR INSTRUCTOR");
                        Console.Write("  ID del instructor a eliminar: ");
                        if (int.TryParse(Console.ReadLine(), out int idElimInst))
                        {
                            try
                            {
                                instructorService.Eliminar(idElimInst);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("  Instructor eliminado.");
                                Console.ResetColor();
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "5":
                        Console.Clear();
                        Titulo("BUSCAR INSTRUCTOR POR ID");
                        Console.Write("  ID: ");
                        if (int.TryParse(Console.ReadLine(), out int idBusInst))
                        {
                            try
                            {
                                var encontrado = instructorService.BuscarPorId(idBusInst);
                                if (encontrado != null)
                                    Console.WriteLine("  " + encontrado.MostrarInfo());
                                else
                                    Console.WriteLine("  No encontrado.");
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "0": volver = true; break;
                    default:
                        Console.WriteLine("  Opcion invalida.");
                        Pausa();
                        break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════
        // MENU CURSOS — Composicion (modulos) y Agregacion (instructor)
        // ══════════════════════════════════════════════════════════
        static void MenuCursos()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Titulo("GESTION DE CURSOS");
                Console.WriteLine("  1. Crear curso");
                Console.WriteLine("  2. Listar cursos");
                Console.WriteLine("  3. Agregar modulo a curso");
                Console.WriteLine("  4. Actualizar curso");
                Console.WriteLine("  5. Eliminar curso");
                Console.WriteLine("  0. Volver");
                Console.Write("\n  Opcion: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        Console.Clear();
                        Titulo("CREAR CURSO");
                        var curso = new Curso();
                        Console.Write("  Nombre: ");
                        curso.Nombre = Console.ReadLine() ?? "";
                        Console.Write("  Descripcion: ");
                        curso.Descripcion = Console.ReadLine() ?? "";
                        Console.Write("  Cupo maximo: ");
                        int.TryParse(Console.ReadLine(), out int cupo);
                        curso.CupoMaximo = cupo;
                        Console.Write("  ID del instructor: ");
                        int.TryParse(Console.ReadLine(), out int instId);
                        curso.InstructorId = instId;
                        try
                        {
                            // Al crear dispara el EVENTO CursoCreado automaticamente
                            cursoService.Crear(curso);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n  Curso '{curso.Nombre}' creado con ID {curso.Id}.");
                            Console.ResetColor();
                        }
                        catch { }
                        Pausa();
                        break;

                    case "2":
                        Console.Clear();
                        Titulo("LISTA DE CURSOS");
                        var cursos = cursoService.Listar();
                        if (cursos.Count == 0)
                        {
                            Console.WriteLine("  No hay cursos registrados.");
                        }
                        else
                        {
                            foreach (var c in cursos)
                            {
                                Console.WriteLine("  " + c);
                                // Composicion: muestra los modulos que pertenecen al curso
                                foreach (var m in c.ListaDeModulos)
                                    Console.WriteLine("    " + m);
                            }
                        }
                        Pausa();
                        break;

                    case "3":
                        Console.Clear();
                        Titulo("AGREGAR MODULO A CURSO");
                        Console.Write("  ID del curso: ");
                        if (int.TryParse(Console.ReadLine(), out int cursoId))
                        {
                            var modulo = new Modulo();
                            Console.Write("  Titulo del modulo: ");
                            modulo.Titulo = Console.ReadLine() ?? "";
                            Console.Write("  Duracion en horas: ");
                            int.TryParse(Console.ReadLine(), out int horas);
                            modulo.DuracionHoras = horas;
                            try
                            {
                                cursoService.AgregarModulo(cursoId, modulo);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("  Modulo agregado.");
                                Console.ResetColor();
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "4":
                        Console.Clear();
                        Titulo("ACTUALIZAR CURSO");
                        Console.Write("  ID del curso a actualizar: ");
                        if (int.TryParse(Console.ReadLine(), out int idActCurso))
                        {
                            var editado = new Curso { Id = idActCurso };
                            Console.Write("  Nuevo nombre: ");
                            editado.Nombre = Console.ReadLine() ?? "";
                            Console.Write("  Nueva descripcion: ");
                            editado.Descripcion = Console.ReadLine() ?? "";
                            Console.Write("  Nuevo cupo maximo: ");
                            int.TryParse(Console.ReadLine(), out int nuevoCupo);
                            editado.CupoMaximo = nuevoCupo;
                            Console.Write("  Nuevo ID instructor: ");
                            int.TryParse(Console.ReadLine(), out int nuevoInst);
                            editado.InstructorId = nuevoInst;
                            try
                            {
                                cursoService.Actualizar(editado);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("  Curso actualizado.");
                                Console.ResetColor();
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "5":
                        Console.Clear();
                        Titulo("ELIMINAR CURSO");
                        Console.Write("  ID del curso a eliminar: ");
                        if (int.TryParse(Console.ReadLine(), out int idElimCurso))
                        {
                            try
                            {
                                cursoService.Eliminar(idElimCurso);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("  Curso eliminado.");
                                Console.ResetColor();
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "0": volver = true; break;
                    default:
                        Console.WriteLine("  Opcion invalida.");
                        Pausa();
                        break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════
        // MENU INSCRIPCIONES — Asociacion + Eventos de dominio
        // ══════════════════════════════════════════════════════════
        static void MenuInscripciones()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Titulo("GESTION DE INSCRIPCIONES");
                Console.WriteLine("  1. Inscribir estudiante en curso");
                Console.WriteLine("  2. Listar todas las inscripciones");
                Console.WriteLine("  3. Ver inscripciones de un estudiante");
                Console.WriteLine("  4. Eliminar inscripcion");
                Console.WriteLine("  0. Volver");
                Console.Write("\n  Opcion: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        Console.Clear();
                        Titulo("INSCRIBIR ESTUDIANTE");
                        Console.Write("  ID del estudiante: ");
                        int.TryParse(Console.ReadLine(), out int estId);
                        Console.Write("  ID del curso: ");
                        int.TryParse(Console.ReadLine(), out int cId);
                        try
                        {
                            // Al inscribir se disparan EVENTOS: EstudianteInscrito y CupoAgotado si aplica
                            inscripcionService.Crear(new Inscripcion
                            {
                                EstudianteId = estId,
                                CursoId = cId
                            });
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n  Inscripcion realizada.");
                            Console.ResetColor();
                        }
                        catch { }
                        Pausa();
                        break;

                    case "2":
                        Console.Clear();
                        Titulo("TODAS LAS INSCRIPCIONES");
                        var todas = inscripcionService.Listar();
                        if (todas.Count == 0)
                            Console.WriteLine("  No hay inscripciones registradas.");
                        else
                            foreach (var i in todas)
                                Console.WriteLine("  " + i);
                        Pausa();
                        break;

                    case "3":
                        Console.Clear();
                        Titulo("INSCRIPCIONES POR ESTUDIANTE");
                        Console.Write("  ID del estudiante: ");
                        if (int.TryParse(Console.ReadLine(), out int idEstInsc))
                        {
                            try
                            {
                                var porEst = inscripcionService.ObtenerPorEstudiante(idEstInsc);
                                if (porEst.Count == 0)
                                    Console.WriteLine("  Este estudiante no tiene inscripciones.");
                                else
                                    foreach (var i in porEst)
                                        Console.WriteLine("  " + i);
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "4":
                        Console.Clear();
                        Titulo("ELIMINAR INSCRIPCION");
                        Console.Write("  ID de la inscripcion: ");
                        if (int.TryParse(Console.ReadLine(), out int idElimInsc))
                        {
                            try
                            {
                                inscripcionService.Eliminar(idElimInsc);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("  Inscripcion eliminada.");
                                Console.ResetColor();
                            }
                            catch { }
                        }
                        Pausa();
                        break;

                    case "0": volver = true; break;
                    default:
                        Console.WriteLine("  Opcion invalida.");
                        Pausa();
                        break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════
        // MENU REPORTES — Paradigma Funcional completo
        // Where, Select, Aggregate, Func<>, Action<>, record inmutable
        // ══════════════════════════════════════════════════════════
        static void MenuReportes()
        {
            Console.Clear();
            Titulo("REPORTES FUNCIONALES");

            var cursos        = cursoService.Listar();
            var inscripciones = inscripcionService.Listar();

            if (cursos.Count == 0)
            {
                Console.WriteLine("  No hay cursos para reportar. Crea cursos primero.");
                Pausa();
                return;
            }

            // LINQ Where: filtra cursos que ya tienen modulos cargados
            var cursosConHoras = CursoQueryFunctions.FiltrarCursos(
                cursos, c => c.DuracionTotalHoras > 0);
            Console.WriteLine($"\n  [Where] Cursos con modulos cargados: {cursosConHoras.Count}");
            cursosConHoras.ForEach(c =>
                Console.WriteLine($"    - {c.Nombre} ({c.DuracionTotalHoras}h)"));

            // LINQ Select proyecta a record inmutable ResumenCurso
            var resumenes = CursoQueryFunctions.ProyectarResumenes(cursos, inscripciones);
            Console.WriteLine("\n  [Select] Resumen por curso (record inmutable):");
            // Action<> como parametro de alto orden
            CursoQueryFunctions.AplicarSobreResumenes(resumenes, r =>
                Console.WriteLine($"    - {r.NombreCurso} | Modulos: {r.TotalModulos} | Horas: {r.TotalHoras}h | Inscritos: {r.TotalInscritos}")
            );

            // LINQ Aggregate: reduce la lista entera a un solo valor
            int totalHoras = CursoQueryFunctions.TotalHorasSistema(cursos);
            Console.WriteLine($"\n  [Aggregate] Total horas en el sistema: {totalHoras}h");

            // Composicion de Func<>: dos predicados combinados en uno (AND logico)
            Func<Curso, bool> tieneMasDeUnModulo = c => c.ListaDeModulos.Count > 1;
            Func<Curso, bool> tieneInstructor    = c => c.InstructorId > 0;
            var filtroCompuesto = CursoQueryFunctions.CombinarFiltros(tieneMasDeUnModulo, tieneInstructor);
            var filtrados = CursoQueryFunctions.FiltrarCursos(cursos, filtroCompuesto);
            Console.WriteLine($"\n  [Func<>] Cursos con +1 modulo y con instructor: {filtrados.Count}");
            filtrados.ForEach(c => Console.WriteLine($"    - {c.Nombre}"));

            // Ordenamiento funcional sin mutar la lista original
            var ordenados = CursoQueryFunctions.OrdenarPorHorasDesc(resumenes);
            Console.WriteLine("\n  [OrderBy] Cursos por horas descendente:");
            ordenados.ForEach(r => Console.WriteLine($"    - {r.NombreCurso}: {r.TotalHoras}h"));

            Pausa();
        }

        // ── Utilidades de consola ──────────────────────────────────
        static void Titulo(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"  == {texto} ==\n");
            Console.ResetColor();
        }

        static void Pausa()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\n  Presione Enter para continuar...");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
