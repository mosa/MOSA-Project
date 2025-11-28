namespace System.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class SettingsSerializeAsAttribute : Attribute
{
	public SettingsSerializeAs SerializeAs
	{
		get
		{
			throw null;
		}
	}

	public SettingsSerializeAsAttribute(SettingsSerializeAs serializeAs)
	{
	}
}
