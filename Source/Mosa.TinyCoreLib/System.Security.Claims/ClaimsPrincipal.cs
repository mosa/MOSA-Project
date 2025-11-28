using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace System.Security.Claims;

public class ClaimsPrincipal : IPrincipal
{
	public virtual IEnumerable<Claim> Claims
	{
		get
		{
			throw null;
		}
	}

	public static Func<ClaimsPrincipal> ClaimsPrincipalSelector
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static ClaimsPrincipal? Current
	{
		get
		{
			throw null;
		}
	}

	protected virtual byte[]? CustomSerializationData
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<ClaimsIdentity> Identities
	{
		get
		{
			throw null;
		}
	}

	public virtual IIdentity? Identity
	{
		get
		{
			throw null;
		}
	}

	public static Func<IEnumerable<ClaimsIdentity>, ClaimsIdentity?> PrimaryIdentitySelector
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ClaimsPrincipal()
	{
	}

	public ClaimsPrincipal(IEnumerable<ClaimsIdentity> identities)
	{
	}

	public ClaimsPrincipal(BinaryReader reader)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected ClaimsPrincipal(SerializationInfo info, StreamingContext context)
	{
	}

	public ClaimsPrincipal(IIdentity identity)
	{
	}

	public ClaimsPrincipal(IPrincipal principal)
	{
	}

	public virtual void AddIdentities(IEnumerable<ClaimsIdentity> identities)
	{
	}

	public virtual void AddIdentity(ClaimsIdentity identity)
	{
	}

	public virtual ClaimsPrincipal Clone()
	{
		throw null;
	}

	protected virtual ClaimsIdentity CreateClaimsIdentity(BinaryReader reader)
	{
		throw null;
	}

	public virtual IEnumerable<Claim> FindAll(Predicate<Claim> match)
	{
		throw null;
	}

	public virtual IEnumerable<Claim> FindAll(string type)
	{
		throw null;
	}

	public virtual Claim? FindFirst(Predicate<Claim> match)
	{
		throw null;
	}

	public virtual Claim? FindFirst(string type)
	{
		throw null;
	}

	protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public virtual bool HasClaim(Predicate<Claim> match)
	{
		throw null;
	}

	public virtual bool HasClaim(string type, string value)
	{
		throw null;
	}

	public virtual bool IsInRole(string role)
	{
		throw null;
	}

	public virtual void WriteTo(BinaryWriter writer)
	{
	}

	protected virtual void WriteTo(BinaryWriter writer, byte[]? userData)
	{
	}
}
