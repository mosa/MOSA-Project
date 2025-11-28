using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace System.Security.Principal;

public class GenericPrincipal : ClaimsPrincipal
{
	public override IIdentity Identity
	{
		get
		{
			throw null;
		}
	}

	public GenericPrincipal(IIdentity identity, string[]? roles)
	{
	}

	public override bool IsInRole([NotNullWhen(true)] string? role)
	{
		throw null;
	}
}
