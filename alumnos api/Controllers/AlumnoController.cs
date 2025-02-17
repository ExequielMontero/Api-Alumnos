using alumnos_api.Models;
using alumnos_api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;


[ApiController]
[Route("api/[controller]")]
public class AlumnoController : ControllerBase
{
    private readonly ContextDb _contextDb;

    public AlumnoController(ContextDb contextDb)
    {
        _contextDb = contextDb;
    }

    [HttpGet]

    public async Task<IActionResult> getAlumnos()
    {
        try
        {
            // Obtener la lista de alumnos desde el ContextDb
            var alumnos = await _contextDb.GetAlumn();


            return Ok(alumnos); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
        }
    }

    [HttpPost]
    [Route("insert")]
    public async Task<ActionResult> postAlumn(AlumnosDto alumno)
    {
        try
        {
            bool bandera = await _contextDb.InsertAlumn(alumno);
            if (bandera != true)
            {
                return BadRequest("Error");
            }

            return Ok(bandera);
        }
        catch(Exception e)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
        }
    }

    [HttpPut]
    [Route("update/{id}")]
    public async Task<IActionResult> ActualizarAlumno(int id, [FromBody] AlumnosDto alumnoDto)
    {
        try
        {
            // Mapear DTO a la entidad y asignar el Id
            var alumno = new Alumnos
            {
                Id = id, // <-- Id viene de la ruta
                DNI = alumnoDto.DNI,
                Nombre = alumnoDto.Nombre,
                Fecha_Nacimiento = alumnoDto.Fecha_Nacimiento
            };

            bool resultado = await _contextDb.ActualizarAlumno(alumno);

            if (resultado)
            {
                return Ok("Alumno actualizado correctamente");
            }
            else
            {
                return NotFound("No se encontró el alumno con el Id especificado");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpPatch]
    [Route("alumnos/{id}/nombre")]
    public async Task<IActionResult> ActualizarNombreAlumno(int id, [FromBody] ActualizarNombreRequestDto nombre)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new { exito = false, mensaje = "ID inválido" });
            }

            if (string.IsNullOrWhiteSpace(nombre.Nombre))
            {
                return BadRequest(new { exito = false, mensaje = "El nombre no puede estar vacío" });
            }


            // Actualizar en la base de datos
            bool resultado = await _contextDb.ActualizarNombreAlumno(id, nombre.Nombre);

            return resultado
                          ? Ok(new { exito = true, mensaje = "Nombre actualizado correctamente" })
                          : NotFound(new { exito = false, mensaje = "Alumno no encontrado" });
        }
        catch (SqliteException ex)
        {
            return StatusCode(500, new { exito = false, mensaje = "Error de base de datos"});
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { exito = false, mensaje = "Error interno del servidor" });
        }
    }

    [HttpPatch]
    [Route("alumnos/{id}/dni")]
    public async Task<IActionResult> ActualizarDniAlumno(int id, [FromBody] ActualizarDniRequestDto dni)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new { exito = false, mensaje = "ID inválido" });
            }

            if (dni.DNI<=4000000) //un ejemplo
            {
                return BadRequest(new { exito = false, mensaje = "El Dni solo puedo contener 8 digitos" });
            }


            // Actualizar en la base de datos
            bool resultado = await _contextDb.ActualizarDniAlumno(id, dni.DNI);

            return resultado
                          ? Ok(new { exito = true, mensaje = "Dni actualizado correctamente" })
                          : NotFound(new { exito = false, mensaje = "Alumno no encontrado" });
        }
        catch (SqliteException ex)
        {
            return StatusCode(500, new { exito = false, mensaje = "Error de base de datos" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { exito = false, mensaje = "Error interno del servidor" });
        }
    }
    /* --Ejecucion script para crear DB
     * 
    [HttpPost]
    [Route("script")]
    public async Task<ActionResult> InsertScript()
    {
        try
        {
            await _contextDb.InsertScriptDb();
            return Ok("Script ejecutado correctamente.");
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
    */

}