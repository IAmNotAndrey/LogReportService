namespace LogReportServiceTests;

public class LogEntryTests
{
	[Fact]
	public void Constructor_ParsesLogLineCorrectly()
	{
		string logLine = "[01.10.2023 12:00:00.813][Info][RequestHandler] Eggs Chicken";

		LogEntry logEntry = new(logLine);

		Assert.Equal(new DateTime(2023, 10, 1, 12, 0, 0, 813), logEntry.Timestamp);
		Assert.Equal("Info", logEntry.Severity);
		Assert.Equal("RequestHandler", logEntry.Category);
		Assert.Equal("Eggs Chicken", logEntry.Message);
	}
	[Fact]
	public void Constructor_ParsesLogLineCorrectly2()
	{
		string logLine = "[  01.10.2023   12:00:00 ][Info][RequestHandler] Eggs Chicken";

		LogEntry logEntry = new(logLine);

		Assert.Equal(new DateTime(2023, 10, 1, 12, 0, 0), logEntry.Timestamp);
		Assert.Equal("Info", logEntry.Severity);
		Assert.Equal("RequestHandler", logEntry.Category);
		Assert.Equal("Eggs Chicken", logEntry.Message);
	}
	[Fact]
	public void Constructor_ParsesLogLineCorrectly3()
	{
		string logLine = "[  01.01.2077 00:00:00][Error ][ Auslander]Eggs Chicken  ";

		LogEntry logEntry = new(logLine);

		Assert.Equal(new DateTime(2077, 1, 1, 0, 0, 0), logEntry.Timestamp);
		Assert.Equal("Error", logEntry.Severity);
		Assert.Equal("Auslander", logEntry.Category);
		Assert.Equal("Eggs Chicken  ", logEntry.Message);
	}
	[Fact]
	public void Constructor_ParsesLogLineCorrectly4()
	{
		string logLine = "[  01.01.2077 00:00:00][Error ][ Auslander]Eggs Chicken perm@mail.ru ";

		LogEntry logEntry = new(logLine);

		Assert.Equal(new DateTime(2077, 1, 1, 0, 0, 0), logEntry.Timestamp);
		Assert.Equal("Error", logEntry.Severity);
		Assert.Equal("Auslander", logEntry.Category);
		Assert.Equal("Eggs Chicken p*r*@mail.ru ", logEntry.Message);
	}

	[Fact]
	public void Constructor_ThrowsFormatExceptionForIncorrectDateTimeString()
	{
		string logLine = "[eggs][ Error ][ Auslander ] Eggs Chicken  ";

		Assert.Throws<FormatException>(() => { new LogEntry(logLine); });
	}

	[Fact]
	public void Constructor_ThrowsFormatExceptionForEmptySeverity()
	{
		string logLine = "[01.10.2023 12:00:00.813][][RequestHandler] Request #69 with 24 items received from user example@domain.com";

		Assert.Throws<FormatException>(() => { new LogEntry(logLine); });
	}
	[Fact]
	public void Constructor_ThrowsFormatExceptionForEmptySeverity2()
	{
		string logLine = "[01.10.2023 12:00:00.813][ ][RequestHandler] Request #69 with 24 items received from user example@domain.com";

		Assert.Throws<FormatException>(() => { new LogEntry(logLine); });
	}

	[Fact]
	public void Constructor_ThrowsFormatExceptionForEmptyCategory()
	{
		string logLine = "[01.10.2023 12:00:00.813][Warning][] Request #69 with 24 items received from user example@domain.com";

		Assert.Throws<FormatException>(() => { new LogEntry(logLine); });
	}
	[Fact]
	public void Constructor_ThrowsFormatExceptionForEmptyCategory2()
	{
		string logLine = "[01.10.2023 12:00:00.813][Warning][ ] Request #69 with 24 items received from user example@domain.com";

		Assert.Throws<FormatException>(() => { new LogEntry(logLine); });
	}

	[Fact]
	public void Constructor_ThrowsFormatExceptionForEmptyMessage()
	{
		string logLine = "[01.10.2023 12:00:00.813][Warning][Manager]";

		Assert.Throws<FormatException>(() => { new LogEntry(logLine); });
	}
	[Fact]
	public void Constructor_ThrowsFormatExceptionForEmptyMessage2()
	{
		string logLine = "[01.10.2023 12:00:00.813][Warning][Manager] ";

		Assert.Throws<FormatException>(() => { new LogEntry(logLine); });
	}
	[Fact]
	public void Constructor_ThrowsFormatExceptionForEmptyMessage3()
	{
		string logLine = "[01.10.2023 12:00:00.813][Warning][Manager]  ";

		Assert.Throws<FormatException>(() => { new LogEntry(logLine); });
	}
}
