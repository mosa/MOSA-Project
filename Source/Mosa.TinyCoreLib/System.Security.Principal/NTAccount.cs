using System.Diagnostics.CodeAnalysis;

namespace System.Security.Principal;

public sealed class NTAccount : IdentityReference
{
	public override string Value
	{
		get
		{
			throw null;
		}
	}

	public NTAccount(string name)
	{
	}

	public NTAccount(string domainName, string accountName)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override bool IsValidTargetType(Type targetType)
	{
		throw null;
	}

	public static bool operator ==(NTAccount? left, NTAccount? right)
	{
		throw null;
	}

	public static bool operator !=(NTAccount? left, NTAccount? right)
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
