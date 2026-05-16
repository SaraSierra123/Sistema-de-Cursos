using System;
using SistemaCursosOnline.Models;

namespace SistemaCursosOnline.Events
{
    // ─────────────────────────────────────────────────────────────
    // EVENTO 1: EstudianteInscrito — se dispara cuando un estudiante
    //           se inscribe exitosamente en un curso.
    // ─────────────────────────────────────────────────────────────
    public class EstudianteInscritoEventArgs : EventArgs
    {
        public Estudiante Estudiante { get; }
        public Curso Curso { get; }
        public DateTime FechaInscripcion { get; }

        public EstudianteInscritoEventArgs(Estudiante estudiante, Curso curso)
        {
            Estudiante = estudiante;
            Curso = curso;
            FechaInscripcion = DateTime.Now;
        }
    }

    // ─────────────────────────────────────────────────────────────
    // EVENTO 2: CursoCreado — se dispara al crear un nuevo curso.
    // ─────────────────────────────────────────────────────────────
    public class CursoCreadoEventArgs : EventArgs
    {
        public Curso Curso { get; }
        public DateTime FechaCreacion { get; }

        public CursoCreadoEventArgs(Curso curso)
        {
            Curso = curso;
            FechaCreacion = DateTime.Now;
        }
    }

    // ─────────────────────────────────────────────────────────────
    // EVENTO 3: CupoAgotado — se dispara cuando un curso llega a su
    //           capacidad máxima de inscritos.
    // ─────────────────────────────────────────────────────────────
    public class CupoAgotadoEventArgs : EventArgs
    {
        public Curso Curso { get; }
        public int TotalInscritos { get; }

        public CupoAgotadoEventArgs(Curso curso, int totalInscritos)
        {
            Curso = curso;
            TotalInscritos = totalInscritos;
        }
    }

    // ─────────────────────────────────────────────────────────────
    // Bus de eventos estático — punto central donde se publican y
    // suscriben todos los eventos de dominio del sistema.
    // ─────────────────────────────────────────────────────────────
    public static class EventBus
    {
        // Los suscriptores se adjuntan con +=  y se desuscriben con -=
        public static event EventHandler<EstudianteInscritoEventArgs>? EstudianteInscrito;
        public static event EventHandler<CursoCreadoEventArgs>? CursoCreado;
        public static event EventHandler<CupoAgotadoEventArgs>? CupoAgotado;

        public static void PublicarEstudianteInscrito(object sender, EstudianteInscritoEventArgs args)
            => EstudianteInscrito?.Invoke(sender, args);

        public static void PublicarCursoCreado(object sender, CursoCreadoEventArgs args)
            => CursoCreado?.Invoke(sender, args);

        public static void PublicarCupoAgotado(object sender, CupoAgotadoEventArgs args)
            => CupoAgotado?.Invoke(sender, args);
    }
}
