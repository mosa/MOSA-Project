using System.IO;

namespace System.CodeDom.Compiler;

public interface ICodeGenerator
{
	string CreateEscapedIdentifier(string value);

	string CreateValidIdentifier(string value);

	void GenerateCodeFromCompileUnit(CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o);

	void GenerateCodeFromExpression(CodeExpression e, TextWriter w, CodeGeneratorOptions o);

	void GenerateCodeFromNamespace(CodeNamespace e, TextWriter w, CodeGeneratorOptions o);

	void GenerateCodeFromStatement(CodeStatement e, TextWriter w, CodeGeneratorOptions o);

	void GenerateCodeFromType(CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o);

	string GetTypeOutput(CodeTypeReference type);

	bool IsValidIdentifier(string value);

	bool Supports(GeneratorSupport supports);

	void ValidateIdentifier(string value);
}
