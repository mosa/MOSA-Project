namespace System.Security.Principal;

public abstract class IdentityReference
{
	public abstract string Value { get; }

	internal IdentityReference()
	{
	}

	public abstract override bool Equals(object? o);

	public abstract override int GetHashCode();

	public abstract bool IsValidTargetType(Type targetType);

	public static bool operator ==(IdentityReference? left, IdentityReference? right)
	{
		throw null;
	}

	public static bool operator !=(IdentityReference? left, IdentityReference? right)
	{
		throw null;
	}

	public abstract override string ToString();

	public abstract IdentityReference Translate(Type targetType);
}
