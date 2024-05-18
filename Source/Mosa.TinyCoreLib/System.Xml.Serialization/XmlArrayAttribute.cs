using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
public class XmlArrayAttribute : Attribute
{
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

	public XmlArrayAttribute()
	{
	}

	public XmlArrayAttribute(string? elementName)
	{
	}
}
