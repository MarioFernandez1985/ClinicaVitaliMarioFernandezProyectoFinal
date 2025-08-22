
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicaVitaliApi.Data;
using ClinicaVitaliApi.Models;
using ClinicaVitaliApi.DTOs;

namespace ClinicaVitaliApi.Controllers
{
    [ApiController]
    [Route("api/citas")]
    [Authorize(AuthenticationSchemes = "Basic")]
    public class CitasController : ControllerBase
    {
        private readonly IRepository<Cita> _repo;
        private readonly IRepository<Paciente> _pacientes;
        private readonly IRepository<Medico> _medicos;

        public CitasController(IRepository<Cita> repo, IRepository<Paciente> pacientes, IRepository<Medico> medicos)
        {
            _repo = repo;
            _pacientes = pacientes;
            _medicos = medicos;
        }

        [HttpGet] public async Task<ActionResult<IEnumerable<Cita>>> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Cita>> GetById(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Cita>> Create(CitaCreateDto dto)
        {
            if (await _pacientes.GetByIdAsync(dto.Id_Paciente) is null)
                return BadRequest($"El paciente {dto.Id_Paciente} no existe");
            if (await _medicos.GetByIdAsync(dto.Id_Medico) is null)
                return BadRequest($"El medico {dto.Id_Medico} no existe");

            var entity = new Cita
            {
                Id_Paciente = dto.Id_Paciente,
                Id_Medico = dto.Id_Medico,
                Fecha = dto.Fecha.Date,
                Hora = dto.Hora,
                Especialidad = dto.Especialidad,
                Estado = dto.Estado ?? "Pendiente"
            };
            await _repo.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, CitaUpdateDto dto)
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
            if (dto.Fecha.HasValue) entity.Fecha = dto.Fecha.Value.Date;
            if (dto.Hora != null) entity.Hora = dto.Hora;
            if (dto.Especialidad != null) entity.Especialidad = dto.Especialidad;
            if (dto.Estado != null) entity.Estado = dto.Estado;

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
