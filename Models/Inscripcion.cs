using System;

namespace SistemaCursosOnline.Models
{
    // ASOCIACIÓN: Conecta un Estudiante con un Curso.
    // Ambos existen independientemente; la Inscripcion solo mantiene la relación.
    public class Inscripcion
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public int CursoId { get; set; }
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Activa"; // Activa | Cancelada

        public override string ToString() =>
            $"Inscripción [{Id}] | Estudiante: {EstudianteId} → Curso: {CursoId} | {FechaInscripcion:dd/MM/yyyy} | {Estado}";
    }
}
