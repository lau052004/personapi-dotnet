using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers.api
{
    [Route("api/telefonos")]
    [ApiController]
    public class APITelefonoController : ControllerBase
    {
        private readonly ITelefonoRepository _telefonoRepository;
        private readonly IPersonaRepository _personaRepository;

        public APITelefonoController(ITelefonoRepository telefonoRepository, IPersonaRepository personaRepository)
        {
            _telefonoRepository = telefonoRepository;
            _personaRepository = personaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var telefonos = await _telefonoRepository.GetAllAsync();
            return Ok(telefonos);
        }

        [HttpGet("{numero}")]
        public async Task<IActionResult> GetByNumber(string numero)
        {
            var telefono = await _telefonoRepository.GetByNumberAsync(numero);
            if (telefono == null)
            {
                return NotFound();
            }
            return Ok(telefono);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string numero, string operador, int duenioId)
        {
            var persona = await _personaRepository.GetByIdAsync(duenioId);
            if (persona == null)
            {
                return BadRequest("La persona con la cédula proporcionada no existe.");
            }

            // Verificar si el teléfono ya existe
            if (await _telefonoRepository.TelefonoExistsAsync(numero))
            {
                return Conflict("El número de teléfono ya existe.");
            }

            var telefono = new Telefono
            {
                Num = numero,
                Oper = operador,
                DuenioNavigation = persona
            };

            await _telefonoRepository.AddAsync(telefono);

            return CreatedAtAction(nameof(GetByNumber), new { numero = telefono.Num }, telefono);
        }

        [HttpPut("{numero}")]
        public async Task<IActionResult> Update(string numero, string operador, int duenioId)
        {
            var telefono = await _telefonoRepository.GetByNumberAsync(numero);
            if (telefono == null)
            {
                return NotFound();
            }

            var persona = await _personaRepository.GetByIdAsync(duenioId);
            if (persona == null)
            {
                return BadRequest("La persona con la cédula proporcionada no existe.");
            }

            telefono.Oper = operador;
            telefono.DuenioNavigation = persona;

            await _telefonoRepository.UpdateAsync(telefono);

            return NoContent();
        }

        [HttpDelete("{numero}")]
        public async Task<IActionResult> Delete(string numero)
        {
            var telefono = await _telefonoRepository.GetByNumberAsync(numero);
            if (telefono == null)
            {
                return NotFound();
            }

            await _telefonoRepository.DeleteAsync(numero);
            return NoContent();
        }
    }
}