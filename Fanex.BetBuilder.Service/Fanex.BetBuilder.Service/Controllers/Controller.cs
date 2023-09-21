namespace Fanex.BetBuilder.Service.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{
    private const string OK = "OK";

    [HttpGet]
    public string Index()
    {
        return OK;
    }
}