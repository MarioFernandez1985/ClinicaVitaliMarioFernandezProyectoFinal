
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicaVitaliApi.Data;
using ClinicaVitaliApi.Models;
using ClinicaVitaliApi.DTOs;

namespace ClinicaVitaliApi.Controllers
{
    [ApiController]
    [Route("api/historial")]
    [Authorize(AuthenticationSchemes = "Basic")]
    public class HistorialController : ControllerBase
    {
        private readonly IRepository<Historial> _repo;
        private readonly IRepository<Paciente> _pacientes;
        private readonly IRepository<Medico> _medicos;

        public HistorialController(IRepository<Historial> repo, IRepository<Paciente> pacientes, IRepository<Medico> medicos)
        {
            _repo = repo; _pacientes = pacientes; _medicos = medicos;
        }

        [HttpGet] public async Task<ActionResult<IEnumerable<Historial>>> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Historial>> GetById(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Historial>> Create(HistorialCreateDto dto)
        {
            if (await _pacientes.GetByIdAsync(dto.Id_Paciente) is null)
                return BadRequest($"El paciente {dto.Id_Paciente} no existe");
            if (await _medicos.GetByIdAsync(dto.Id_Medico) is null)
                return BadRequest($"El medico {dto.Id_Medico} no existe");

            var entity = new Historial
            {
                Id_Paciente = dto.Id_Paciente,
                Id_Medico = dto.Id_Medico,
                Diagnostico = dto.Diagnostico,
                Tratamiento = dto.Tratamiento,
                Fecha_Consulta = dto.Fecha_Consulta
            };
            await _repo.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, HistorialUpdateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) return NotFound();

            if (dto.Id_Paciente.HasValue)
            {
                if (await _pacientes.GetByIdAsync(dto.Id_Paciente.Value) is null)
                    return BadRequest($"El paciente {dto.Id_Paciente} no existe");
                entity.Id_Paciente = dto.Id_Paciente.Value;
            }
            if (dto.Id_Medico.HasValue)
            {
                if (await _medicos.GetByIdAsync(dto.Id_Medico.Value) is null)
                    return BadRequest($"El medico {dto.Id_Medico} no existe");
                entity.Id_Medico = dto.Id_Medico.Value;
            }
            if (dto.Diagnostico != null) entity.Diagnostico = dto.Diagnostico;
            if (dto.Tratamiento != null) entity.Tratamiento = dto.Tratamiento;
            if (dto.Fecha_Consulta.HasValue) entity.Fecha_Consulta = dto.Fecha_Consulta.Value;

            await _repo.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
