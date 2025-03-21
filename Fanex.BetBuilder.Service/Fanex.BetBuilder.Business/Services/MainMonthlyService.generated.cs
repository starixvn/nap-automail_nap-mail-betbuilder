namespace Fanex.BetBuilder.Business.Services;

using Fanex.MailDelivery.Core.Models;
using Models;
using System;

public interface MainMonthlyService
{
    JobMessage Execute();
}

public class MainMonthlyServiceImp : MainMonthlyService
{
    private readonly Log log;
    private readonly DataService dataService;
    private readonly TransformService transformService;
    private readonly ExcelService excelService;
    private readonly MailService mailService;

    public MainMonthlyServiceImp(
        Log log,
        DataService dataService,
        TransformService transformService,
        MailService mailService,
        ExcelService excelService)
    {
        this.log = log;
        this.dataService = dataService;
        this.transformService = transformService;
        this.mailService = mailService;
        this.excelService = excelService;
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
        var rawData = dataService.QueryMonthlyData();
        var data = transformService.ExecuteMonthly(rawData);
        var isNoData = data.Count == 0;
        var fileName = isNoData ? string.Empty : excelService.Execute(data);
        var mail = mailService.SendMonthlyMail(fileName, isNoData);

        return new ExecuteResultModel { Success = true, Mail = mail };
    }
}