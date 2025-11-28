namespace System.CodeDom;

public class CodeIterationStatement : CodeStatement
{
	public CodeStatement IncrementStatement
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeStatement InitStatement
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeStatementCollection Statements
	{
		get
		{
			throw null;
		}
	}

	public CodeExpression TestExpression
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeIterationStatement()
	{
	}

	public CodeIterationStatement(CodeStatement initStatement, CodeExpression testExpression, CodeStatement incrementStatement, params CodeStatement[] statements)
	{
	}
}
