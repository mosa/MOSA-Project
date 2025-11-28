namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false)]
public sealed class XmlSerializerAssemblyAttribute : Attribute
{
	public string? AssemblyName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? CodeBase
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlSerializerAssemblyAttribute()
	{
	}

	public XmlSerializerAssemblyAttribute(string? assemblyName)
	{
	}

	public XmlSerializerAssemblyAttribute(string? assemblyName, string? codeBase)
	{
	}
}
