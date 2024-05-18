namespace System.Reflection.PortableExecutable;

public sealed class PEHeaderBuilder
{
	public DllCharacteristics DllCharacteristics
	{
		get
		{
			throw null;
		}
	}

	public int FileAlignment
	{
		get
		{
			throw null;
		}
	}

	public ulong ImageBase
	{
		get
		{
			throw null;
		}
	}

	public Characteristics ImageCharacteristics
	{
		get
		{
			throw null;
		}
	}

	public Machine Machine
	{
		get
		{
			throw null;
		}
	}

	public ushort MajorImageVersion
	{
		get
		{
			throw null;
		}
	}

	public byte MajorLinkerVersion
	{
		get
		{
			throw null;
		}
	}

	public ushort MajorOperatingSystemVersion
	{
		get
		{
			throw null;
		}
	}

	public ushort MajorSubsystemVersion
	{
		get
		{
			throw null;
		}
	}

	public ushort MinorImageVersion
	{
		get
		{
			throw null;
		}
	}

	public byte MinorLinkerVersion
	{
		get
		{
			throw null;
		}
	}

	public ushort MinorOperatingSystemVersion
	{
		get
		{
			throw null;
		}
	}

	public ushort MinorSubsystemVersion
	{
		get
		{
			throw null;
		}
	}

	public int SectionAlignment
	{
		get
		{
			throw null;
		}
	}

	public ulong SizeOfHeapCommit
	{
		get
		{
			throw null;
		}
	}

	public ulong SizeOfHeapReserve
	{
		get
		{
			throw null;
		}
	}

	public ulong SizeOfStackCommit
	{
		get
		{
			throw null;
		}
	}

	public ulong SizeOfStackReserve
	{
		get
		{
			throw null;
		}
	}

	public Subsystem Subsystem
	{
		get
		{
			throw null;
		}
	}

	public PEHeaderBuilder(Machine machine = Machine.Unknown, int sectionAlignment = 8192, int fileAlignment = 512, ulong imageBase = 4194304uL, byte majorLinkerVersion = 48, byte minorLinkerVersion = 0, ushort majorOperatingSystemVersion = 4, ushort minorOperatingSystemVersion = 0, ushort majorImageVersion = 0, ushort minorImageVersion = 0, ushort majorSubsystemVersion = 4, ushort minorSubsystemVersion = 0, Subsystem subsystem = Subsystem.WindowsCui, DllCharacteristics dllCharacteristics = DllCharacteristics.DynamicBase | DllCharacteristics.NxCompatible | DllCharacteristics.NoSeh | DllCharacteristics.TerminalServerAware, Characteristics imageCharacteristics = Characteristics.Dll, ulong sizeOfStackReserve = 1048576uL, ulong sizeOfStackCommit = 4096uL, ulong sizeOfHeapReserve = 1048576uL, ulong sizeOfHeapCommit = 4096uL)
	{
	}

	public static PEHeaderBuilder CreateExecutableHeader()
	{
		throw null;
	}

	public static PEHeaderBuilder CreateLibraryHeader()
	{
		throw null;
	}
}
