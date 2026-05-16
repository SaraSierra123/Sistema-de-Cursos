using System;
using System.Collections.Generic;
using System.Linq;
using SistemaCursosOnline.Interfaces;
using SistemaCursosOnline.Models;

namespace SistemaCursosOnline.Services
{
    // Implementación concreta de IEstudianteService.
    // Castle Windsor registra esta clase bajo la interfaz IEstudianteService.
    public class EstudianteService : IEstudianteService
    {
        // Almacenamiento en memoria — simula la capa de persistencia
        private readonly List<Estudiante> _estudiantes = new();
        private int _nextId = 1;

        public void Crear(Estudiante nuevo)
        {
            nuevo.Id = _nextId++;
            _estudiantes.Add(nuevo);
        }

        public List<Estudiante> Listar() => _estudiantes;

        public void Actualizar(Estudiante editado)
        {
            var existente = BuscarPorId(editado.Id)
                ?? throw new Exception($"Estudiante {editado.Id} no encontrado.");
            existente.Nombre = editado.Nombre;
            existente.Email = editado.Email;
            existente.CodigoEstudiante = editado.CodigoEstudiante;
        }

        public void Eliminar(int id)
        {
            var estudiante = BuscarPorId(id)
                ?? throw new Exception($"Estudiante {id} no encontrado.");
            _estudiantes.Remove(estudiante);
        }

        public Estudiante? BuscarPorId(int id) =>
            _estudiantes.FirstOrDefault(e => e.Id == id);
    }
}
