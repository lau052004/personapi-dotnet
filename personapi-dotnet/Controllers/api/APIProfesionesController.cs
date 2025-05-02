using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers.api
{
    [Route("api/profesiones")]
    [ApiController]
    public class APIProfesionesController : ControllerBase
    {
        private readonly IProfesionRepository _profesionRepository;

        public APIProfesionesController(IProfesionRepository profesionRepository)
        {
            _profesionRepository = profesionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profesiones = await _profesionRepository.GetAllAsync();
            return Ok(profesiones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesion = await _profesionRepository.GetByIdAsync(id);
            if (profesion == null)
            {
                return NotFound();
            }
            return Ok(profesion);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nom, string des)
        {
            var profesion = new Profesion
            {
                Nom = nom,
                Des = des
            };
            await _profesionRepository.AddAsync(profesion);
            return CreatedAtAction(nameof(GetById), new { id = profesion.Id }, profesion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, string nom, string des)
        {
            var profesion = await _profesionRepository.GetByIdAsync(id);
            if (profesion == null)
            {
                return NotFound();
            }

            profesion.Nom = nom;
            profesion.Des = des;

            await _profesionRepository.UpdateAsync(profesion);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var profesion = await _profesionRepository.GetByIdAsync(id);
            if (profesion == null)
            {
                return NotFound();
            }

            await _profesionRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}