using System.Collections;

namespace System.Security.AccessControl;

public sealed class AceEnumerator : IEnumerator
{
	public GenericAce Current
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

	internal AceEnumerator()
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
