using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class SoapAttributeAttribute : Attribute
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

	public SoapAttributeAttribute()
	{
	}

	public SoapAttributeAttribute(string attributeName)
	{
	}
}
