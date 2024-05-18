using System.Runtime.InteropServices;

namespace System.Net.Http;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct HttpRequestOptionsKey<TValue>
{
	public string Key
	{
		get
		{
			throw null;
		}
	}

	public HttpRequestOptionsKey(string key)
	{
	}
}
