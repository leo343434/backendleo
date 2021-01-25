using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IMateriaRepository
    {
        Task<object> GetMaterias();

    }
}
