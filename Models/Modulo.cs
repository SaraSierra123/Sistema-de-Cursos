namespace SistemaCursosOnline.Models
{
    // Módulo pertenece exclusivamente a un Curso.
    // COMPOSICIÓN: no tiene sentido fuera de su Curso padre.
    public class Modulo
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int DuracionHoras { get; set; }
        public int CursoId { get; set; }

        public override string ToString() =>
            $"  Módulo [{Id}]: {Titulo} ({DuracionHoras}h)";
    }
}
