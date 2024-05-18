using System.Collections;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates;

public sealed class X509Certificate2Enumerator : IEnumerator<X509Certificate2>, IEnumerator, IDisposable
{
	public X509Certificate2 Current
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

	internal X509Certificate2Enumerator()
	{
	}

	public bool MoveNext()
	{
		throw null;
	}

	public void Reset()
	{
	}

	bool IEnumerator.MoveNext()
	{
		throw null;
	}

	void IEnumerator.Reset()
	{
	}

	void IDisposable.Dispose()
	{
	}
}
