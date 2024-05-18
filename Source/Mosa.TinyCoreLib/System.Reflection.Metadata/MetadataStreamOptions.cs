namespace System.Reflection.Metadata;

[Flags]
public enum MetadataStreamOptions
{
	Default = 0,
	LeaveOpen = 1,
	PrefetchMetadata = 2
}
