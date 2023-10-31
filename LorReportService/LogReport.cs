namespace LogReportService;

public record LogReport
{
	public string ServiceName { get; }
	public int RotationCount { get; }
	public int TotalLogCount { get; }
	public DateTime? EarliestLogEntryTimestamp { get; }
	public DateTime? LatestLogEntryTimestamp { get; }
	public Dictionary<string, CountPercentage> SeverityCounts { get; }
	public Dictionary<string, CountPercentage> CategoryCounts { get; }

    public LogReport(string serviceName, int rotationNum, IEnumerable<LogEntry> logEntries)
    {
		ServiceName = serviceName;
		RotationCount = rotationNum;
		TotalLogCount = logEntries.Count();

		var otherParams = CalculateOtherParams(logEntries);
		EarliestLogEntryTimestamp = otherParams.EarliestLogEntryTimestamp;
		LatestLogEntryTimestamp = otherParams.LatestLogEntryTimestamp;

		SeverityCounts = CalculateCountPercentages(otherParams.SeverityIntCounts);
		CategoryCounts = CalculateCountPercentages(otherParams.CategoryIntCounts);
	}

	/// <exception cref="ArgumentNullException"></exception>
    public LogReport(IEnumerable<LogFile> logFiles)
	{
		if (!logFiles.Any())
			throw new ArgumentNullException(nameof(logFiles));

		List<LogEntry> concatLogEntries = logFiles.SelectMany(logFile => logFile.LogEntries).ToList();

		ServiceName = logFiles.First().ServiceName;
		RotationCount = logFiles.Max(logFile => logFile.RotationNumber);
		TotalLogCount = concatLogEntries.Count;

		var otherParams = CalculateOtherParams(concatLogEntries);
		EarliestLogEntryTimestamp = otherParams.EarliestLogEntryTimestamp;
		LatestLogEntryTimestamp = otherParams.LatestLogEntryTimestamp;

		SeverityCounts = CalculateCountPercentages(otherParams.SeverityIntCounts);
		CategoryCounts = CalculateCountPercentages(otherParams.CategoryIntCounts);
	}

	private (
			DateTime? EarliestLogEntryTimestamp,
			DateTime? LatestLogEntryTimestamp,
			Dictionary<string, int> SeverityIntCounts,
			Dictionary<string, int> CategoryIntCounts)
	CalculateOtherParams(IEnumerable<LogEntry> logEntries)
	{
		Dictionary<string, int> severityIntCounts = new();
		Dictionary<string, int> categoryIntCounts = new();
		DateTime? earliestLogEntryTimestamp = null;
		DateTime? latestLogEntryTimestamp = null;

		foreach (LogEntry entry in logEntries)
		{
			earliestLogEntryTimestamp = new DateTime[] { earliestLogEntryTimestamp ?? DateTime.MaxValue, entry.Timestamp }
				.Min();

			latestLogEntryTimestamp = new DateTime[] { latestLogEntryTimestamp ?? DateTime.MinValue, entry.Timestamp }
				.Max();

			if (severityIntCounts.ContainsKey(entry.Severity))
				severityIntCounts[entry.Severity]++;
			else
				severityIntCounts[entry.Severity] = 1;

			if (categoryIntCounts.ContainsKey(entry.Category))
				categoryIntCounts[entry.Category]++;
			else
				categoryIntCounts[entry.Category] = 1;
		}
		return (
			EarliestLogEntryTimestamp: earliestLogEntryTimestamp,
			LatestLogEntryTimestamp: latestLogEntryTimestamp,
			SeverityIntCounts: severityIntCounts,
			CategoryIntCounts: categoryIntCounts
		);
	}

	private Dictionary<string, CountPercentage> CalculateCountPercentages(Dictionary<string, int> counts)
	{
		return counts.ToDictionary(
			item => item.Key,
			item => new CountPercentage(item.Value, (float)item.Value / TotalLogCount)
		);
	}
}
