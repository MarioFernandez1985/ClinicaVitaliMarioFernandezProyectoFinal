
namespace ClinicaVitaliApi.Models
{
    public class Historial : BaseEntity
    {
        public Guid Id_Paciente { get; set; }
        public Guid Id_Medico { get; set; }
        public string Diagnostico { get; set; } = "";
        public string Tratamiento { get; set; } = "";
        public DateTime Fecha_Consulta { get; set; } = DateTime.UtcNow;
    }
}
