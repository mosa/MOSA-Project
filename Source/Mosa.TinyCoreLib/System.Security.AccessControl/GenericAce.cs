using System.Diagnostics.CodeAnalysis;

namespace System.Security.AccessControl;

public abstract class GenericAce
{
	public AceFlags AceFlags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AceType AceType
	{
		get
		{
			throw null;
		}
	}

	public AuditFlags AuditFlags
	{
		get
		{
			throw null;
		}
	}

	public abstract int BinaryLength { get; }

	public InheritanceFlags InheritanceFlags
	{
		get
		{
			throw null;
		}
	}

	public bool IsInherited
	{
		get
		{
			throw null;
		}
	}

	public PropagationFlags PropagationFlags
	{
		get
		{
			throw null;
		}
	}

	internal GenericAce()
	{
	}

	public GenericAce Copy()
	{
		throw null;
	}

	public static GenericAce CreateFromBinaryForm(byte[] binaryForm, int offset)
	{
		throw null;
	}

	public sealed override bool Equals([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	public abstract void GetBinaryForm(byte[] binaryForm, int offset);

	public sealed override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(GenericAce? left, GenericAce? right)
	{
		throw null;
	}

	public static bool operator !=(GenericAce? left, GenericAce? right)
	{
		throw null;
	}
}
