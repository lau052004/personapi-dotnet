using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Interfaces
{
    public interface IPersonaRepository
    {
        IEnumerable<Persona> GetAll();
        Persona? GetById(long cc);
        void Add(Persona persona);
        void Update(Persona persona);
        void Delete(long cc);
    }
}
