using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Data;
using System.Net;

namespace alumnos_api.Models
{
    public class Alumno
    {

        public int Id { get; set; }
        public int Dni { get; set; }
        public string Name { get; set; } 
        public DateTime? MyProperty { get; set; }


        /* Id INT IDENTITY(1,1),
     DNI INT NOT NULL,
         Nombre NVARCHAR(100) NOT NULL,
     Fecha_Nacimiento DATE,
     --
     CONSTRAINT[PK_Personas] PRIMARY KEY CLUSTERED
     (
         [Id] ASC
     )WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
             ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON,
             OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]
    ) ON[PRIMARY];*/
    }
}
