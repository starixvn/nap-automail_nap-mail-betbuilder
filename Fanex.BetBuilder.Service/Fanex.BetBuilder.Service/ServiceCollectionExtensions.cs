namespace Fanex.BetBuilder.Service;

using Business.Services;
using Fanex.BetBuilder.Business.Models;
using Fanex.Data.Repository;
using Fanex.MailDelivery.Client;
using Fanex.RemotingRef;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.IO;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Services { get; set; }

    public static void AddConfiguration(this IServiceCollection services)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        var text = File.ReadAllText("appsettings.json");
        AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
    }

    public static void AddRemotingRef(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration.GetSection("Fanex.RemotingRef.Service.Url").Value;
        RefDataTypeContainer
           .ConnectTo(new Uri(url))
           .FromApplication("Fanex.BetBuilder.Service")
           .SupportOnlyDefaultLanguage();
    }

    public static void AddInternalService(this IServiceCollection services)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<Log, LogImp>();
        services.AddSingleton<CacheRefService, CacheRefServiceImp>();
        services.AddSingleton<IRemotingRefService, RemotingRefService>();
        services.AddSingleton<CriteriaService, CriteriaServiceImp>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<DataService, DataServiceImp>();
        services.AddSingleton<TransformService, TransformServiceImp>();
        services.AddSingleton<CsvService, CsvServiceImp>();
        services.AddSingleton<MailContentService, MailContentServiceImp>();
        services.AddSingleton<MailService, MailServiceImp>();
        services.AddSingleton<DateTimeService, DateTimeServiceImp>();
        services.AddSingleton<ExcelService, ExcelServiceImp>();
        services.AddSingleton<MainDailyService, MainDailyServiceImp>();
        services.AddSingleton<MainMonthlyService, MainMonthlyServiceImp>();

        Services = services;

        try
        {
            MailClientHandler.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}