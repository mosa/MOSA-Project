using System.Diagnostics.CodeAnalysis;

namespace System.Net;

[Obsolete("GlobalProxySelection has been deprecated. Use WebRequest.DefaultWebProxy instead to access and set the global default proxy. Use 'null' instead of GetEmptyWebProxy.")]
public class GlobalProxySelection
{
	public static IWebProxy Select
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

	public static IWebProxy GetEmptyWebProxy()
	{
		throw null;
	}
}
