namespace BetBuilder.DailyService;

using System.Collections.Generic;
using System.Linq;
using Business.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.RemotingRef;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using SharpRaven;

public static class ServiceCollectionExtensions
{
   
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
            .AsEnumerable()
            .ToDictionary(
                x => x.GetValue<string>("Name"),
                y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName"))
            );

        DbSettingProviderManager
            .StartNewSession()
            .Use(connections)
            .WithSqlServer()
            .UseDefaultDbSettingProvider(resourcePath: configuration.GetSection("StoredProcedurePath").Value)
            .Run();
    }

    public static void RegisterOnebookUm(this IServiceCollection services, IConfiguration configuration)
    {
        var umConfig = new UMClientConfiguration();
        services.AddSingleton(umConfig.Initialize(configuration));
        services.AddSingleton<IUMCacheService, MemoryUmCacheService>();
        services.AddSingleton<IUMRestClient, UMRestClient>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
        services.AddSingleton<IMaintenanceService, MaintenanceService>();
    }
    
    public static void InternalServiceRegister(this IServiceCollection services)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<Log, LogImp>();
        services.AddSingleton<CacheRefService, CacheRefServiceImp>();
        services.AddSingleton<IRemotingRefService, RemotingRefService>();
        services.AddSingleton<CriteriaService, CriteriaServiceImp>();
        services.AddSingleton<DailyBetBuilderNotificationService, DailyBetBuilderNotificationServiceImp>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<DataService, DataServiceImp>();
        services.AddSingleton<TransformService, TransformServiceImp>();
        services.AddSingleton<CsvService, CsvServiceImp>();
        services.AddSingleton<MailContentService, MailContentServiceImp>();
        services.AddSingleton<MailService, MailServiceImp>();
        services.AddSingleton<DateTimeService, DateTimeServiceImp>();
        services.AddSingleton<DailyBetBuilderNotificationService, DailyBetBuilderNotificationServiceImp>();
    }
}