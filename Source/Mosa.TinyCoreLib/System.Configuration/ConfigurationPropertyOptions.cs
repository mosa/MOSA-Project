namespace System.Configuration;

[Flags]
public enum ConfigurationPropertyOptions
{
	None = 0,
	IsDefaultCollection = 1,
	IsRequired = 2,
	IsKey = 4,
	IsTypeStringTransformationRequired = 8,
	IsAssemblyStringTransformationRequired = 0x10,
	IsVersionCheckRequired = 0x20
}
