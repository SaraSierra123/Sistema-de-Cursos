namespace SistemaCursosOnline.Models
{
    // HERENCIA: Instructor extiende Usuario con especialidad y comportamiento propio
    public class Instructor : Usuario
    {
        public string Especialidad { get; set; } = string.Empty;

        // Polimorfismo: implementación de MostrarInfo() distinta a la de Estudiante
        public override string MostrarInfo() =>
            $"[Instructor] {Nombre} | Especialidad: {Especialidad} | Email: {Email}";
    }
}
