namespace System.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class SpecialSettingAttribute : Attribute
{
	public SpecialSetting SpecialSetting
	{
		get
		{
			throw null;
		}
	}

	public SpecialSettingAttribute(SpecialSetting specialSetting)
	{
	}
}
