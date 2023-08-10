namespace Fanex.BetBuilder.Business.Services;

using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;

public interface MailContentService
{
    string GenerateDailyMailContent(DateTime winlostDate, bool isNoData);

    string GenerateMonthlyMailContent(DateTime fromDate, DateTime toDate, bool isNoData);
}

public class MailContentServiceImp : MailContentService
{
    public string GenerateDailyMailContent(DateTime winlostDate, bool isNoData)
    {
        var tuple = new Tuple<DateTime, bool>(winlostDate, isNoData);
        var template = File.ReadAllText("Templates/DailyEmailTemplate.cshtml");
        const string tplKey = "Fanex.BetBuilder.Service-Daily";
        var result = Engine.Razor.IsTemplateCached(tplKey, null)
            ? Engine.Razor.Run(tplKey, null, tuple)
            : Engine.Razor.RunCompile(template, tplKey, model: tuple);

        return result;
    }

    public string GenerateMonthlyMailContent(DateTime fromDate, DateTime toDate, bool isNoData)
    {
        var tuple = new Tuple<DateTime, DateTime, bool>(fromDate, toDate, isNoData);
        var template = File.ReadAllText("Templates/MonthlyEmailTemplate.cshtml");
        const string tplKey = "Fanex.BetBuilder.Service-Monthly";
        var result = Engine.Razor.IsTemplateCached(tplKey, null)
            ? Engine.Razor.Run(tplKey, null, tuple)
            : Engine.Razor.RunCompile(template, tplKey, model: tuple);

        return result;
    }
}