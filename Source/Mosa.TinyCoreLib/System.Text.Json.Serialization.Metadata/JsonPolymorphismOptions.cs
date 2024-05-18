using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json.Serialization.Metadata;

public class JsonPolymorphismOptions
{
	public IList<JsonDerivedType> DerivedTypes
	{
		get
		{
			throw null;
		}
	}

	public bool IgnoreUnrecognizedTypeDiscriminators
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string TypeDiscriminatorPropertyName
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public JsonUnknownDerivedTypeHandling UnknownDerivedTypeHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}
}
