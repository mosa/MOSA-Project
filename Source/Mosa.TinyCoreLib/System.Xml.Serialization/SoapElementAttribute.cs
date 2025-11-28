using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class SoapElementAttribute : Attribute
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

	public SoapElementAttribute()
	{
	}

	public SoapElementAttribute(string? elementName)
	{
	}
}
