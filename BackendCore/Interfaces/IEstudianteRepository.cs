using Backend.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IEstudianteRepository
    {
        Task<bool> AddEstudiante(Estudiante estudiante);
        Task<Object> GetEstudiantes();

        Task<bool> GenerarDatos();

    }
}
