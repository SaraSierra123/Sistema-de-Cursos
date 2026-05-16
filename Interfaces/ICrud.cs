using System.Collections.Generic;

namespace SistemaCursosOnline.Interfaces
{
    // Interfaz genérica de operaciones CRUD — garantiza contrato uniforme para todos los servicios
    public interface ICrud<T>
    {
        void Crear(T nuevoElemento);
        List<T> Listar();
        void Actualizar(T elementoEditado);
        void Eliminar(int id);
        T? BuscarPorId(int id);
    }
}
