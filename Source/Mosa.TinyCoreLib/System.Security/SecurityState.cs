namespace System.Security;

public abstract class SecurityState
{
	public abstract void EnsureState();

	public bool IsStateAvailable()
	{
		throw null;
	}
}
