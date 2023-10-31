namespace LogReportServiceTests;

public class LogReportGetterTests
{
	[Fact]
	public void Constructor_Succeed()
	{
		string directoryPath = "D:\\GitHub\\LogReportService\\LogReportServiceTests\\TestLogDir\\";
		string serviceNamePattern = "^Awful";

		// [01.10.2023 12:00:00.813][Error][RequestHandler] Request #7 with 9 items received from user user7@example.com
		// [01.10.2023 12:00:01.119][Warning][RequestPayloadProcessor] Request #7 contains unsupported items
		// [01.10.2023 12:00:00.813][Info][RequestHandler] Request #8 with 20 items received from user user8@example.com
		// [01.10.2023 12:00:01.119][Error][RequestPayloadProcessor] Request #8 failed due to an error
		// [01.10.2023 12:00:00.813][Info][RequestHandler] Request #9 with 14 items received from user user9@example.com
		LogReport[] reports = LogReportGetter.Get(directoryPath, serviceNamePattern);
		var report = reports[0];

		Assert.Equal("AwfulService", report.ServiceName);
		Assert.Equal(3, report.RotationCount);
		Assert.Equal(5, report.TotalLogCount);
		Assert.Equal(DateTime.Parse("01.10.2023 12:00:00.813"), report.EarliestLogEntryTimestamp);
		Assert.Equal(DateTime.Parse("01.10.2023 12:00:01.119"), report.LatestLogEntryTimestamp);

		Dictionary<string, CountPercentage> sevCounts = new()
		{
			{ "Error", new CountPercentage(2, (float)2/5) },
			{ "Info", new CountPercentage(2, (float)2/5) },
			{ "Warning", new CountPercentage(1, (float)1/5) },
		};
		Assert.Equal(sevCounts, report.SeverityCounts);

		Dictionary<string, CountPercentage> catCounts = new()
		{
			{ "RequestHandler", new CountPercentage(3, (float)3/5) },
			{ "RequestPayloadProcessor", new CountPercentage(2, (float)2/5) },
		};
		Assert.Equal(catCounts, report.CategoryCounts);
	}
}
