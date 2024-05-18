using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.InteropServices.JavaScript;

[SupportedOSPlatform("browser")]
public static class JSHost
{
	public static JSObject GlobalThis
	{
		get
		{
			throw null;
		}
	}

	public static JSObject DotnetInstance
	{
		get
		{
			throw null;
		}
	}

	public static Task<JSObject> ImportAsync(string moduleName, string moduleUrl, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}
}
