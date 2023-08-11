namespace Fanex.BetBuilder.Service.Controllers;

using Business.Services;
using Fanex.MailDelivery.Core.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ExecuteDailyController : ControllerBase
{
    private readonly MainDailyService mainService;

    public ExecuteDailyController(MainDailyService mainService)
    {
        this.mainService = mainService;
    }

    [HttpGet]
    public JobMessage Get()
    {
        return mainService.Execute();
    }
}