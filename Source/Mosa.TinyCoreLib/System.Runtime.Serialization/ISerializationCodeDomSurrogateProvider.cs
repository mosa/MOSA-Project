using System.CodeDom;

namespace System.Runtime.Serialization;

public interface ISerializationCodeDomSurrogateProvider
{
	CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit);
}
