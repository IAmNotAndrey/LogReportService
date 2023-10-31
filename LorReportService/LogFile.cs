using System.Text.RegularExpressions;

namespace LogReportService;

public record LogFile
{
	public string Name { get; }
	public LogEntry[] LogEntries { get; }
	public bool HasInvalidLogEntries { get; } = false;

	public string ServiceName => GetServiceName(Name);
	public int RotationNumber => GetRotationNumber(Name);

	/// <exception cref="FormatException"></exception>
	/// <exception cref="ArgumentNullException"></exception>
	public LogFile(string fileName, LogEntry[] logEntries, bool hasInvalidLogEntries=false)
	{
		if (!IsLogFileNameCorrect(fileName))
			throw new FormatException();

		Name = fileName;
		LogEntries = logEntries;
		HasInvalidLogEntries = hasInvalidLogEntries;
	}

	/// <exception cref="FileNotFoundException"></exception>
	/// <exception cref="FormatException"></exception>
	public static LogFile Exclude(string logFileFullPath)
	{
		string fileName = Path.GetFileName(logFileFullPath);

		if (!IsLogFileNameCorrect(fileName))
			throw new FormatException();

		string[] strings = File.ReadAllLines(logFileFullPath);
		List<LogEntry> logEntries = new();
		bool hasInvalidLogEntries = false;

		foreach (string s in strings)
		{
			try
			{
				logEntries.Add(new LogEntry(s));
			}
			catch (FormatException)
			{
				hasInvalidLogEntries = true;
			}
		}
		return new LogFile(fileName, logEntries.ToArray(), hasInvalidLogEntries);
	}

	public static bool IsLogFileNameCorrect(string fileName)
	{
		const string Pattern = @"^\w+(\.\d+)?.log$";
		Match match = Regex.Match(fileName, Pattern);
		return match.Success;
	}

	/// <exception cref="FormatException"></exception>
	public static string GetServiceName(string fileName)
	{
		if (!IsLogFileNameCorrect(fileName))
			throw new FormatException();

		int firstDotIndex = fileName.IndexOf('.');
		string serviceName = firstDotIndex != -1 ? fileName.Substring(0, firstDotIndex) : fileName;

		return serviceName;
	}

	/// <exception cref="FormatException"></exception>
	public static int GetRotationNumber(string fileName)
	{
		if (!IsLogFileNameCorrect(fileName))
			throw new FormatException();

		const string Pattern = @"\.(\d+)?\.log$";
		Match match = Regex.Match(fileName, Pattern);

		if (match.Success 
				&& int.TryParse(match.Groups[1].Value, out int rotationNumber))
			return rotationNumber;
		return 0;
	}
}
