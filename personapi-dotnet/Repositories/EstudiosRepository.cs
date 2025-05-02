using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class EstudiosRepository : IEstudiosRepository
    {
        private readonly PersonaDbContext _ctx;
        public EstudiosRepository(PersonaDbContext ctx) => _ctx = ctx;

        public IEnumerable<Estudio> GetAll() =>
            _ctx.Estudios
                .Include(e => e.CcPerNavigation)   // persona
                .Include(e => e.IdProfNavigation)  // profesión
                .AsNoTracking()
                .ToList();

        public Estudio? GetById(int idProf, long ccPer) =>
            _ctx.Estudios
                .Include(e => e.CcPerNavigation)
                .Include(e => e.IdProfNavigation)
                .FirstOrDefault(e => e.IdProf == idProf && e.CcPer == ccPer);

        public void Add(Estudio estudio)
        {
            _ctx.Estudios.Add(estudio);
            _ctx.SaveChanges();
        }

        public void Update(Estudio estudio)
        {
            _ctx.Estudios.Update(estudio);
            _ctx.SaveChanges();
        }

        public void Delete(int idProf, long ccPer)
        {
            var entity = _ctx.Estudios.Find(idProf, ccPer);
            if (entity != null)
            {
                _ctx.Estudios.Remove(entity);
                _ctx.SaveChanges();
            }
        }
    }
}
