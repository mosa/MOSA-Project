namespace System.CodeDom;

public class CodeTryCatchFinallyStatement : CodeStatement
{
	public CodeCatchClauseCollection CatchClauses
	{
		get
		{
			throw null;
		}
	}

	public CodeStatementCollection FinallyStatements
	{
		get
		{
			throw null;
		}
	}

	public CodeStatementCollection TryStatements
	{
		get
		{
			throw null;
		}
	}

	public CodeTryCatchFinallyStatement()
	{
	}

	public CodeTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses)
	{
	}

	public CodeTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses, CodeStatement[] finallyStatements)
	{
	}
}
