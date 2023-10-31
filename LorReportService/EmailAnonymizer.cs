using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace LogReportService;

public static class EmailAnonymizer
{
    public static string Anonymize(this string email)
	{
		if (!new EmailAddressAttribute().IsValid(email))
		{
			throw new ArgumentException("Wrong email-address format", nameof(email));
		}
		// Split email to local part and domen
		string[] parts = email.Split('@');

		string localPart = parts[0];
		string domain = parts[1];

		// Create anonymized local part
		StringBuilder anonymizedLocalPart = new();
		for (int i = 0; i < localPart.Length; i++)
		{
			if (i % 2 == 0 || localPart[i] == '.')
				anonymizedLocalPart.Append(localPart[i]);
			else
				anonymizedLocalPart.Append('*');
		}
		// Get anonymized email
		string anonymizedEmail = anonymizedLocalPart + "@" + domain;

		return anonymizedEmail;
	}

	public static string AnonymizeInText(this string text)
	{
		// Regular expression pattern to find email addresses in the text
		string pattern = @"[\w\.-]+@[\w\.-]+";

		// Create a regular expression for finding email addresses
		Regex regex = new(pattern);

		// Find all email addresses in the text
		MatchCollection matches = regex.Matches(text);

		// Iterate through each found email address and anonymize it
		foreach (Match match in matches)
		{
			string email = match.Value;
			string anonymizedEmail = email.Anonymize();

			// Replace the original email with the anonymized version in the text
			text = text.Replace(email, anonymizedEmail);
		}

		return text;
	}
}
