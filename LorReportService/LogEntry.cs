using System.Text.RegularExpressions;

namespace LogReportService;

public record LogEntry
{
	public DateTime Timestamp { get; }
	public string Severity { get; }
	public string Category { get; }

	private string _message;
	public string Message 
	{
		get { return _message; }
		init { _message = value.AnonymizeInText(); }
	}


	public LogEntry(
		DateTime timestamp,
		string severity,
		string category,
		string message
	)
    {
		Timestamp = timestamp;
		Severity = severity;
		Category = category;
		Message = message;
    }

    public LogEntry(string logLine)
	{
		const string Pattern = @"^\[(.+)\]\[\s*(\w+)\s*\]\[\s*(\w+)\s*\]\s*(.+)$";

		Match match = Regex.Match(logLine, Pattern);

		if (!match.Success)
			throw new FormatException($"`{nameof(logLine)}` doesn't match `{nameof(Pattern)}`");

		Timestamp = DateTime.Parse(match.Groups[1].Value);
		Severity = match.Groups[2].Value;
		Category = match.Groups[3].Value;
		Message = string.IsNullOrWhiteSpace(match.Groups[4].Value)
			? throw new FormatException()
			: match.Groups[4].Value;
	}
}
