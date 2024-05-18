namespace System.CodeDom;

public class CodeCatchClause
{
	public CodeTypeReference CatchExceptionType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string LocalName
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

	public CodeCatchClause()
	{
	}

	public CodeCatchClause(string localName)
	{
	}

	public CodeCatchClause(string localName, CodeTypeReference catchExceptionType)
	{
	}

	public CodeCatchClause(string localName, CodeTypeReference catchExceptionType, params CodeStatement[] statements)
	{
	}
}
