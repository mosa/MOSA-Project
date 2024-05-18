using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Field)]
public class SoapEnumAttribute : Attribute
{
	public string Name
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

	public SoapEnumAttribute()
	{
	}

	public SoapEnumAttribute(string name)
	{
	}
}
