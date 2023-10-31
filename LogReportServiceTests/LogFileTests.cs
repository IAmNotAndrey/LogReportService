using LogReportService;

namespace LogReportServiceTests;

public class LogFileTests
{
	[Fact]
	public void IsLogFileNameCorrect_TrueForValidFileName()
	{
		string fileName = "Test.log";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.True(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_TrueForValidFileName2()
	{
		string fileName = "Test.1.log";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.True(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_TrueForValidFileName3()
	{
		string fileName = "tesT.12345.log";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.True(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_FalseForInValidFileName()
	{
		string fileName = "Test.loggy";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.False(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_FalseForInValidFileName2()
	{
		string fileName = ".log";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.False(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_FalseForInValidFileName3()
	{
		string fileName = "Test..log";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.False(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_FalseForInValidFileName4()
	{
		string fileName = "test.docx";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.False(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_FalseForInValidFileName5()
	{
		string fileName = "test.1.docx";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.False(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_FalseForInValidFileName6()
	{
		string fileName = "test.1.";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.False(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_FalseForInValidFileName7()
	{
		string fileName = "test.1.1";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.False(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_FalseForInValidFileName8()
	{
		string fileName = "test.";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.False(res);
	}
	[Fact]
	public void IsLogFileNameCorrect_FalseForInValidFileName9()
	{
		string fileName = ".123";
		bool res = LogFile.IsLogFileNameCorrect(fileName);

		Assert.False(res);
	}

	[Fact]
	public void GetServiceName_Succeed()
	{
		string fileName = "MyService.log";
		string res = LogFile.GetServiceName(fileName);

		Assert.Equal("MyService", res);
	}
	[Fact]
	public void GetServiceName_Succeed2()
	{
		string fileName = "myService.log";
		string res = LogFile.GetServiceName(fileName);

		Assert.Equal("myService", res);
	}
	[Fact]
	public void GetServiceName_Succeed3()
	{
		string fileName = "MyService.1.log";
		string res = LogFile.GetServiceName(fileName);

		Assert.Equal("MyService", res);
	}
	[Fact]
	public void GetServiceName_ThrowsFormatExceptionForIncorrectFileName()
	{
		string fileName = "MyService.1.logy";
		Assert.Throws<FormatException>(() => { LogFile.GetServiceName(fileName); });
	}

	[Fact]
	public void GetRotationNumber_Succeed()
	{
		string fileName = "MyService.1.log";
		int res = LogFile.GetRotationNumber(fileName);

		Assert.Equal(1, res);
	}
	[Fact]
	public void GetRotationNumber_Succeed2()
	{
		string fileName = "MyService.log";
		int res = LogFile.GetRotationNumber(fileName);

		Assert.Equal(0, res);
	}
	[Fact]
	public void GetRotationNumber_Succeed3()
	{
		string fileName = "MyService.12345.log";
		int res = LogFile.GetRotationNumber(fileName);

		Assert.Equal(12345, res);
	}
	[Fact]
	public void GetRotationNumber_ThrowsFormatExceptionForInvalidFileName()
	{
		string fileName = "MyService.logy";
		Assert.Throws<FormatException>(() => { LogFile.GetServiceName(fileName); });
	}
	[Fact]
	public void GetRotationNumber_ThrowsFormatExceptionForInvalidFileName2()
	{
		string fileName = "MyService.1.logy";
		Assert.Throws<FormatException>(() => { LogFile.GetServiceName(fileName); });
	}

	[Fact]
	public void Exclude_Succeed()
	{
		string logFilePath = "D:\\GitHub\\LogReportService\\LogReportServiceTests\\TestLogDir\\MyService.log";

		// [01.10.2023 12:00:00.813][Info][RequestHandler] Request #4 with 12 items received from user user4@example.com
		LogFile clonLogFile = new(
			"MyService.log",
			new LogEntry[]
			{
				new LogEntry("[01.10.2023 12:00:00.813][Info][RequestHandler] Request #4 with 12 items received from user user4@example.com")
			},
			false
		);
		LogFile res = LogFile.Exclude(logFilePath);

		Assert.Equal(clonLogFile.Name, res.Name);
		Assert.Equal(clonLogFile.LogEntries, res.LogEntries);
		Assert.Equal(clonLogFile.HasInvalidLogEntries, res.HasInvalidLogEntries);
		Assert.Equal(clonLogFile.ServiceName, res.ServiceName);
		Assert.Equal(clonLogFile.RotationNumber, res.RotationNumber);
	}
	[Fact]
	public void Exclude_ThrowsFormatExceptionForInvalidFileName()
	{
		string logFilePath = "D:\\Program Files\\personal_programmes\\LorReportService\\LogReportServiceTests\\TestLogDir\\MyService..log";

		Assert.Throws<FormatException>(() => { LogFile.Exclude(logFilePath); });
	}
	[Fact]
	public void Exclude_ThrowsFileNotFoundExceptionForInvalidFilePath()
	{
		string logFilePath = "D:\\GitHub\\LogReportService\\LogReportServiceTests\\TestLogDir\\Invalid.log";

		Assert.Throws<FileNotFoundException>(() => { LogFile.Exclude(logFilePath); });
	}
}
