namespace BetBuilder.DailyService.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{
    [HttpGet]
    public string Index()
    {
        return "OK";
    }
}