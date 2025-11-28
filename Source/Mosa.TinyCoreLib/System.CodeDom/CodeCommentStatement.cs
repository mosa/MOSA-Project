namespace System.CodeDom;

public class CodeCommentStatement : CodeStatement
{
	public CodeComment Comment
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeCommentStatement()
	{
	}

	public CodeCommentStatement(CodeComment comment)
	{
	}

	public CodeCommentStatement(string text)
	{
	}

	public CodeCommentStatement(string text, bool docComment)
	{
	}
}
