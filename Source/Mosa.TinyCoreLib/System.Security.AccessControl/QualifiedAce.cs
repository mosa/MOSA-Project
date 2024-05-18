namespace System.Security.AccessControl;

public abstract class QualifiedAce : KnownAce
{
	public AceQualifier AceQualifier
	{
		get
		{
			throw null;
		}
	}

	public bool IsCallback
	{
		get
		{
			throw null;
		}
	}

	public int OpaqueLength
	{
		get
		{
			throw null;
		}
	}

	internal QualifiedAce()
	{
	}

	public byte[]? GetOpaque()
	{
		throw null;
	}

	public void SetOpaque(byte[]? opaque)
	{
	}
}
