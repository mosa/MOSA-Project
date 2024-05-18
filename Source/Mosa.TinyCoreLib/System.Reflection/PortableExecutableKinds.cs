namespace System.Reflection;

[Flags]
public enum PortableExecutableKinds
{
	NotAPortableExecutableImage = 0,
	ILOnly = 1,
	Required32Bit = 2,
	PE32Plus = 4,
	Unmanaged32Bit = 8,
	Preferred32Bit = 0x10
}
