using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class ProfesionRepository : IProfesionRepository
    {
        private readonly PersonaDbContext _ctx;
        public ProfesionRepository(PersonaDbContext ctx) => _ctx = ctx;

        public IEnumerable<Profesion> GetAll() =>
            _ctx.Profesions
                .AsNoTracking()
                .ToList();

        public Profesion? GetById(int id) =>
            _ctx.Profesions.Find(id);

        public void Add(Profesion profesion)
        {
            _ctx.Profesions.Add(profesion);
            _ctx.SaveChanges();
        }

        public void Update(Profesion profesion)
        {
            _ctx.Profesions.Update(profesion);
            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _ctx.Profesions.Find(id);
            if (entity != null)
            {
                _ctx.Profesions.Remove(entity);
                _ctx.SaveChanges();
            }
        }
    }
}
