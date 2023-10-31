using System.Text.RegularExpressions;

namespace LogReportService;

public class LogReportGetter
{
	public static async Task<LogReport[]> GetAsync(string directoryPath, string serviceNamePattern)
	{
		return await Task.Run(() =>
		{
			return Get(directoryPath, serviceNamePattern);
		});
	}

	public static LogReport[] Get(string directoryPath, string serviceNamePattern)
	{
		var groupedLogFiles = Directory.EnumerateFiles(directoryPath, "*.log")
			.Where(x => LogFile.IsLogFileNameCorrect(Path.GetFileName(x)))
			.Select(x => LogFile.Exclude(x))
			.Where(x => Regex.IsMatch(x.ServiceName, serviceNamePattern))
			.GroupBy(x => x.ServiceName);

		List<LogReport> reports = new();
		foreach (var group in groupedLogFiles)
		{
			LogReport report = new(group);
			reports.Add(report);
		}

		return reports.ToArray();
	}
}
