namespace Fanex.BetBuilder.Business.Services;

using Fanex.BetBuilder.Business.Models;
using System.Collections.Generic;

public static class AppConfigService
{
    public static ConfigSettingModel Config { get; set; }

    public static bool SafeToRestart { get; set; }

    public static List<int> CounterList { get; set; } = [];
}