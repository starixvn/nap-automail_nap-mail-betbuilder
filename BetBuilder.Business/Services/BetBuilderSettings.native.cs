namespace BetBuilder.Business.Services;

using System;
using Microsoft.Extensions.Configuration;

public static class BetBuilderSettings
{
    public static void LoadSettings(IConfiguration configuration)
    {
        Password = configuration.GetValue<string>("Password");
        FilePath = configuration.GetValue<string>("FilePath");
        WinlostDate = configuration.GetValue<DateTime>("WinlostDate");
        IsManual = configuration.GetValue<bool>("IsManual");
    }


    public static bool IsManual { get;private set; }

    public static DateTime WinlostDate { get; private set; }

    public static string FilePath { get; private set; }

    public static string Password { get; private set; }
}