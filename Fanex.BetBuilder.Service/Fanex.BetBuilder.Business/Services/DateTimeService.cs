namespace Fanex.BetBuilder.Business.Services;

using System;

public interface DateTimeService
{
    DateTime FromDate();

    DateTime ToDate();
}

public class DateTimeServiceImp : DateTimeService
{
    public DateTime FromDate()
    {
        var date = AppConfigService.Config.IsManual ? AppConfigService.Config.WinlostDate : DateTime.Today.AddDays(-1);
        date = date.AddDays(-(date.Day - 1)).AddMonths(-1);

        return date;
    }

    public DateTime ToDate()
    {
        var date = AppConfigService.Config.IsManual ? AppConfigService.Config.WinlostDate : DateTime.Today.AddDays(-1);
        date = date.AddDays(-date.Day);

        return date;
    }
}