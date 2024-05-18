using System.Diagnostics.CodeAnalysis;

namespace System.Security.Principal;

public sealed class SecurityIdentifier : IdentityReference, IComparable<SecurityIdentifier>
{
	public static readonly int MaxBinaryLength;

	public static readonly int MinBinaryLength;

	public SecurityIdentifier? AccountDomainSid
	{
		get
		{
			throw null;
		}
	}

	public int BinaryLength
	{
		get
		{
			throw null;
		}
	}

	public override string Value
	{
		get
		{
			throw null;
		}
	}

	public SecurityIdentifier(byte[] binaryForm, int offset)
	{
	}

	public SecurityIdentifier(IntPtr binaryForm)
	{
	}

	public SecurityIdentifier(WellKnownSidType sidType, SecurityIdentifier? domainSid)
	{
	}

	public SecurityIdentifier(string sddlForm)
	{
	}

	public int CompareTo(SecurityIdentifier? sid)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	public bool Equals(SecurityIdentifier sid)
	{
		throw null;
	}

	public void GetBinaryForm(byte[] binaryForm, int offset)
	{
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public bool IsAccountSid()
	{
		throw null;
	}

	public bool IsEqualDomainSid(SecurityIdentifier sid)
	{
		throw null;
	}

	public override bool IsValidTargetType(Type targetType)
	{
		throw null;
	}

	public bool IsWellKnown(WellKnownSidType type)
	{
		throw null;
	}

	public static bool operator ==(SecurityIdentifier? left, SecurityIdentifier? right)
	{
		throw null;
	}

	public static bool operator !=(SecurityIdentifier? left, SecurityIdentifier? right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public override IdentityReference Translate(Type targetType)
	{
		throw null;
	}
}
