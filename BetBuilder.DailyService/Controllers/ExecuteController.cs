namespace BetBuilder.DailyService.Controllers;

using Business.Services;
using Fanex.MailDelivery.Core.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ExecuteController : ControllerBase
{
    private readonly DailyBetBuilderNotificationService mainService;

    public ExecuteController(DailyBetBuilderNotificationService mainService)
    {
        this.mainService = mainService;
    }

    [HttpGet]
    public JobMessage Get()
    {
        return mainService.Execute();
    }
}