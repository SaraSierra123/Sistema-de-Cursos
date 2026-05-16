using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCursosOnline.Events;
using SistemaCursosOnline.Interfaces;
using SistemaCursosOnline.Models;

namespace SistemaCursosOnline.Services
{
    // Servicio de Cursos: gestiona el ciclo de vida de los cursos y publica eventos.
    public class CursoService : ICursoService
    {
        private readonly List<Curso> _cursos = new();
        private int _nextId = 1;
        private int _nextModuloId = 1;

        public void Crear(Curso nuevo)
        {
            nuevo.Id = _nextId++;
            _cursos.Add(nuevo);

            // EVENTO: Notifica a todos los suscriptores que se creó un curso
            EventBus.PublicarCursoCreado(this, new CursoCreadoEventArgs(nuevo));
        }

        public List<Curso> Listar() => _cursos;

        public void Actualizar(Curso editado)
        {
            var existente = BuscarPorId(editado.Id)
                ?? throw new Exception($"Curso {editado.Id} no encontrado.");
            existente.Nombre = editado.Nombre;
            existente.Descripcion = editado.Descripcion;
            existente.CupoMaximo = editado.CupoMaximo;
            existente.InstructorId = editado.InstructorId;
        }

        public void Eliminar(int id)
        {
            var curso = BuscarPorId(id)
                ?? throw new Exception($"Curso {id} no encontrado.");
            _cursos.Remove(curso);
        }

        public Curso? BuscarPorId(int id) =>
            _cursos.FirstOrDefault(c => c.Id == id);

        // Agrega un módulo al curso especificado (operación de dominio rica)
        public void AgregarModulo(int cursoId, Modulo modulo)
        {
            var curso = BuscarPorId(cursoId)
                ?? throw new Exception($"Curso {cursoId} no encontrado.");
            modulo.Id = _nextModuloId++;
            modulo.CursoId = cursoId;
            curso.AgregarModulo(modulo);
        }
    }
}
