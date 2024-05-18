namespace System.CodeDom.Compiler;

public abstract class CodeCompiler : CodeGenerator, ICodeCompiler
{
	protected abstract string CompilerName { get; }

	protected abstract string FileExtension { get; }

	protected abstract string CmdArgsFromParameters(CompilerParameters options);

	protected virtual CompilerResults FromDom(CompilerParameters options, CodeCompileUnit e)
	{
		throw null;
	}

	protected virtual CompilerResults FromDomBatch(CompilerParameters options, CodeCompileUnit[] ea)
	{
		throw null;
	}

	protected virtual CompilerResults FromFile(CompilerParameters options, string fileName)
	{
		throw null;
	}

	protected virtual CompilerResults FromFileBatch(CompilerParameters options, string[] fileNames)
	{
		throw null;
	}

	protected virtual CompilerResults FromSource(CompilerParameters options, string source)
	{
		throw null;
	}

	protected virtual CompilerResults FromSourceBatch(CompilerParameters options, string[] sources)
	{
		throw null;
	}

	protected virtual string GetResponseFileCmdArgs(CompilerParameters options, string cmdArgs)
	{
		throw null;
	}

	protected static string JoinStringArray(string[] sa, string separator)
	{
		throw null;
	}

	protected abstract void ProcessCompilerOutputLine(CompilerResults results, string line);

	CompilerResults ICodeCompiler.CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit e)
	{
		throw null;
	}

	CompilerResults ICodeCompiler.CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] ea)
	{
		throw null;
	}

	CompilerResults ICodeCompiler.CompileAssemblyFromFile(CompilerParameters options, string fileName)
	{
		throw null;
	}

	CompilerResults ICodeCompiler.CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames)
	{
		throw null;
	}

	CompilerResults ICodeCompiler.CompileAssemblyFromSource(CompilerParameters options, string source)
	{
		throw null;
	}

	CompilerResults ICodeCompiler.CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources)
	{
		throw null;
	}
}
