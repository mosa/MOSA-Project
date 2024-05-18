namespace System.IO;

[Flags]
public enum WatcherChangeTypes
{
	Created = 1,
	Deleted = 2,
	Changed = 4,
	Renamed = 8,
	All = 0xF
}
