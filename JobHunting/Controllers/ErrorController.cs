using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace JobHunting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ErrorController : ControllerBase
{
    [HttpGet("notfound")]
    [DisplayName("/api/error/notfound")]
    public async Task<IActionResult> NotFound()
        => NotFound("Страница не найдена");
}
