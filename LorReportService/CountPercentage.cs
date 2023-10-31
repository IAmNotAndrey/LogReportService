namespace LogReportService;

public struct CountPercentage
{
    public int Count { get; set; }
    public float Percentage { get; set; }

    public CountPercentage(int count, float percentage)
    {
        Count = count;
        Percentage = percentage;
    }

	public override string ToString()
	{
		return $"{nameof(Count)}: {Count}, {nameof(Percentage)}: {Percentage}";
	}
}
