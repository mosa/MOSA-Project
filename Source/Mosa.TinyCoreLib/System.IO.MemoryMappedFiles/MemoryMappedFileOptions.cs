namespace System.IO.MemoryMappedFiles;

[Flags]
public enum MemoryMappedFileOptions
{
	None = 0,
	DelayAllocatePages = 0x4000000
}
