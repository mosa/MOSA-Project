namespace System.Reflection.Metadata;

[Flags]
public enum SignatureAttributes : byte
{
	None = 0,
	Generic = 0x10,
	Instance = 0x20,
	ExplicitThis = 0x40
}
