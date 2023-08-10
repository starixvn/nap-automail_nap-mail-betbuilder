namespace Fanex.BetBuilder.Service;

using Business.Services;
using Fanex.BetBuilder.Business.Models;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.MailDelivery.Client;
using Fanex.RemotingRef;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Services { get; set; }

    public static void LoadConfiguration(this IServiceCollection services)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        var text = File.ReadAllText("appsettings.json");
        AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
    }

    public static void RegisterSentry(this IServiceCollection services, IConfiguration configuration)
    {
        LogManager
            .SetDefaultLogCategory(configuration.GetSection("LogCategory").Value)
            .Use(new List<ILogEngine>
            {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(configuration.GetSection("SentryEngineOptions:Dsn").Value)
                })
            });
    }

    public static void RegisterFanexData(this IServiceCollection services, IConfiguration configuration)
    {
        var connections = configuration.GetSection("ConnectionStrings")
           .GetChildren()
           .ToDictionary(
               x => x.Key,
               y => new ConnectionConfiguration(y.Key, y.Value)
           );

        DbSettingProviderManager
            .StartNewSession()
            .Use(connections)
            .WithSqlServer()
            .UseDefaultDbSettingProvider(configuration.GetSection("StoredProcedurePath").Value)
            .Run();
    }

    public static void RegisterRemotingRef(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration.GetSection("Fanex.RemotingRef.Service.Url").Value;
        RefDataTypeContainer
           .ConnectTo(new Uri(url))
           .FromApplication("Fanex.BetBuilder.Service")
           .SupportOnlyDefaultLanguage();
    }

    public static void InternalServiceRegister(this IServiceCollection services)
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
        MailClientHandler.Start();
    }
}