using System.Collections.Generic;
using System.IO;

namespace System.Security.Claims;

public class Claim
{
	protected virtual byte[]? CustomSerializationData
	{
		get
		{
			throw null;
		}
	}

	public string Issuer
	{
		get
		{
			throw null;
		}
	}

	public string OriginalIssuer
	{
		get
		{
			throw null;
		}
	}

	public IDictionary<string, string> Properties
	{
		get
		{
			throw null;
		}
	}

	public ClaimsIdentity? Subject
	{
		get
		{
			throw null;
		}
	}

	public string Type
	{
		get
		{
			throw null;
		}
	}

	public string Value
	{
		get
		{
			throw null;
		}
	}

	public string ValueType
	{
		get
		{
			throw null;
		}
	}

	public Claim(BinaryReader reader)
	{
	}

	public Claim(BinaryReader reader, ClaimsIdentity? subject)
	{
	}

	protected Claim(Claim other)
	{
	}

	protected Claim(Claim other, ClaimsIdentity? subject)
	{
	}

	public Claim(string type, string value)
	{
	}

	public Claim(string type, string value, string? valueType)
	{
	}

	public Claim(string type, string value, string? valueType, string? issuer)
	{
	}

	public Claim(string type, string value, string? valueType, string? issuer, string? originalIssuer)
	{
	}

	public Claim(string type, string value, string? valueType, string? issuer, string? originalIssuer, ClaimsIdentity? subject)
	{
	}

	public virtual Claim Clone()
	{
		throw null;
	}

	public virtual Claim Clone(ClaimsIdentity? identity)
	{
		throw null;
	}

	public override string ToString()
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
