
namespace ClinicaVitaliApi.DTOs
{
    public class MedicoCreateDto
    {
        public string Nombre { get; set; } = "";
        public string Cedula_Profesional { get; set; } = "";
        public string Especialidad { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Horario_Consulta { get; set; } = "";
        public string Estado { get; set; } = "Activo";
    }
    public class MedicoUpdateDto
    {
        public string? Nombre { get; set; }
        public string? Cedula_Profesional { get; set; }
        public string? Especialidad { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Horario_Consulta { get; set; }
        public string? Estado { get; set; }
    }
}
