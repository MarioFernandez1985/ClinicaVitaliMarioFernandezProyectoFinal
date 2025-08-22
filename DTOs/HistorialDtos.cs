
namespace ClinicaVitaliApi.DTOs
{
    public class HistorialCreateDto
    {
        public Guid Id_Paciente { get; set; }
        public Guid Id_Medico { get; set; }
        public string Diagnostico { get; set; } = "";
        public string Tratamiento { get; set; } = "";
        public DateTime Fecha_Consulta { get; set; }
    }
    public class HistorialUpdateDto
    {
        public Guid? Id_Paciente { get; set; }
        public Guid? Id_Medico { get; set; }
        public string? Diagnostico { get; set; }
        public string? Tratamiento { get; set; }
        public DateTime? Fecha_Consulta { get; set; }
    }
}
