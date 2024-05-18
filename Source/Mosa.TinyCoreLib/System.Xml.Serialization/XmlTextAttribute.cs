using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class XmlTextAttribute : Attribute
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

	public XmlTextAttribute()
	{
	}

	public XmlTextAttribute(Type? type)
	{
	}
}
