using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.Net.Mail;

public class MailAddress
{
	public string Address
	{
		get
		{
			throw null;
		}
	}

	public string DisplayName
	{
		get
		{
			throw null;
		}
	}

	public string Host
	{
		get
		{
			throw null;
		}
	}

	public string User
	{
		get
		{
			throw null;
		}
	}

	public MailAddress(string address)
	{
	}

	public MailAddress(string address, string? displayName)
	{
	}

	public MailAddress(string address, string? displayName, Encoding? displayNameEncoding)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static bool TryCreate([NotNullWhen(true)] string? address, [NotNullWhen(true)] out MailAddress? result)
	{
		throw null;
	}

	public static bool TryCreate([NotNullWhen(true)] string? address, string? displayName, [NotNullWhen(true)] out MailAddress? result)
	{
		throw null;
	}

	public static bool TryCreate([NotNullWhen(true)] string? address, string? displayName, Encoding? displayNameEncoding, [NotNullWhen(true)] out MailAddress? result)
	{
		throw null;
	}
}
