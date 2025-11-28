using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
public class SoapTypeAttribute : Attribute
{
	public bool IncludeInSchema
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

	public string TypeName
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

	public SoapTypeAttribute()
	{
	}

	public SoapTypeAttribute(string? typeName)
	{
	}

	public SoapTypeAttribute(string? typeName, string? ns)
	{
	}
}
