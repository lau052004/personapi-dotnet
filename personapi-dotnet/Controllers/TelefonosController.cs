using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class TelefonosController : Controller
    {
        private readonly ITelefonoRepository _telefonoRepository;
        private readonly PersonaDbContext _context;

        public TelefonosController(ITelefonoRepository telefonoRepository, PersonaDbContext context)
        {
            _telefonoRepository = telefonoRepository;
            _context = context;
        }

        // GET: Telefonos
        public async Task<IActionResult> Index()
        {
            var telefonos = await _telefonoRepository.GetAllAsync();
            return View(telefonos);
        }

        // GET: Telefonos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _telefonoRepository.GetByNumberAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // GET: Telefonos/Create
        public IActionResult Create()
        {
            ViewData["Duenio"] = new SelectList(_context.Personas, "Cc", "Cc");
            return View();
        }

        // POST: Telefonos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            // Verificar si el teléfono ya existe
            if (await _telefonoRepository.TelefonoExistsAsync(telefono.Num))
            {
                ModelState.AddModelError("Num", "El número de teléfono ya existe.");
            }
            else
            {
                var persona = await _context.Personas.FirstOrDefaultAsync(m =>
                    m.Cc == telefono.Duenio
                );
                telefono.DuenioNavigation = persona;
                ModelState.Clear();
                TryValidateModel(telefono);

                if (ModelState.IsValid) // Solo si no hay errores en el ModelState
                {
                    await _telefonoRepository.AddAsync(telefono);
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Duenio"] = new SelectList(_context.Personas, "Cc", "Cc", telefono.Duenio);
            return View(telefono);
        }

        // GET: Telefonos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _telefonoRepository.GetByNumberAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }
            ViewData["Duenio"] = new SelectList(_context.Personas, "Cc", "Cc", telefono.Duenio);
            return View(telefono);
        }

        // POST: Telefonos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Num,Oper,Duenio")] Telefono telefono)
        {
            if (id != telefono.Num)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FirstOrDefaultAsync(m =>
                m.Cc == telefono.Duenio
            );
            telefono.DuenioNavigation = persona;
            ModelState.Clear();
            TryValidateModel(telefono);

            if (ModelState.IsValid)
            {
                try
                {
                    await _telefonoRepository.UpdateAsync(telefono);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _telefonoRepository.TelefonoExistsAsync(telefono.Num))
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
            ViewData["Duenio"] = new SelectList(_context.Personas, "Cc", "Cc", telefono.Duenio);
            return View(telefono);
        }

        // GET: Telefonos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _telefonoRepository.GetByNumberAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // POST: Telefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var telefono = await _telefonoRepository.GetByNumberAsync(id);
            if (telefono != null)
            {
                await _telefonoRepository.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TelefonoExists(string id)
        {
            return await _telefonoRepository.TelefonoExistsAsync(id);
        }
    }
}