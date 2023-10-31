using NuGet.Frameworks;

namespace LogReportServiceTests;

public class LogReportTests
{
	[Fact]
	public void Constructor_ThrowsArgumentNullExceptionForLogFiles()
	{
		List<LogFile> logFiles = new();
		Assert.Throws<ArgumentNullException>(() => { new LogReport(logFiles); });
	}

	[Fact]
	public void Constructor_Sucсeed()
	{
		List<LogFile> logFiles = new()
		{
			new LogFile("MyService.log",
				new LogEntry[]
				{
					new LogEntry("[01.10.2023 12:00:00.813][Info][RequestHandler] Request #4 with 12 items received from user user4@example.com")
				},
				false
			),
		};

		LogReport logReport = new(logFiles);

		Assert.Equal("MyService", logReport.ServiceName);
		Assert.Equal(0, logReport.RotationCount);
		Assert.Equal(1, logReport.TotalLogCount);
		Assert.Equal(DateTime.Parse("01.10.2023 12:00:00.813"), logReport.EarliestLogEntryTimestamp);
		Assert.Equal(DateTime.Parse("01.10.2023 12:00:00.813"), logReport.LatestLogEntryTimestamp);

		Dictionary<string, CountPercentage> sevCounts = new()
		{
			{ "Info", new CountPercentage(1, 1f) }
		};
		Assert.Equal(sevCounts, logReport.SeverityCounts);

		Dictionary<string, CountPercentage> catCounts = new()
		{
			{ "RequestHandler", new CountPercentage(1, 1f) }
		};
		Assert.Equal(catCounts, logReport.CategoryCounts);
	}
	[Fact]
	public void Constructor_Sucсeed2()
	{
		List<LogFile> logFiles = new()
		{
			new LogFile("MyService.log",
				new LogEntry[]
				{
					new LogEntry("[02.10.2023 12:00:00][Info][RequestHandler] Request #4 with 12 items received from user user4@example.com")
				},
				false
			),
			new LogFile("MyService.1.log",
				new LogEntry[]
				{
					new LogEntry("[01.10.2023 12:00:00][Warning][RequestHandler] Eggs Meal")
				},
				false
			)
		};

		LogReport logReport = new(logFiles);

		Assert.Equal("MyService", logReport.ServiceName);
		Assert.Equal(1, logReport.RotationCount);
		Assert.Equal(2, logReport.TotalLogCount);
		Assert.Equal(DateTime.Parse("01.10.2023 12:00:00"), logReport.EarliestLogEntryTimestamp);
		Assert.Equal(DateTime.Parse("02.10.2023 12:00:00"), logReport.LatestLogEntryTimestamp);

		Dictionary<string, CountPercentage> sevCounts = new()
		{
			{ "Info", new CountPercentage(1, 0.5f) },
			{ "Warning", new CountPercentage(1, 0.5f) },
		};
		Assert.Equal(sevCounts, logReport.SeverityCounts);

		Dictionary<string, CountPercentage> catCounts = new()
		{
			{ "RequestHandler", new CountPercentage(2, 1f) }
		};
		Assert.Equal(catCounts, logReport.CategoryCounts);
	}
	[Fact]
	public void Constructor_SucсeedWithNoLogEntries()
	{
		List<LogFile> logFiles = new()
		{
			new LogFile("MyService.log",
				Array.Empty<LogEntry>()
			),
			new LogFile("MyService.1.log",
				Array.Empty<LogEntry>()
			)
		};

		LogReport logReport = new(logFiles);

		Assert.Equal("MyService", logReport.ServiceName);
		Assert.Equal(1, logReport.RotationCount);
		Assert.Equal(0, logReport.TotalLogCount);
		Assert.Null(logReport.EarliestLogEntryTimestamp);
		Assert.Null(logReport.LatestLogEntryTimestamp);
		Assert.Equal(new Dictionary<string, CountPercentage>(), logReport.SeverityCounts);
		Assert.Equal(new Dictionary<string, CountPercentage>(), logReport.CategoryCounts);
	}
}
