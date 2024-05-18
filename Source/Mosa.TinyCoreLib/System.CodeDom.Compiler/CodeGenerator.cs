using System.IO;
using System.Reflection;

namespace System.CodeDom.Compiler;

public abstract class CodeGenerator : ICodeGenerator
{
	protected CodeTypeDeclaration CurrentClass
	{
		get
		{
			throw null;
		}
	}

	protected CodeTypeMember CurrentMember
	{
		get
		{
			throw null;
		}
	}

	protected string CurrentMemberName
	{
		get
		{
			throw null;
		}
	}

	protected string CurrentTypeName
	{
		get
		{
			throw null;
		}
	}

	protected int Indent
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected bool IsCurrentClass
	{
		get
		{
			throw null;
		}
	}

	protected bool IsCurrentDelegate
	{
		get
		{
			throw null;
		}
	}

	protected bool IsCurrentEnum
	{
		get
		{
			throw null;
		}
	}

	protected bool IsCurrentInterface
	{
		get
		{
			throw null;
		}
	}

	protected bool IsCurrentStruct
	{
		get
		{
			throw null;
		}
	}

	protected abstract string NullToken { get; }

	protected CodeGeneratorOptions Options
	{
		get
		{
			throw null;
		}
	}

	protected TextWriter Output
	{
		get
		{
			throw null;
		}
	}

	protected virtual void ContinueOnNewLine(string st)
	{
	}

	protected abstract string CreateEscapedIdentifier(string value);

	protected abstract string CreateValidIdentifier(string value);

