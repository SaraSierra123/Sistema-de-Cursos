namespace SistemaCursosOnline.Models
{
    // Clase base abstracta — todos los actores del sistema son Usuarios.
    // Polimorfismo: MostrarInfo() es abstracto y cada subclase lo implementa diferente.
    public abstract class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Método abstracto que fuerza polimorfismo en tiempo de ejecución
        public abstract string MostrarInfo();

        public override string ToString() => MostrarInfo();
    }
}
