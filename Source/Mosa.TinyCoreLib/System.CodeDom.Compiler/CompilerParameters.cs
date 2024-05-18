using System.Collections.Specialized;

namespace System.CodeDom.Compiler;

public class CompilerParameters
{
	public string CompilerOptions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string CoreAssemblyFileName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringCollection EmbeddedResources
	{
		get
		{
			throw null;
		}
	}

	public bool GenerateExecutable
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool GenerateInMemory
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IncludeDebugInformation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringCollection LinkedResources
	{
		get
		{
			throw null;
		}
	}

	public string MainClass
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string OutputAssembly
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringCollection ReferencedAssemblies
	{
		get
		{
			throw null;
		}
	}

	public TempFileCollection TempFiles
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool TreatWarningsAsErrors
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IntPtr UserToken
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int WarningLevel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Win32Resource
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CompilerParameters()
	{
	}

	public CompilerParameters(string[] assemblyNames)
	{
	}

	public CompilerParameters(string[] assemblyNames, string outputName)
	{
	}

	public CompilerParameters(string[] assemblyNames, string outputName, bool includeDebugInformation)
	{
	}
}
