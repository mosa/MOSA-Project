using System.Collections;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates;

public sealed class X509ExtensionEnumerator : IEnumerator<X509Extension>, IEnumerator, IDisposable
{
	public X509Extension Current
	{
		get
		{
			throw null;
		}
	}

	object IEnumerator.Current
	{
		get
		{
			throw null;
		}
	}

	internal X509ExtensionEnumerator()
	{
	}

	public bool MoveNext()
	{
		throw null;
	}

	public void Reset()
	{
	}

	void IDisposable.Dispose()
	{
	}
}
