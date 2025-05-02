using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class PersonasController : Controller
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly ITelefonoRepository _telefonoRepository;
        private readonly IEstudioRepository _estudioRepository;

        public PersonasController(IPersonaRepository personaRepository, ITelefonoRepository telefonoRepository, IEstudioRepository estudioRepository)
        {
            _personaRepository = personaRepository;
            _telefonoRepository = telefonoRepository;
            _estudioRepository = estudioRepository;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            var personas = await _personaRepository.GetAllAsync();
            return View(personas);
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _personaRepository.GetByIdAsync(id.Value);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            bool ccExiste = await _personaRepository.GetByIdAsync(persona.Cc) != null;

            if (ccExiste)
            {
                ModelState.AddModelError("Cc", "La persona con esa cédula ya existe.");
            }

            if (ModelState.IsValid)
            {
                await _personaRepository.AddAsync(persona);
                return RedirectToAction(nameof(Index));
            }

            return View(persona);
        }

        // GET: Personas/Edit/#
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _personaRepository.GetByIdAsync(id.Value);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Edit/#
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cc,Nombre,Apellido,Genero,Edad")] Persona persona)
        {
            if (id != persona.Cc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _personaRepository.UpdateAsync(persona);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PersonaExists(persona.Cc))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(persona);
        }

        // GET: Personas/Delete/#
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _personaRepository.GetByIdAsync(id.Value);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/#
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _personaRepository.GetByIdAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            // Eliminar todos los teléfonos asociados a la persona
            var telefonos = await _telefonoRepository.GetByDuenioAsync(id);
            foreach (var telefono in telefonos)
            {
                await _telefonoRepository.DeleteAsync(telefono.Num);
            }

            // Eliminar todos los estudios asociados a la persona
            var estudios = await _estudioRepository.GetAllByCcPerAsync(id);
            foreach (var estudio in estudios)
            {
                await _estudioRepository.DeleteAsync(estudio.CcPer, estudio.IdProf);
            }

            // Eliminar la persona
            await _personaRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PersonaExists(int id)
        {
            var persona = await _personaRepository.GetByIdAsync(id);
            return persona != null;
        }
    }
}