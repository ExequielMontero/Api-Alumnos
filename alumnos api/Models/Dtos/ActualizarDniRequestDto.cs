using System.ComponentModel.DataAnnotations;

namespace alumnos_api.Models.Dtos
{
    public class ActualizarDniRequestDto
    {
        [Required]
        public int DNI { get; set; }
    }
}
