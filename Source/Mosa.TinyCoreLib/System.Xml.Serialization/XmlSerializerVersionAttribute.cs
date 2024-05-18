namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class XmlSerializerVersionAttribute : Attribute
{
	public string? Namespace
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? ParentAssemblyId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Type? Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Version
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlSerializerVersionAttribute()
	{
	}

	public XmlSerializerVersionAttribute(Type? type)
	{
	}
}
