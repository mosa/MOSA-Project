namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class LibraryImportAttribute : Attribute
{
	public string LibraryName
	{
		get
		{
			throw null;
		}
	}

	public string? EntryPoint
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool SetLastError
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringMarshalling StringMarshalling
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Type? StringMarshallingCustomType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public LibraryImportAttribute(string libraryName)
	{
	}
}
