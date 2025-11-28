using System.Collections.Generic;
using System.Security.Claims;

namespace System.Security.Principal;

public class GenericIdentity : ClaimsIdentity
{
	public override string AuthenticationType
	{
		get
		{
			throw null;
		}
	}

	public override IEnumerable<Claim> Claims
	{
		get
		{
			throw null;
		}
	}

	public override bool IsAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public override string Name
	{
		get
		{
			throw null;
		}
	}

	protected GenericIdentity(GenericIdentity identity)
	{
	}

	public GenericIdentity(string name)
	{
	}

	public GenericIdentity(string name, string type)
	{
	}

	public override ClaimsIdentity Clone()
	{
		throw null;
	}
}
