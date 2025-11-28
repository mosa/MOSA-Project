namespace System.Reflection.Metadata;

public readonly struct AssemblyReference
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

	public BlobHandle HashValue
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

	public BlobHandle PublicKeyOrToken
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
}
