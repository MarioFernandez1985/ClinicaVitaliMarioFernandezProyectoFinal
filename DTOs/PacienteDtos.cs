
namespace ClinicaVitaliApi.DTOs
{
    public class PacienteCreateDto
    {
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";
        public DateTime Fecha_Nacimiento { get; set; }
        public string Genero { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Estado_Clinico { get; set; } = "";
    }
    public class PacienteUpdateDto
    {
        public string? Nombre { get; set; }
        public string? Cedula { get; set; }
        public DateTime? Fecha_Nacimiento { get; set; }
        public string? Genero { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Estado_Clinico { get; set; }
    }
}
