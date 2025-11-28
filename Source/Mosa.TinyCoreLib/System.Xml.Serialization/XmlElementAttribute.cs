using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
public class XmlElementAttribute : Attribute
{
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

	public string ElementName
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

	public bool IsNullable
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

	public XmlElementAttribute()
	{
	}

	public XmlElementAttribute(string? elementName)
	{
	}

	public XmlElementAttribute(string? elementName, Type? type)
	{
	}

	public XmlElementAttribute(Type? type)
	{
	}
}
