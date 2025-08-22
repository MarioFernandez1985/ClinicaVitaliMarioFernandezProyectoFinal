
namespace ClinicaVitaliApi.Models
{
    public class Medico : BaseEntity
    {
        public string Nombre { get; set; } = "";
        public string Cedula_Profesional { get; set; } = "";
        public string Especialidad { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Horario_Consulta { get; set; } = "";
        public string Estado { get; set; } = "Activo";
    }
}
