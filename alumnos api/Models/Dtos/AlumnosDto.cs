namespace alumnos_api.Models.Dtos
{
    public class AlumnosDto
    {
        public int DNI { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime? Fecha_Nacimiento { get; set; }
    }
}
