namespace SistemaCursosOnline.Models
{
    // HERENCIA: Estudiante extiende Usuario con su propio código y comportamiento
    public class Estudiante : Usuario
    {
        public string CodigoEstudiante { get; set; } = string.Empty;

        // Polimorfismo en tiempo de ejecución: implementación específica de Estudiante
        public override string MostrarInfo() =>
            $"[Estudiante] {Nombre} | Código: {CodigoEstudiante} | Email: {Email}";
    }
}
