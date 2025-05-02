using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelefonoesController : ControllerBase
    {
        private readonly PersonaDbContext _context;

        public TelefonoesController(PersonaDbContext context)
        {
            _context = context;
        }

        // GET: api/Telefonoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Telefono>>> GetTelefonos()
        {
            return await _context.Telefonos.ToListAsync();
        }

        // GET: api/Telefonoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Telefono>> GetTelefono(string id)
        {
            var telefono = await _context.Telefonos.FindAsync(id);

            if (telefono == null)
            {
                return NotFound();
            }

            return telefono;
        }

        // PUT: api/Telefonoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTelefono(string id, Telefono telefono)
        {
            if (id != telefono.Num)
            {
                return BadRequest();
            }

            _context.Entry(telefono).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelefonoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Telefonoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Telefono>> PostTelefono(Telefono telefono)
        {
            // Verifica si ya existe el número
            if (TelefonoExists(telefono.Num))
                return Conflict(new { message = "El número ya existe." });

            // Por esta línea para manejar la posible referencia nula:
            telefono.DuenioNavigation = await _context.Personas.FindAsync(telefono.Duenio)
                                       ?? throw new InvalidOperationException("No se encontró la persona con el documento especificado.");
            if (telefono.DuenioNavigation == null)
                return BadRequest(new { message = "No existe la persona con ese documento." });

            // Agrega y guarda
            _context.Telefonos.Add(telefono);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTelefono", new { id = telefono.Num }, telefono);
        }

        // DELETE: api/Telefonoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTelefono(string id)
        {
            var telefono = await _context.Telefonos.FindAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }

            _context.Telefonos.Remove(telefono);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TelefonoExists(string id)
        {
            return _context.Telefonos.Any(e => e.Num == id);
        }
    }
}
