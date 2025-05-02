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
    public class EstudiosController : ControllerBase
    {
        private readonly PersonaDbContext _context;

        public EstudiosController(PersonaDbContext context)
        {
            _context = context;
        }

        // GET: api/Estudios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudio>>> GetEstudios()
        {
            return await _context.Estudios.ToListAsync();
        }

        // GET: api/Estudios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudio>> GetEstudio(int id)
        {
            var estudio = await _context.Estudios.FindAsync(id);

            if (estudio == null)
            {
                return NotFound();
            }

            return estudio;
        }

        // PUT: api/Estudios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudio(int id, Estudio estudio)
        {
            if (id != estudio.IdProf)
            {
                return BadRequest();
            }

            _context.Entry(estudio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudioExists(id))
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

        // POST: api/Estudios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estudio>> PostEstudio(Estudio estudio)
        {
            // Opcionalmente podrías validar existencia de Persona y Profesion aquí
            var persona = await _context.Personas.FindAsync(estudio.CcPer);
            var profesion = await _context.Profesions.FindAsync(estudio.IdProf);

            if (persona == null || profesion == null)
            {
                return BadRequest("Persona o Profesion no existente.");
            }

            // Asignación opcional si quieres seguir manejando navegación
            estudio.CcPerNavigation = persona;
            estudio.IdProfNavigation = profesion;

            _context.Estudios.Add(estudio);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (_context.Estudios.Any(e => e.IdProf == estudio.IdProf && e.CcPer == estudio.CcPer))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetEstudio", new { idProf = estudio.IdProf, ccPer = estudio.CcPer }, estudio);
        }

        // DELETE: api/Estudios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudio(int id)
        {
            var estudio = await _context.Estudios.FindAsync(id);
            if (estudio == null)
            {
                return NotFound();
            }

            _context.Estudios.Remove(estudio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstudioExists(int id)
        {
            return _context.Estudios.Any(e => e.IdProf == id);
        }
    }
}
