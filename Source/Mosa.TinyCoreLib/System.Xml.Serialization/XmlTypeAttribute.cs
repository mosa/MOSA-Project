using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
public class XmlTypeAttribute : Attribute
{
	public bool AnonymousType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IncludeInSchema
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public string TypeName
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

	public XmlTypeAttribute()
	{
	}

	public XmlTypeAttribute(string? typeName)
	{
	}
}
