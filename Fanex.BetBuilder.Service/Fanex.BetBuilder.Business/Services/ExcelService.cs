namespace Fanex.BetBuilder.Business.Services;

using Fanex.BetBuilder.Business.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

public interface ExcelService
{
    string Execute(List<MonthlyDataModel> data);
}

public class ExcelServiceImp : ExcelService
{
    private readonly DateTimeService dateTimeService;

    public ExcelServiceImp(DateTimeService dateTimeService)
    {
        this.dateTimeService = dateTimeService;
    }

    public string Execute(List<MonthlyDataModel> data)
    {
        using var excelPackage = new ExcelPackage(new FileInfo("Templates/MonthlyBetBuilderTemplate.xlsx"));
        using var monthlySheet = excelPackage.Workbook.Worksheets["Template"];

        monthlySheet.Name = $"{dateTimeService.FromDate():MM-yyyy}";
        RenderTitle(monthlySheet);
        RenderData(monthlySheet, data);
        var path = SaveFile(excelPackage);

        return path;
    }

    private void RenderTitle(ExcelWorksheet monthlySheet)
    {
        var title = $"{dateTimeService.FromDate():MM-yyyy} - GMT - 4";
        monthlySheet.Cells["B2"].Value = title;
    }

    private void RenderData(ExcelWorksheet monthlySheet, List<MonthlyDataModel> data)
    {
        const int RowIndex = 4;
        const int ColumnIndex = 1;
        const int MinWidth = 12;
        const int MaxWidth = 200;
        var rowsCount = data.Count;

        monthlySheet.Cells[RowIndex, ColumnIndex].LoadFromCollection(data, false);

        var lastColumn = monthlySheet.Dimension.End.Column;
        for (var column = 1; column <= lastColumn; column++)
        {
            monthlySheet.Column(column).AutoFit(MinWidth, MaxWidth);
            monthlySheet.Cells[RowIndex, column, rowsCount + RowIndex - 1, column].StyleID = monthlySheet.Cells[RowIndex, column].StyleID;
        }

        for (var row = RowIndex; row < rowsCount + RowIndex; row++)
        {
            monthlySheet.Row(row).Height = monthlySheet.Row(RowIndex).Height;
        }

        for (var column = 9; column <= lastColumn; column++)
        {
            for (var row = RowIndex; row < rowsCount + RowIndex; row++)
            {
                if (monthlySheet.Cells[row, column].Value == null)
                {
                    monthlySheet.Cells[row, column].Value = "-";
                    monthlySheet.Cells[row, column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                }
            }
        }
    }

    private string SaveFile(ExcelPackage excelPackage)
    {
        var fromDate = dateTimeService.FromDate();
        var toDate = dateTimeService.ToDate();
        var filename = $"Bet_Builder_Monthly_Cross_Check_Report_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.xlsx";

        var path = $@"{AppConfigService.Config.Path}/{fromDate:yyyy}";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = $@"{path}/{filename}";
        excelPackage.SaveAs(new FileInfo(path), AppConfigService.Config.Password);

        return path;
    }
}