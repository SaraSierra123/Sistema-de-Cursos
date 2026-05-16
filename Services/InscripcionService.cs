using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCursosOnline.Events;
using SistemaCursosOnline.Interfaces;
using SistemaCursosOnline.Models;

namespace SistemaCursosOnline.Services
{
    // Servicio de Inscripciones: valida cupo, persiste y dispara eventos de dominio.
    public class InscripcionService : IInscripcionService
    {
        private readonly List<Inscripcion> _inscripciones = new();
        private readonly ICursoService _cursoService;
        private readonly IEstudianteService _estudianteService;
        private int _nextId = 1;

        // Inyección de dependencias vía constructor — SOLID: DIP aplicado
        public InscripcionService(ICursoService cursoService, IEstudianteService estudianteService)
        {
            _cursoService = cursoService;
            _estudianteService = estudianteService;
        }

        public void Crear(Inscripcion nueva)
        {
            var curso = _cursoService.BuscarPorId(nueva.CursoId)
                ?? throw new Exception($"Curso {nueva.CursoId} no encontrado.");

            var estudiante = _estudianteService.BuscarPorId(nueva.EstudianteId)
                ?? throw new Exception($"Estudiante {nueva.EstudianteId} no encontrado.");

            // Validación de cupo antes de inscribir
            int inscritos = _inscripciones.Count(i => i.CursoId == nueva.CursoId && i.Estado == "Activa");
            if (inscritos >= curso.CupoMaximo)
                throw new Exception($"El curso '{curso.Nombre}' no tiene cupo disponible.");

            nueva.Id = _nextId++;
            nueva.FechaInscripcion = DateTime.Now;
            _inscripciones.Add(nueva);

            // EVENTO: Notifica que el estudiante se inscribió
            EventBus.PublicarEstudianteInscrito(this, new EstudianteInscritoEventArgs(estudiante, curso));

            // EVENTO: Si se alcanzó el cupo máximo, notifica cupo agotado
            int inscritosActualizados = _inscripciones.Count(i => i.CursoId == nueva.CursoId && i.Estado == "Activa");
            if (inscritosActualizados >= curso.CupoMaximo)
                EventBus.PublicarCupoAgotado(this, new CupoAgotadoEventArgs(curso, inscritosActualizados));
        }

        public List<Inscripcion> Listar() => _inscripciones;

        public void Actualizar(Inscripcion editada)
        {
            var existente = BuscarPorId(editada.Id)
                ?? throw new Exception($"Inscripción {editada.Id} no encontrada.");
            existente.Estado = editada.Estado;
        }

        public void Eliminar(int id)
        {
            var inscripcion = BuscarPorId(id)
                ?? throw new Exception($"Inscripción {id} no encontrada.");
            _inscripciones.Remove(inscripcion);
        }

        public Inscripcion? BuscarPorId(int id) =>
            _inscripciones.FirstOrDefault(i => i.Id == id);

        // Filtra las inscripciones activas de un estudiante específico
        public List<Inscripcion> ObtenerPorEstudiante(int estudianteId) =>
            _inscripciones.Where(i => i.EstudianteId == estudianteId).ToList();
    }
}
