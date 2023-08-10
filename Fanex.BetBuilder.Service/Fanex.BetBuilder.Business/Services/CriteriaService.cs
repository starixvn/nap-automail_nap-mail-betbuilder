namespace Fanex.BetBuilder.Business.Services;

using System;

public interface CriteriaService
{
    DateTime WinlostDate { get; }
}

public class CriteriaServiceImp : CriteriaService
{
    public DateTime WinlostDate => AppConfigService.Config.IsManual ? AppConfigService.Config.WinlostDate : Convert.ToDateTime($"{DateTime.Today.AddDays(-1):MM/dd/yyyy}");
}