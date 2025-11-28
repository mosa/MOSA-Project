namespace System.Security.Cryptography;

[Flags]
public enum CngPropertyOptions
{
	Persist = int.MinValue,
	None = 0,
	CustomProperty = 0x40000000
}
