using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace System.CodeDom.Compiler;

[ToolboxItem(false)]
public abstract class CodeDomProvider : Component
{
	public virtual string FileExtension
	{
		get
		{
			throw null;
		}
	}

	public virtual LanguageOptions LanguageOptions
	{
		get
		{
			throw null;
		}
	}

	public virtual CompilerResults CompileAssemblyFromDom(CompilerParameters options, params CodeCompileUnit[] compilationUnits)
	{
		throw null;
	}

	public virtual CompilerResults CompileAssemblyFromFile(CompilerParameters options, params string[] fileNames)
	{
		throw null;
	}

	public virtual CompilerResults CompileAssemblyFromSource(CompilerParameters options, params string[] sources)
	{
		throw null;
	}

	[Obsolete("ICodeCompiler has been deprecated. Use the methods directly on the CodeDomProvider class instead. Classes inheriting from CodeDomProvider must still implement this interface, and should suppress this warning or also mark this method as obsolete.")]
	public abstract ICodeCompiler CreateCompiler();

	public virtual string CreateEscapedIdentifier(string value)
	{
		throw null;
	}

	[Obsolete("ICodeGenerator has been deprecated. Use the methods directly on the CodeDomProvider class instead. Classes inheriting from CodeDomProvider must still implement this interface, and should suppress this warning or also mark this method as obsolete.")]
	public abstract ICodeGenerator CreateGenerator();

	public virtual ICodeGenerator CreateGenerator(TextWriter output)
	{
		throw null;
	}

	public virtual ICodeGenerator CreateGenerator(string fileName)
	{
		throw null;
	}

	[Obsolete("ICodeParser has been deprecated. Use the methods directly on the CodeDomProvider class instead. Classes inheriting from CodeDomProvider must still implement this interface, and should suppress this warning or also mark this method as obsolete.")]
	public virtual ICodeParser CreateParser()
	{
		throw null;
	}

	public static CodeDomProvider CreateProvider(string language)
	{
		throw null;
	}

	public static CodeDomProvider CreateProvider(string language, IDictionary<string, string> providerOptions)
	{
		throw null;
	}

	public virtual string CreateValidIdentifier(string value)
	{
		throw null;
	}

	public virtual void GenerateCodeFromCompileUnit(CodeCompileUnit compileUnit, TextWriter writer, CodeGeneratorOptions options)
	{
	}

	public virtual void GenerateCodeFromExpression(CodeExpression expression, TextWriter writer, CodeGeneratorOptions options)
	{
	}

	public virtual void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
	{
	}

	public virtual void GenerateCodeFromNamespace(CodeNamespace codeNamespace, TextWriter writer, CodeGeneratorOptions options)
	{
	}

	public virtual void GenerateCodeFromStatement(CodeStatement statement, TextWriter writer, CodeGeneratorOptions options)
	{
	}

	public virtual void GenerateCodeFromType(CodeTypeDeclaration codeType, TextWriter writer, CodeGeneratorOptions options)
	{
	}

	public static CompilerInfo[] GetAllCompilerInfo()
	{
		throw null;
	}

	public static CompilerInfo GetCompilerInfo(string language)
	{
		throw null;
	}

	public virtual TypeConverter GetConverter(Type type)
	{
		throw null;
	}

	public static string GetLanguageFromExtension(string extension)
	{
		throw null;
	}

	public virtual string GetTypeOutput(CodeTypeReference type)
	{
		throw null;
	}

	public static bool IsDefinedExtension(string extension)
	{
		throw null;
	}

	public static bool IsDefinedLanguage(string language)
	{
		throw null;
	}

	public virtual bool IsValidIdentifier(string value)
	{
		throw null;
	}

	public virtual CodeCompileUnit Parse(TextReader codeStream)
	{
		throw null;
	}

	public virtual bool Supports(GeneratorSupport generatorSupport)
	{
		throw null;
	}
}
