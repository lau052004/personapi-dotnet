using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Interfaces
{
    public interface IEstudiosRepository
    {
        IEnumerable<Estudio> GetAll();
        Estudio? GetById(int idProf, long ccPer);
        void Add(Estudio estudio);
        void Update(Estudio estudio);
        void Delete(int idProf, long ccPer);
    }
}
