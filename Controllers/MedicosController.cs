
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicaVitaliApi.Data;
using ClinicaVitaliApi.Models;
using ClinicaVitaliApi.DTOs;

namespace ClinicaVitaliApi.Controllers
{
    [ApiController]
    [Route("api/medicos")]
    [Authorize(AuthenticationSchemes = "Basic")]
    public class MedicosController : ControllerBase
    {
        private readonly IRepository<Medico> _repo;
        public MedicosController(IRepository<Medico> repo) { _repo = repo; }

        [HttpGet] public async Task<ActionResult<IEnumerable<Medico>>> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Medico>> GetById(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Medico>> Create(MedicoCreateDto dto)
        {
            var entity = new Medico
            {
                Nombre = dto.Nombre,
                Cedula_Profesional = dto.Cedula_Profesional,
                Especialidad = dto.Especialidad,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Horario_Consulta = dto.Horario_Consulta,
                Estado = dto.Estado ?? "Activo"
            };
            await _repo.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, MedicoUpdateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) return NotFound();

            if (dto.Nombre != null) entity.Nombre = dto.Nombre;
            if (dto.Cedula_Profesional != null) entity.Cedula_Profesional = dto.Cedula_Profesional;
            if (dto.Especialidad != null) entity.Especialidad = dto.Especialidad;
            if (dto.Telefono != null) entity.Telefono = dto.Telefono;
            if (dto.Correo != null) entity.Correo = dto.Correo;
            if (dto.Horario_Consulta != null) entity.Horario_Consulta = dto.Horario_Consulta;
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
