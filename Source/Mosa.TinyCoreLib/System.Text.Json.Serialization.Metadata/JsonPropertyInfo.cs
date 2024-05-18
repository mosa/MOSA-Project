using System.Reflection;

namespace System.Text.Json.Serialization.Metadata;

public abstract class JsonPropertyInfo
{
	public ICustomAttributeProvider? AttributeProvider
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonConverter? CustomConverter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Func<object, object?>? Get
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsExtensionData
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsRequired
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonNumberHandling? NumberHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonObjectCreationHandling? ObjectCreationHandling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public JsonSerializerOptions Options
	{
		get
		{
			throw null;
		}
	}

	public int Order
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Type PropertyType
	{
		get
		{
			throw null;
		}
	}

	public Action<object, object?>? Set
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Func<object, object?, bool>? ShouldSerialize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal JsonPropertyInfo()
	{
	}
}
