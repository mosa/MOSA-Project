using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;

namespace System.Net;

public sealed class HttpListener : IDisposable
{
	public delegate ExtendedProtectionPolicy ExtendedProtectionSelector(HttpListenerRequest request);

	public AuthenticationSchemes AuthenticationSchemes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AuthenticationSchemeSelector? AuthenticationSchemeSelectorDelegate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ServiceNameCollection DefaultServiceNames
	{
		get
		{
			throw null;
		}
	}

	public ExtendedProtectionPolicy ExtendedProtectionPolicy
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ExtendedProtectionSelector? ExtendedProtectionSelectorDelegate
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public bool IgnoreWriteExceptions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsListening
	{
		get
		{
			throw null;
		}
	}

	public static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	public HttpListenerPrefixCollection Prefixes
	{
		get
		{
			throw null;
		}
	}

	public string? Realm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public HttpListenerTimeoutManager TimeoutManager
	{
		get
		{
			throw null;
		}
	}

	public bool UnsafeConnectionNtlmAuthentication
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public void Abort()
	{
	}

	public IAsyncResult BeginGetContext(AsyncCallback? callback, object? state)
	{
		throw null;
	}

	public void Close()
	{
	}

	public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
	{
		throw null;
	}

	public HttpListenerContext GetContext()
	{
		throw null;
	}

	public Task<HttpListenerContext> GetContextAsync()
	{
		throw null;
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	void IDisposable.Dispose()
	{
	}
}
