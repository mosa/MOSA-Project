namespace System.ComponentModel.Design.Serialization;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DefaultSerializationProviderAttribute : Attribute
{
	public string ProviderTypeName
	{
		get
		{
			throw null;
		}
	}

	public DefaultSerializationProviderAttribute(string providerTypeName)
	{
	}

	public DefaultSerializationProviderAttribute(Type providerType)
	{
	}
}
