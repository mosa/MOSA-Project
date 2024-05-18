using System.Net.Security;
using System.Runtime.Versioning;

namespace System.Net;

public class ServicePointManager
{
	public const int DefaultNonPersistentConnectionLimit = 4;

	public const int DefaultPersistentConnectionLimit = 2;

	public static bool CheckCertificateRevocationList
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int DefaultConnectionLimit
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int DnsRefreshTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static bool EnableDnsRoundRobin
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public static EncryptionPolicy EncryptionPolicy
	{
		get
		{
			throw null;
		}
	}

	public static bool Expect100Continue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int MaxServicePointIdleTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int MaxServicePoints
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static bool ReusePort
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static SecurityProtocolType SecurityProtocol
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static RemoteCertificateValidationCallback? ServerCertificateValidationCallback
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static bool UseNagleAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal ServicePointManager()
	{
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static ServicePoint FindServicePoint(string uriString, IWebProxy? proxy)
	{
		throw null;
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static ServicePoint FindServicePoint(Uri address)
	{
		throw null;
	}

	[Obsolete("WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete. Use HttpClient instead.", DiagnosticId = "SYSLIB0014", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static ServicePoint FindServicePoint(Uri address, IWebProxy? proxy)
	{
		throw null;
	}

	public static void SetTcpKeepAlive(bool enabled, int keepAliveTime, int keepAliveInterval)
	{
	}
}
