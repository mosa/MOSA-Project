namespace System.Text.Json.Serialization.Metadata;

public readonly struct JsonDerivedType
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public Type DerivedType
	{
		get
		{
			throw null;
		}
	}

	public object? TypeDiscriminator
	{
		get
		{
			throw null;
		}
	}

	public JsonDerivedType(Type derivedType)
	{
		throw null;
	}

	public JsonDerivedType(Type derivedType, int typeDiscriminator)
	{
		throw null;
	}

	public JsonDerivedType(Type derivedType, string typeDiscriminator)
	{
		throw null;
	}
}
