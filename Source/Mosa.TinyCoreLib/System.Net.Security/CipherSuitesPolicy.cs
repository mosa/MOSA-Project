using System.Collections.Generic;
using System.Runtime.Versioning;

namespace System.Net.Security;

[UnsupportedOSPlatform("android")]
[UnsupportedOSPlatform("windows")]
public sealed class CipherSuitesPolicy
{
	[CLSCompliant(false)]
	public IEnumerable<TlsCipherSuite> AllowedCipherSuites
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	public CipherSuitesPolicy(IEnumerable<TlsCipherSuite> allowedCipherSuites)
	{
	}
}
