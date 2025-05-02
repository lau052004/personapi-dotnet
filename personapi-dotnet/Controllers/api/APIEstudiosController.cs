using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Interfaces;
using personapi_dotnet.Models.Entities;
using System.Text.Json.Serialization;

namespace personapi_dotnet.Controllers.api
{
    [Route("api/estudios")]
    [ApiController]
    public class APIEstudiosController : ControllerBase
    {
        private readonly IEstudioRepository _estudioRepository;
        private readonly IProfesionRepository _profesionRepository;
        private readonly IPersonaRepository _personaRepository;

        public APIEstudiosController(IEstudioRepository estudioRepository, IProfesionRepository profesionRepository, IPersonaRepository personaRepository)
        {
            _estudioRepository = estudioRepository;
            _profesionRepository = profesionRepository;
            _personaRepository = personaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var estudios = await _estudioRepository.GetAllAsync();
            return Ok(estudios);
        }

        [HttpGet("{ccPer}/{idProf}")]
        public async Task<IActionResult> GetById(int ccPer, int idProf)
        {
            var estudio = await _estudioRepository.GetByIdAsync(ccPer, idProf);
            if (estudio == null)
            {
                return NotFound();
            }
            return Ok(estudio);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id_profesion, int cc_persona, DateOnly date, string universidad)
        {
            // Verificar si el estudio ya existe
            var existingEstudio = await _estudioRepository.GetByIdAsync(cc_persona, id_profesion);

            if (existingEstudio != null)
            {
                return Conflict("El estudio ya existe para esta persona y profesión.");
            }

            // Crear un nuevo objeto Estudio
            var estudio = new Estudio
            {
                IdProf = id_profesion,
                CcPer = cc_persona,
                Fecha = date,
                Univer = universidad
            };

            // Agregar el estudio a la lista de estudios de la persona
            var persona = await _personaRepository.GetByIdAsync(cc_persona);
            if (persona == null)
            {
                return NotFound($"Persona con ID {cc_persona} no encontrada.");
            }

            persona.Estudios.Add(estudio);
            await _personaRepository.UpdateAsync(persona);

            // Agregar el estudio a la lista de estudios de la profesión
            var profesion = await _profesionRepository.GetByIdAsync(id_profesion);
            if (profesion == null)
            {
                return NotFound($"Profesión con ID {id_profesion} no encontrada.");
            }

            profesion.Estudios.Add(estudio);
            await _profesionRepository.UpdateAsync(profesion);

            // Configurar las opciones de serialización para evitar ciclos de referencias
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            // Serializar el estudio con las opciones configuradas
            var serializedEstudio = JsonSerializer.Serialize(estudio, options);

            // Devolver una respuesta exitosa con el estudio creado
            return CreatedAtAction(nameof(GetById), new { ccPer = estudio.CcPer, idProf = estudio.IdProf }, serializedEstudio);
        }

        [HttpPut("{ccPer}/{idProf}")]
        public async Task<IActionResult> Update(int ccPer, int idProf, string universidad, DateOnly date)
        {
            // Obtener el estudio a actualizar
            var estudio = await _estudioRepository.GetByIdAsync(ccPer, idProf);

            // Verificar si el estudio existe
            if (estudio == null)
            {
                return NotFound();
            }

            // Actualizar los campos de universidad si no están vacíos
            if (!string.IsNullOrEmpty(universidad))
            {
                estudio.Univer = universidad;
            }

            // Actualizar el campo de fecha si no está vacío
            if (date != default)
            {
                estudio.Fecha = date;
            }

            // Actualizar el estudio en el repositorio
            await _estudioRepository.UpdateAsync(estudio);

            return NoContent();
        }

        [HttpDelete("{ccPer}/{idProf}")]
        public async Task<IActionResult> Delete(int ccPer, int idProf)
        {
            await _estudioRepository.DeleteAsync(ccPer, idProf);
            return NoContent();
        }
    }
}