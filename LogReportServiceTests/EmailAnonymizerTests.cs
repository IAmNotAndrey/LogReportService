namespace LogReportServiceTests;

public class EmailAnonymizerTests
{
	[Fact]
	public void Anonymize_Succeed()
	{
		string mail = "email@mail.ru";
		string res = mail.Anonymize();

		string expected = "e*a*l@mail.ru";

		Assert.Equal(expected, res);
	}
	[Fact]
	public void Anonymize_Succeed2()
	{
		string mail = "emai@mail.ru";
		string res = mail.Anonymize();

		string expected = "e*a*@mail.ru";

		Assert.Equal(expected, res);
	}
	[Fact]
	public void Anonymize_Succeed3()
	{
		string mail = "e@mail.ru";
		string res = mail.Anonymize();

		string expected = "e@mail.ru";

		Assert.Equal(expected, res);
	}

	[Fact]
	public void Anonymize_ThrowsArgumentException()
	{
		string mail = "@mail.ru";

		Assert.Throws<ArgumentException>(() => { mail.Anonymize(); });
	}
	[Fact]
	public void Anonymize_ThrowsArgumentException2()
	{
		string mail = "emailmail.ru";

		Assert.Throws<ArgumentException>(() => { mail.Anonymize(); });
	}

	[Fact]
	public void AnonymizeInText_Succeed()
	{
		string text = "Loram  mrokd local@mail.ru su";
		string res = text.AnonymizeInText();

		string expected = "Loram  mrokd l*c*l@mail.ru su";

		Assert.Equal(expected, res);
	}
	[Fact]
	public void AnonymizeInText_Succeed2()
	{
		string text = "@ru Loram  mrokd local@mail.ru su";
		string res = text.AnonymizeInText();

		string expected = "@ru Loram  mrokd l*c*l@mail.ru su";

		Assert.Equal(expected, res);
	}
	[Fact]
	public void AnonymizeInText_Succeed3()
	{
		string text = "Loram p@yandex.ru mrokd local@mail.ru su";
		string res = text.AnonymizeInText();

		string expected = "Loram p@yandex.ru mrokd l*c*l@mail.ru su";

		Assert.Equal(expected, res);
	}
}
