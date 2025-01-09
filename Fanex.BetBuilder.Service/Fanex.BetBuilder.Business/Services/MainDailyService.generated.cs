namespace Fanex.BetBuilder.Business.Services;

using Fanex.BetBuilder.Business.Models;
using Fanex.MailDelivery.Core.Models;
using System;

public interface MainDailyService
{
    JobMessage Execute();
}

public class MainDailyServiceImp : MainDailyService
{
    private readonly Log log;
    private readonly DataService dataService;
    private readonly TransformService transformService;
    private readonly CsvService csvService;
    private readonly MailService mailService;

    public MainDailyServiceImp(
        Log log,
        DataService dataService,
        TransformService transformService,
        CsvService csvService,
        MailService mailService)
    {
        this.log = log;
        this.dataService = dataService;
        this.transformService = transformService;
        this.csvService = csvService;
        this.mailService = mailService;
    }

    public JobMessage Execute()
    {
        var result = new ExecuteResultModel();
        var jobMessage = new JobMessage();
        try
        {
            AppConfigService.CounterList.Add(1);
            result = SendNotification();
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.Message;
            log.Log(e);
        }
        finally
        {
            jobMessage.Mail = result.Mail;
            jobMessage.JobStatus = result.Success ? JobStatus.Success : JobStatus.DataIsNotReady;
            jobMessage.TaskLog = new TaskLog
            {
                LogTime = DateTime.Now,
                Message = result.Message
            };

            if (AppConfigService.CounterList.Count > 0)
            {
                AppConfigService.CounterList.RemoveAt(0);
            }
            if (AppConfigService.CounterList.Count == 0)
            {
                AppConfigService.SafeToRestart = true;
            }
        }

        return jobMessage;
    }

    private ExecuteResultModel SendNotification()
    {
        var rawData = dataService.QueryDailyData();
        var data = transformService.ExecuteDaily(rawData);
        var isNoData = data.Count == 1;
        var fileName = isNoData ? string.Empty : csvService.ExportCsv(data);
        var mail = mailService.SendDailyMail(fileName, isNoData);

        return new ExecuteResultModel { Success = true, Mail = mail };
    }
}