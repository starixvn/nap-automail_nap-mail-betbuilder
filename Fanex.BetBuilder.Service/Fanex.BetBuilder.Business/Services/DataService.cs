namespace Fanex.BetBuilder.Business.Services;

using System.Collections.Generic;
using System.Linq;
using Criteria;
using Fanex.BetBuilder.Business.Models;
using Fanex.Data.Repository;
using Models;

public interface DataService
{
    List<DailyRawDataModel> QueryDailyData();

    List<MonthlyRawDataModel> QueryMonthlyData();
}

public class DataServiceImp : DataService
{
    private readonly IDynamicRepository repository;
    private readonly CriteriaService param;

    public DataServiceImp(IDynamicRepository repository, CriteriaService param)
    {
        this.repository = repository;
        this.param = param;
    }

    public List<DailyRawDataModel> QueryDailyData()
    {
        var criteria = new DailyBetBuilderCriteria
        {
            WinlostDate = param.WinlostDate
        };

        return repository.Fetch<DailyRawDataModel>(criteria).ToList();
    }

    public List<MonthlyRawDataModel> QueryMonthlyData()
    {
        var criteria = new MonthlyBetBuilderCriteria
        {
            RptDate = param.WinlostDate
        };

        return repository.Fetch<MonthlyRawDataModel>(criteria).ToList();
    }
}