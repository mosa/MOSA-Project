using System.Collections.Specialized;
using System.Reflection;

namespace System.CodeDom.Compiler;

public class CompilerResults
{
	public Assembly CompiledAssembly
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CompilerErrorCollection Errors
	{
		get
		{
			throw null;
		}
	}

	public int NativeCompilerReturnValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringCollection Output
	{
		get
		{
			throw null;
		}
	}

	public string PathToAssembly
	{
		get
		{
			throw null;
		}
		set
		{
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

	public CompilerResults(TempFileCollection tempFiles)
	{
	}
}
