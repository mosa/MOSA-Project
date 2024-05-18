using System.Collections;

namespace System.Security.Policy;

public sealed class ApplicationTrustEnumerator : IEnumerator
{
	public ApplicationTrust Current
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

	internal ApplicationTrustEnumerator()
	{
	}

	public bool MoveNext()
	{
		throw null;
	}

	public void Reset()
	{
	}
}
