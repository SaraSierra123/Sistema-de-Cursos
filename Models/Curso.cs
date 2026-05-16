using System.Collections.Generic;
using System.Linq;

namespace SistemaCursosOnline.Models
{
    // COMPOSICIÓN: Curso posee su lista de Módulos — si el Curso muere, los módulos también.
    // AGREGACIÓN: Guarda InstructorId; el Instructor existe independientemente del Curso.
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CupoMaximo { get; set; }
        public int InstructorId { get; set; }

        // Composición: los módulos son parte intrínseca del curso
        public List<Modulo> ListaDeModulos { get; set; } = new();

        // Calcula duración total con LINQ — se usa también en la capa funcional
        public int DuracionTotalHoras => ListaDeModulos.Sum(m => m.DuracionHoras);

        public void AgregarModulo(Modulo modulo) => ListaDeModulos.Add(modulo);

        public override string ToString() =>
            $"Curso [{Id}]: {Nombre} | Cupo: {CupoMaximo} | Instructor ID: {InstructorId} | Total horas: {DuracionTotalHoras}h";
    }
}
