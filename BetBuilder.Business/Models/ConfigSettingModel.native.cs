namespace BetBuilder.Business.Models;

using System;

public class ConfigSettingModel
{
    public bool IsManual { get; set; }

    public DateTime WinlostDate { get; set; }

    public string FilePath { get; set; }

    public string Password { get; set; }                
}