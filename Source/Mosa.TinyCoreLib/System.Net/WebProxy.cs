using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace System.Net;

public class WebProxy : IWebProxy, ISerializable
{
	public Uri? Address
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ArrayList BypassArrayList
	{
		get
		{
			throw null;
		}
	}

	public string[] BypassList
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

	public bool BypassProxyOnLocal
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ICredentials? Credentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool UseDefaultCredentials
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public WebProxy()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected WebProxy(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public WebProxy(string? Address)
	{
	}

	public WebProxy(string? Address, bool BypassOnLocal)
	{
	}

	public WebProxy(string? Address, bool BypassOnLocal, [StringSyntax("Regex", new object[] { RegexOptions.IgnoreCase | RegexOptions.CultureInvariant })] string[]? BypassList)
	{
	}

	public WebProxy(string? Address, bool BypassOnLocal, [StringSyntax("Regex", new object[] { RegexOptions.IgnoreCase | RegexOptions.CultureInvariant })] string[]? BypassList, ICredentials? Credentials)
	{
	}

	public WebProxy(string Host, int Port)
	{
	}

	public WebProxy(Uri? Address)
	{
	}

	public WebProxy(Uri? Address, bool BypassOnLocal)
	{
	}

	public WebProxy(Uri? Address, bool BypassOnLocal, [StringSyntax("Regex", new object[] { RegexOptions.IgnoreCase | RegexOptions.CultureInvariant })] string[]? BypassList)
	{
	}

	public WebProxy(Uri? Address, bool BypassOnLocal, [StringSyntax("Regex", new object[] { RegexOptions.IgnoreCase | RegexOptions.CultureInvariant })] string[]? BypassList, ICredentials? Credentials)
	{
	}

	[Obsolete("WebProxy.GetDefaultProxy has been deprecated. Use the proxy selected for you by default.")]
	public static WebProxy GetDefaultProxy()
	{
		throw null;
	}

	protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}

	public Uri? GetProxy(Uri destination)
	{
		throw null;
	}

	public bool IsBypassed(Uri host)
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
