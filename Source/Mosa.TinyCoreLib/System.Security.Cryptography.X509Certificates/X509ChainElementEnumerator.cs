using System.Collections;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates;

public sealed class X509ChainElementEnumerator : IEnumerator<X509ChainElement>, IEnumerator, IDisposable
{
	public X509ChainElement Current
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

	internal X509ChainElementEnumerator()
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
