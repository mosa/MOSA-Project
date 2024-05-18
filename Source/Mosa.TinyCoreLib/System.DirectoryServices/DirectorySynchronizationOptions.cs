namespace System.DirectoryServices;

[Flags]
public enum DirectorySynchronizationOptions : long
{
	None = 0L,
	ObjectSecurity = 1L,
	ParentsFirst = 0x800L,
	PublicDataOnly = 0x2000L,
	IncrementalValues = 0x80000000L
}
