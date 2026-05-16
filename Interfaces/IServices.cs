using System.Collections.Generic;
using SistemaCursosOnline.Models;

namespace SistemaCursosOnline.Interfaces
{
    // Castle DynamicProxy intercepta llamadas a través de estas interfaces.
    // Los servicios se registran y resuelven SIEMPRE por su interfaz, nunca por clase concreta.

    public interface IEstudianteService : ICrud<Estudiante> { }

    public interface IInstructorService : ICrud<Instructor> { }

    public interface ICursoService : ICrud<Curso>
    {
        // Operación de dominio adicional — agrega un módulo a un curso existente
        void AgregarModulo(int cursoId, Modulo modulo);
    }

    public interface IInscripcionService : ICrud<Inscripcion>
    {
        // Devuelve todos los cursos en los que está inscrito un estudiante
        List<Inscripcion> ObtenerPorEstudiante(int estudianteId);
    }
}
