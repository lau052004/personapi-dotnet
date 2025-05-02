using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Interfaces
{
    public interface ITelefonoRepository
    {
        Task<IEnumerable<Telefono>> GetAllAsync();
        Task<Telefono?> GetByNumberAsync(string numero);
        Task<IEnumerable<Telefono>> GetByDuenioAsync(int id);
        Task AddAsync(Telefono telefono);
        Task UpdateAsync(Telefono telefono);
        Task DeleteAsync(string numero);
        Task<bool> TelefonoExistsAsync(string numero);
    }
}