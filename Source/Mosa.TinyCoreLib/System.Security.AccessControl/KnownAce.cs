using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class KnownAce : GenericAce
{
	public int AccessMask
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SecurityIdentifier SecurityIdentifier
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal KnownAce()
	{
	}
}
