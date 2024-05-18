using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace System.Security.Claims;

public class ClaimsIdentity : IIdentity
{
	public const string DefaultIssuer = "LOCAL AUTHORITY";

	public const string DefaultNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

	public const string DefaultRoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

	public ClaimsIdentity? Actor
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string? AuthenticationType
	{
		get
		{
			throw null;
		}
	}

	public object? BootstrapContext
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual IEnumerable<Claim> Claims
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

	public virtual bool IsAuthenticated
	{
		get
		{
			throw null;
		}
	}

	public string? Label
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string? Name
	{
		get
		{
			throw null;
		}
	}

	public string NameClaimType
	{
		get
		{
			throw null;
		}
	}

	public string RoleClaimType
	{
		get
		{
			throw null;
		}
	}

	public ClaimsIdentity()
	{
	}

	public ClaimsIdentity(IEnumerable<Claim>? claims)
	{
	}

	public ClaimsIdentity(IEnumerable<Claim>? claims, string? authenticationType)
	{
	}

	public ClaimsIdentity(IEnumerable<Claim>? claims, string? authenticationType, string? nameType, string? roleType)
	{
	}

	public ClaimsIdentity(BinaryReader reader)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected ClaimsIdentity(SerializationInfo info)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected ClaimsIdentity(SerializationInfo info, StreamingContext context)
	{
	}

	protected ClaimsIdentity(ClaimsIdentity other)
	{
	}

	public ClaimsIdentity(IIdentity? identity)
	{
	}

	public ClaimsIdentity(IIdentity? identity, IEnumerable<Claim>? claims)
	{
	}

	public ClaimsIdentity(IIdentity? identity, IEnumerable<Claim>? claims, string? authenticationType, string? nameType, string? roleType)
	{
	}

	public ClaimsIdentity(string? authenticationType)
	{
	}

	public ClaimsIdentity(string? authenticationType, string? nameType, string? roleType)
	{
	}

	public virtual void AddClaim(Claim claim)
	{
	}

	public virtual void AddClaims(IEnumerable<Claim?> claims)
	{
	}

	public virtual ClaimsIdentity Clone()
	{
		throw null;
	}

	protected virtual Claim CreateClaim(BinaryReader reader)
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

	public virtual void RemoveClaim(Claim? claim)
	{
	}

	public virtual bool TryRemoveClaim(Claim? claim)
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
