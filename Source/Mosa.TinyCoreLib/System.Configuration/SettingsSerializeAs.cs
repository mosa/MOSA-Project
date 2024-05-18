namespace System.Configuration;

public enum SettingsSerializeAs
{
	String,
	Xml,
	[Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information. Consider using Xml instead.")]
	Binary,
	ProviderSpecific
}