	protected abstract void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e);

	protected abstract void GenerateArrayCreateExpression(CodeArrayCreateExpression e);

	protected abstract void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e);

	protected abstract void GenerateAssignStatement(CodeAssignStatement e);

	protected abstract void GenerateAttachEventStatement(CodeAttachEventStatement e);

	protected abstract void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes);

	protected abstract void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes);

	protected abstract void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e);

	protected virtual void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e)
	{
	}

	protected abstract void GenerateCastExpression(CodeCastExpression e);

	public virtual void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
	{
	}

	protected abstract void GenerateComment(CodeComment e);

	protected virtual void GenerateCommentStatement(CodeCommentStatement e)
	{
	}

	protected virtual void GenerateCommentStatements(CodeCommentStatementCollection e)
	{
	}

	protected virtual void GenerateCompileUnit(CodeCompileUnit e)
	{
	}

	protected virtual void GenerateCompileUnitEnd(CodeCompileUnit e)
	{
	}

	protected virtual void GenerateCompileUnitStart(CodeCompileUnit e)
	{
	}

	protected abstract void GenerateConditionStatement(CodeConditionStatement e);

	protected abstract void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c);

	protected virtual void GenerateDecimalValue(decimal d)
	{
	}

	protected virtual void GenerateDefaultValueExpression(CodeDefaultValueExpression e)
	{
	}

	protected abstract void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e);

	protected abstract void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e);

	protected virtual void GenerateDirectionExpression(CodeDirectionExpression e)
	{
	}

	protected virtual void GenerateDirectives(CodeDirectiveCollection directives)
	{
	}

	protected virtual void GenerateDoubleValue(double d)
	{
	}

	protected abstract void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c);

	protected abstract void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c);

	protected abstract void GenerateEventReferenceExpression(CodeEventReferenceExpression e);

	protected void GenerateExpression(CodeExpression e)
	{
	}

	protected abstract void GenerateExpressionStatement(CodeExpressionStatement e);

	protected abstract void GenerateField(CodeMemberField e);

	protected abstract void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e);

	protected abstract void GenerateGotoStatement(CodeGotoStatement e);

	protected abstract void GenerateIndexerExpression(CodeIndexerExpression e);

	protected abstract void GenerateIterationStatement(CodeIterationStatement e);

	protected abstract void GenerateLabeledStatement(CodeLabeledStatement e);

	protected abstract void GenerateLinePragmaEnd(CodeLinePragma e);

	protected abstract void GenerateLinePragmaStart(CodeLinePragma e);

	protected abstract void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c);

	protected abstract void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e);

	protected abstract void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e);

	protected abstract void GenerateMethodReturnStatement(CodeMethodReturnStatement e);

	protected virtual void GenerateNamespace(CodeNamespace e)
	{
	}

	protected abstract void GenerateNamespaceEnd(CodeNamespace e);

	protected abstract void GenerateNamespaceImport(CodeNamespaceImport e);

	protected void GenerateNamespaceImports(CodeNamespace e)
	{
	}

	protected void GenerateNamespaces(CodeCompileUnit e)
	{
	}

	protected abstract void GenerateNamespaceStart(CodeNamespace e);

	protected abstract void GenerateObjectCreateExpression(CodeObjectCreateExpression e);

	protected virtual void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
	{
	}

	protected virtual void GeneratePrimitiveExpression(CodePrimitiveExpression e)
	{
	}

	protected abstract void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c);

	protected abstract void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e);

	protected abstract void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e);

	protected abstract void GenerateRemoveEventStatement(CodeRemoveEventStatement e);

	protected virtual void GenerateSingleFloatValue(float s)
	{
	}

	protected virtual void GenerateSnippetCompileUnit(CodeSnippetCompileUnit e)
	{
	}

	protected abstract void GenerateSnippetExpression(CodeSnippetExpression e);

	protected abstract void GenerateSnippetMember(CodeSnippetTypeMember e);

	protected virtual void GenerateSnippetStatement(CodeSnippetStatement e)
	{
	}

	protected void GenerateStatement(CodeStatement e)
	{
	}

	protected void GenerateStatements(CodeStatementCollection stmts)
	{
	}

	protected abstract void GenerateThisReferenceExpression(CodeThisReferenceExpression e);

	protected abstract void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e);

	protected abstract void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e);

	protected abstract void GenerateTypeConstructor(CodeTypeConstructor e);

	protected abstract void GenerateTypeEnd(CodeTypeDeclaration e);

	protected virtual void GenerateTypeOfExpression(CodeTypeOfExpression e)
	{
	}

	protected virtual void GenerateTypeReferenceExpression(CodeTypeReferenceExpression e)
	{
	}

	protected void GenerateTypes(CodeNamespace e)
	{
	}

	protected abstract void GenerateTypeStart(CodeTypeDeclaration e);

	protected abstract void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e);

	protected abstract void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e);

	protected abstract string GetTypeOutput(CodeTypeReference value);

	protected abstract bool IsValidIdentifier(string value);

	public static bool IsValidLanguageIndependentIdentifier(string value)
	{
		throw null;
	}

	protected virtual void OutputAttributeArgument(CodeAttributeArgument arg)
	{
	}

	protected virtual void OutputAttributeDeclarations(CodeAttributeDeclarationCollection attributes)
	{
	}

	protected virtual void OutputDirection(FieldDirection dir)
	{
	}

	protected virtual void OutputExpressionList(CodeExpressionCollection expressions)
	{
	}

	protected virtual void OutputExpressionList(CodeExpressionCollection expressions, bool newlineBetweenItems)
	{
	}

	protected virtual void OutputFieldScopeModifier(MemberAttributes attributes)
	{
	}

	protected virtual void OutputIdentifier(string ident)
	{
	}

	protected virtual void OutputMemberAccessModifier(MemberAttributes attributes)
	{
	}

	protected virtual void OutputMemberScopeModifier(MemberAttributes attributes)
	{
	}

	protected virtual void OutputOperator(CodeBinaryOperatorType op)
	{
	}

	protected virtual void OutputParameters(CodeParameterDeclarationExpressionCollection parameters)
	{
	}

	protected abstract void OutputType(CodeTypeReference typeRef);

	protected virtual void OutputTypeAttributes(TypeAttributes attributes, bool isStruct, bool isEnum)
	{
	}

	protected virtual void OutputTypeNamePair(CodeTypeReference typeRef, string name)
	{
	}

	protected abstract string QuoteSnippetString(string value);

	protected abstract bool Supports(GeneratorSupport support);

	string ICodeGenerator.CreateEscapedIdentifier(string value)
	{
		throw null;
	}

	string ICodeGenerator.CreateValidIdentifier(string value)
	{
		throw null;
	}

	void ICodeGenerator.GenerateCodeFromCompileUnit(CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o)
	{
	}

	void ICodeGenerator.GenerateCodeFromExpression(CodeExpression e, TextWriter w, CodeGeneratorOptions o)
	{
	}

	void ICodeGenerator.GenerateCodeFromNamespace(CodeNamespace e, TextWriter w, CodeGeneratorOptions o)
	{
	}

	void ICodeGenerator.GenerateCodeFromStatement(CodeStatement e, TextWriter w, CodeGeneratorOptions o)
	{
	}

	void ICodeGenerator.GenerateCodeFromType(CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o)
	{
	}

	string ICodeGenerator.GetTypeOutput(CodeTypeReference type)
	{
		throw null;
	}

	bool ICodeGenerator.IsValidIdentifier(string value)
	{
		throw null;
	}

	bool ICodeGenerator.Supports(GeneratorSupport support)
	{
		throw null;
	}

	void ICodeGenerator.ValidateIdentifier(string value)
	{
	}

	protected virtual void ValidateIdentifier(string value)
	{
	}

	public static void ValidateIdentifiers(CodeObject e)
	{
	}
}
