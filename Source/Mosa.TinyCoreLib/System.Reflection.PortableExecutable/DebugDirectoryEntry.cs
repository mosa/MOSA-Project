namespace System.Reflection.PortableExecutable;

public readonly struct DebugDirectoryEntry
{
	private readonly int _dummyPrimitive;

	public int DataPointer
	{
		get
		{
			throw null;
		}
	}

	public int DataRelativeVirtualAddress
	{
		get
		{
			throw null;
		}
	}

	public int DataSize
	{
		get
		{
			throw null;
		}
	}

	public bool IsPortableCodeView
	{
		get
		{
			throw null;
		}
	}

	public ushort MajorVersion
	{
		get
		{
			throw null;
		}
	}

	public ushort MinorVersion
	{
		get
		{
			throw null;
		}
	}

	public uint Stamp
	{
		get
		{
			throw null;
		}
	}

	public DebugDirectoryEntryType Type
	{
		get
		{
			throw null;
		}
	}

	public DebugDirectoryEntry(uint stamp, ushort majorVersion, ushort minorVersion, DebugDirectoryEntryType type, int dataSize, int dataRelativeVirtualAddress, int dataPointer)
	{
		throw null;
	}
}
