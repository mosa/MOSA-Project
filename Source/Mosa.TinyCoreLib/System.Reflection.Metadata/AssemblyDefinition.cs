namespace System.Reflection.Metadata;

public readonly struct AssemblyDefinition
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public StringHandle Culture
	{
		get
		{
			throw null;
		}
	}

	public AssemblyFlags Flags
	{
		get
		{
			throw null;
		}
	}

	public AssemblyHashAlgorithm HashAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public StringHandle Name
	{
		get
		{
			throw null;
		}
	}

	public BlobHandle PublicKey
	{
		get
		{
			throw null;
		}
	}

	public Version Version
	{
		get
		{
			throw null;
		}
	}

	public AssemblyName GetAssemblyName()
	{
		throw null;
	}

	public CustomAttributeHandleCollection GetCustomAttributes()
	{
		throw null;
	}

	public DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes()
	{
		throw null;
	}
}
