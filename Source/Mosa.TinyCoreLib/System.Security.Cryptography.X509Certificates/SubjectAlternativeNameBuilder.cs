using System.Net;

namespace System.Security.Cryptography.X509Certificates;

public sealed class SubjectAlternativeNameBuilder
{
	public void AddDnsName(string dnsName)
	{
	}

	public void AddEmailAddress(string emailAddress)
	{
	}

	public void AddIpAddress(IPAddress ipAddress)
	{
	}

	public void AddUri(Uri uri)
	{
	}

	public void AddUserPrincipalName(string upn)
	{
	}

	public X509Extension Build(bool critical = false)
	{
		throw null;
	}
}
