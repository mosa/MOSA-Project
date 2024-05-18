namespace System.Reflection.PortableExecutable;

public enum DebugDirectoryEntryType
{
	Unknown = 0,
	Coff = 1,
	CodeView = 2,
	Reproducible = 16,
	EmbeddedPortablePdb = 17,
	PdbChecksum = 19
}
