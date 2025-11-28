using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace System.Net;

public class NetworkCredential : ICredentials, ICredentialsByHost
{
	public string Domain
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public string Password
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	[CLSCompliant(false)]
	public SecureString SecurePassword
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public string UserName
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public NetworkCredential()
	{
	}

	[CLSCompliant(false)]
	public NetworkCredential(string? userName, SecureString? password)
	{
	}

	[CLSCompliant(false)]
	public NetworkCredential(string? userName, SecureString? password, string? domain)
	{
	}

	public NetworkCredential(string? userName, string? password)
	{
	}

	public NetworkCredential(string? userName, string? password, string? domain)
	{
	}

	public NetworkCredential GetCredential(string? host, int port, string? authenticationType)
	{
		throw null;
	}

	public NetworkCredential GetCredential(Uri? uri, string? authenticationType)
	{
		throw null;
	}
}
