using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Data;
using System.Net;

namespace alumnos_api.Models
{
    public class Alumnos
    {

        public int Id { get; set; }
        public int DNI { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime? Fecha_Nacimiento { get; set; }


        /* Id INT IDENTITY(1,1),
           DNI INT NOT NULL,
           Nombre NVARCHAR(100) NOT NULL,
           Fecha_Nacimiento DATE,
        */
    }
}
