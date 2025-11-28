using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
public class XmlChoiceIdentifierAttribute : Attribute
{
	public string MemberName
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

	public XmlChoiceIdentifierAttribute()
	{
	}

	public XmlChoiceIdentifierAttribute(string? name)
	{
	}
}
