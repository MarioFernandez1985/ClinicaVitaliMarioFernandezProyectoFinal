
namespace ClinicaVitaliApi.Models
{
    public class Cita : BaseEntity
    {
        public Guid Id_Paciente { get; set; }
        public Guid Id_Medico { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; } = "08:00";
        public string Especialidad { get; set; } = "";
        public string Estado { get; set; } = "Pendiente";
    }
}
