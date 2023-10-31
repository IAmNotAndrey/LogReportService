namespace LogReportServiceTests;

public class CountPercentageTests
{
	[Fact]
	public void Constructor_SetsCountAndPercentage()
	{
		int count = 10;
		float percentage = 0.75f;

		CountPercentage countPercentage = new(count, percentage);

 		Assert.Equal(count, countPercentage.Count);
		Assert.Equal(percentage, countPercentage.Percentage);
	}

	[Fact]
	public void CountProperty_CanBeSet()
	{
		int count = 5;
		float percentage = 0.25f;
		CountPercentage countPercentage = new(count, percentage);

		countPercentage.Count = 20;

		Assert.Equal(20, countPercentage.Count);
	}

	[Fact]
	public void PercentageProperty_CanBeSet()
	{
		int count = 10;
		float percentage = 0.5f;
		CountPercentage countPercentage = new(count, percentage);

		countPercentage.Percentage = 0.75f;

		Assert.Equal(0.75f, countPercentage.Percentage);
	}
}
