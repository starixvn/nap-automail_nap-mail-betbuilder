namespace BetBuilder.Business.Services;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using ICSharpCode.SharpZipLib.Zip;
using Models;

public interface CsvService
{
    string ExportCsv(List<DailyDataModel> data);
}

public class CsvServiceImp : CsvService
{
    private readonly CriteriaService param;

    public CsvServiceImp(CriteriaService param)
    {
        this.param = param;
    }

    public string ExportCsv(List<DailyDataModel> data)
    {
        var csvPath = GenerateFileName();
        SaveFileCsv(data, csvPath);
        var zipPath = ZipFile(csvPath);
        Delete(csvPath);

        return zipPath;
    }

    private static void SaveFileCsv(List<DailyDataModel> data, string path)
    {
        using var writer = new StreamWriter(path);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(data);
    }

    private string GenerateFileName()
    {
        var fileName = $@"Bet_Builder_Daily_Report_{param.WinlostDate:yyyyMMdd}.csv";
        var path = $@"{BetBuilderSettings.FilePath}\{param.WinlostDate:yyyy}\{param.WinlostDate:MM}";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = $@"{path}\{fileName}";

        return path;
    }

    private static string ZipFile(string path)
    {
        var zipFile = $@"{path.Replace(".csv", string.Empty)}.zip";
        var shortName = new FileInfo(path).Name;
        var directoryInfo = new DirectoryInfo(path).Parent;
        if (directoryInfo != null)
        {
            var folderPath = directoryInfo.FullName;
            var zip = new FastZip { Password = BetBuilderSettings.Password };
            zip.CreateZip(zipFile, folderPath, false, shortName);
        }

        return zipFile;
    }

    private static void Delete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}