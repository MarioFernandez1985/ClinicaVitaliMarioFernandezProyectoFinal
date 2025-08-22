
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicaVitaliApi.Data;
using ClinicaVitaliApi.Models;
using ClinicaVitaliApi.DTOs;

namespace ClinicaVitaliApi.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    [Authorize(AuthenticationSchemes = "Basic")]
    public class PacientesController : ControllerBase
    {
        private readonly IRepository<Paciente> _repo;
        public PacientesController(IRepository<Paciente> repo) { _repo = repo; }

        [HttpGet] public async Task<ActionResult<IEnumerable<Paciente>>> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Paciente>> GetById(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpGet("buscar/{cedula}")]
        public async Task<ActionResult<Paciente>> GetByCedula(string cedula)
        {
            var list = await _repo.GetAllAsync();
            var item = list.FirstOrDefault(p => p.Cedula == cedula);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Paciente>> Create(PacienteCreateDto dto)
        {
            var all = await _repo.GetAllAsync();
            if (all.Any(p => p.Cedula == dto.Cedula))
                return Conflict($"Ya existe un paciente con la cédula {dto.Cedula}");

            var entity = new Paciente
            {
                Nombre = dto.Nombre,
                Cedula = dto.Cedula,
                Fecha_Nacimiento = dto.Fecha_Nacimiento,
                Genero = dto.Genero,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Estado_Clinico = dto.Estado_Clinico,
                Fecha_Registro = DateTime.UtcNow
            };
            await _repo.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, PacienteUpdateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) return NotFound();

            if (dto.Nombre != null) entity.Nombre = dto.Nombre;
            if (dto.Cedula != null) entity.Cedula = dto.Cedula;
            if (dto.Fecha_Nacimiento.HasValue) entity.Fecha_Nacimiento = dto.Fecha_Nacimiento.Value;
            if (dto.Genero != null) entity.Genero = dto.Genero;
            if (dto.Direccion != null) entity.Direccion = dto.Direccion;
            if (dto.Telefono != null) entity.Telefono = dto.Telefono;
            if (dto.Correo != null) entity.Correo = dto.Correo;
            if (dto.Estado_Clinico != null) entity.Estado_Clinico = dto.Estado_Clinico;

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
