namespace System.Security.Principal;

public interface IIdentity
{
	string? AuthenticationType { get; }

	bool IsAuthenticated { get; }

	string? Name { get; }
}
