using Backend.Core.Entities;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IInscripcionRepository
    {
        Task<object> Getinscripciones();
        Task<object> GetEstudiantesPorMateria(int idMateria);
        Task<object> GetMateriasPorEstudiante(int idEstudiante);

        Task<bool> AddInscripcion(Inscripcion inscripcion);
        Task<bool> DeleteInscripcion(int id);
        Task<bool> UpdateInscripcion(Inscripcion inscripcion);


    }
}
