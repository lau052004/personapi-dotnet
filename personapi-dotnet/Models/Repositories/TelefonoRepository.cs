using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Repositories
{
    public class TelefonoRepository : ITelefonoRepository
    {
        private readonly PersonaDbContext _context;

        public TelefonoRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Telefono>> GetAllAsync()
        {
            return await _context.Telefonos.ToListAsync();
        }

        public async Task<Telefono?> GetByNumberAsync(string numero)
        {
            return await _context.Telefonos.FirstOrDefaultAsync(t => t.Num == numero);
        }

        public async Task<IEnumerable<Telefono>> GetByDuenioAsync(int id)
        {
            return await _context.Telefonos.Where(t => t.Duenio == id).ToListAsync();
        }

        public async Task AddAsync(Telefono telefono)
        {
            await _context.Telefonos.AddAsync(telefono);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Telefono telefono)
        {
            _context.Telefonos.Update(telefono);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string numero)
        {
            var telefono = await _context.Telefonos.FirstOrDefaultAsync(t => t.Num == numero);
            if (telefono != null)
            {
                _context.Telefonos.Remove(telefono);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TelefonoExistsAsync(string numero)
        {
            return await _context.Telefonos.AnyAsync(t => t.Num == numero);
        }
    }
}