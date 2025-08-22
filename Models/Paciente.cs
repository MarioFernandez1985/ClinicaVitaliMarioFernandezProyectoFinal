
namespace ClinicaVitaliApi.Models
{
    public class Paciente : BaseEntity
    {
        public string Nombre { get; set; } = "";
        public string Cedula { get; set; } = "";
        public DateTime Fecha_Nacimiento { get; set; }
        public string Genero { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Estado_Clinico { get; set; } = "";
        public DateTime Fecha_Registro { get; set; } = DateTime.UtcNow;
    }
}
