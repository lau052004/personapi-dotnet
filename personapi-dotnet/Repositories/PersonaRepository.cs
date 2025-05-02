using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly PersonaDbContext _ctx;

        public PersonaRepository(PersonaDbContext ctx)
            => _ctx = ctx;

        public IEnumerable<Persona> GetAll()
            => _ctx.Personas.AsNoTracking().ToList();

        public Persona? GetById(long cc)
            => _ctx.Personas.Find(cc);

        public void Add(Persona persona)
        {
            _ctx.Personas.Add(persona);
            _ctx.SaveChanges();
        }

        public void Update(Persona persona)
        {
            _ctx.Personas.Update(persona);
            _ctx.SaveChanges();
        }

        public void Delete(long cc)
        {
            var entity = _ctx.Personas.Find(cc);
            if (entity != null)
            {
                _ctx.Personas.Remove(entity);
                _ctx.SaveChanges();
            }
        }
    }
}
