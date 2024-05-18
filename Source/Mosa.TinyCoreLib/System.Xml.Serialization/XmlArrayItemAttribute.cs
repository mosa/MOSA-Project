using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
public class XmlArrayItemAttribute : Attribute
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

	public int NestingLevel
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

	public XmlArrayItemAttribute()
	{
	}

	public XmlArrayItemAttribute(string? elementName)
	{
	}

	public XmlArrayItemAttribute(string? elementName, Type? type)
	{
	}

	public XmlArrayItemAttribute(Type? type)
	{
	}
}
