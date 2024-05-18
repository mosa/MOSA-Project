using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.ReturnValue)]
public class XmlRootAttribute : Attribute
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

	public XmlRootAttribute()
	{
	}

	public XmlRootAttribute(string elementName)
	{
	}
}
