using LogReportService;

string directoryPath;
string serviceNamePattern;
LogReport[] logReports;

while (true)
{
	Console.WriteLine("Input diriectoryPath: ");
	directoryPath = Console.ReadLine();

	Console.WriteLine("\nInput serviceNamePattern (accepts Regex): ");
	serviceNamePattern = Console.ReadLine();
	try
	{
		logReports = await LogReportGetter.GetAsync(directoryPath, serviceNamePattern);
		break;
	}
	catch (Exception e)
	{
		Console.WriteLine("[!] " + e.Message + "\n");
    }
}

foreach (LogReport logReport in logReports)
{
	Console.WriteLine(logReport + "\n");
	foreach (var item in logReport.SeverityCounts)
	{
        Console.WriteLine(item);
    }
	foreach (var item in logReport.CategoryCounts)
	{
		Console.WriteLine(item);
	}
	Console.WriteLine("-------------------------------\n");
}
