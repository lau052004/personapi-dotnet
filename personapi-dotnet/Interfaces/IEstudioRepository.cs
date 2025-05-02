using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Interfaces
{
    public interface IEstudioRepository
    {
        Task<IEnumerable<Estudio>> GetAllAsync();
        Task<Estudio?> GetByIdAsync(int ccPer, int idProf);
        Task<IEnumerable<Estudio>> GetAllByIdProfAsync(int idProf);  // Método asincrónico añadido
        Task<IEnumerable<Estudio>> GetAllByCcPerAsync(int ccPer);    // Método asincrónico añadido
        Task AddAsync(Estudio estudio);
        Task UpdateAsync(Estudio estudio);
        Task DeleteAsync(int ccPer, int idProf);
    }
}