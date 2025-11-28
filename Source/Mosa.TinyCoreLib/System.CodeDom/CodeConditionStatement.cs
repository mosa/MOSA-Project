namespace System.CodeDom;

public class CodeConditionStatement : CodeStatement
{
	public CodeExpression Condition
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeStatementCollection FalseStatements
	{
		get
		{
			throw null;
		}
	}

	public CodeStatementCollection TrueStatements
	{
		get
		{
			throw null;
		}
	}

	public CodeConditionStatement()
	{
	}

	public CodeConditionStatement(CodeExpression condition, params CodeStatement[] trueStatements)
	{
	}

	public CodeConditionStatement(CodeExpression condition, CodeStatement[] trueStatements, CodeStatement[] falseStatements)
	{
	}
}
