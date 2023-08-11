namespace Fanex.BetBuilder.Business.Criteria;

using Fanex.Data.Repository;
using System;

public class DailyBetBuilderCriteria : CriteriaBase
{
    public DateTime WinlostDate { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "Acc_Rpt_Daily_Bet_Builder_AutoMail";
}