using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class XmlAttributeAttribute : Attribute
{
	public string AttributeName
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

	public string DataType
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

	public XmlSchemaForm Form
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

	public XmlAttributeAttribute()
	{
	}

	public XmlAttributeAttribute(string? attributeName)
	{
	}

	public XmlAttributeAttribute(string? attributeName, Type? type)
	{
	}

	public XmlAttributeAttribute(Type? type)
	{
	}
}
