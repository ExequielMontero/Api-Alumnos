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


    public async Task<IEnumerable>
}