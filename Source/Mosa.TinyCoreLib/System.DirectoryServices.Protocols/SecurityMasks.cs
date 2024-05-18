namespace System.DirectoryServices.Protocols;

[Flags]
public enum SecurityMasks
{
	None = 0,
	Owner = 1,
	Group = 2,
	Dacl = 4,
	Sacl = 8
}
