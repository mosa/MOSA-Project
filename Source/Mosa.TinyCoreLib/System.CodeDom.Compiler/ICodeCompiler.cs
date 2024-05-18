namespace System.CodeDom.Compiler;

public interface ICodeCompiler
{
	CompilerResults CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit compilationUnit);

	CompilerResults CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] compilationUnits);

	CompilerResults CompileAssemblyFromFile(CompilerParameters options, string fileName);

	CompilerResults CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames);

	CompilerResults CompileAssemblyFromSource(CompilerParameters options, string source);

	CompilerResults CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources);
}
