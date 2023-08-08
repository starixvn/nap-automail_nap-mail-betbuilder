namespace BetBuilder.Business.Services;

using System;

public interface CriteriaService
{
    DateTime WinlostDate { get; }
}

public class CriteriaServiceImp : CriteriaService
{
    // Robert: coi lai cho nay
    // public DateTime WinlostDate => BetBuilderSettings.AppConfig.IsManual ? BetBuilderSettings.AppConfig.WinlostDate : Convert.ToDateTime($"{SystemTime.Now.AddDays(-1):MM/dd/yyyy}");
    public DateTime WinlostDate => BetBuilderSettings.IsManual ? BetBuilderSettings.WinlostDate : Convert.ToDateTime($"{DateTime.Today.AddDays(-1):MM/dd/yyyy}");
}