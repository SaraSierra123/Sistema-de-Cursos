using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCursosOnline.Interfaces;
using SistemaCursosOnline.Models;

namespace SistemaCursosOnline.Services
{
    // Implementación concreta de IInstructorService.
    public class InstructorService : IInstructorService
    {
        private readonly List<Instructor> _instructores = new();
        private int _nextId = 1;

        public void Crear(Instructor nuevo)
        {
            nuevo.Id = _nextId++;
            _instructores.Add(nuevo);
        }

        public List<Instructor> Listar() => _instructores;

        public void Actualizar(Instructor editado)
        {
            var existente = BuscarPorId(editado.Id)
                ?? throw new Exception($"Instructor {editado.Id} no encontrado.");
            existente.Nombre = editado.Nombre;
            existente.Email = editado.Email;
            existente.Especialidad = editado.Especialidad;
        }

        public void Eliminar(int id)
        {
            var instructor = BuscarPorId(id)
                ?? throw new Exception($"Instructor {id} no encontrado.");
            _instructores.Remove(instructor);
        }

        public Instructor? BuscarPorId(int id) =>
            _instructores.FirstOrDefault(i => i.Id == id);
    }
}
