using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
public class XmlAnyElementAttribute : Attribute
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

	public XmlAnyElementAttribute()
	{
	}

	public XmlAnyElementAttribute(string? name)
	{
	}

	public XmlAnyElementAttribute(string? name, string? ns)
	{
	}
}
