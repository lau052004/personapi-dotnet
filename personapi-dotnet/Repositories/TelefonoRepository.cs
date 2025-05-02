using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class TelefonoRepository : ITelefonoRepository
    {
        private readonly PersonaDbContext _ctx;
        public TelefonoRepository(PersonaDbContext ctx) => _ctx = ctx;

        public IEnumerable<Telefono> GetAll() =>
            _ctx.Telefonos
                .AsNoTracking()
                .ToList();

        public Telefono? GetById(string num) =>
            _ctx.Telefonos.Find(num);

        public void Add(Telefono telefono)
        {
            _ctx.Telefonos.Add(telefono);
            _ctx.SaveChanges();
        }

        public void Update(Telefono telefono)
        {
            _ctx.Telefonos.Update(telefono);
            _ctx.SaveChanges();
        }

        public void Delete(string num)
        {
            var entity = _ctx.Telefonos.Find(num);
            if (entity != null)
            {
                _ctx.Telefonos.Remove(entity);
                _ctx.SaveChanges();
            }
        }
    }
}
