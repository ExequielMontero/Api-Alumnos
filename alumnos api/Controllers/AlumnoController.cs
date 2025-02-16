using alumnos_api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<IActionResult> GetAlumnos()
    {
        try
        {
            // Obtener la lista de Personas desde el ContextDb
            var alumnos = await _contextDb.GetAlumn();

            // Mapear la lista de Personas a PersonaDTO
            var alumnosDto = alumnos.Select(p => new AlumnosDto
            {
                DNI = p.DNI,
                Nombre = p.Nombre,
                Fecha_Nacimiento = p.Fecha_Nacimiento
            }).ToList();

            return Ok(alumnosDto); // Devolver la lista de PersonaDTO
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
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