namespace System.CodeDom;

public class CodeLabeledStatement : CodeStatement
{
	public string Label
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeStatement Statement
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeLabeledStatement()
	{
	}

	public CodeLabeledStatement(string label)
	{
	}

	public CodeLabeledStatement(string label, CodeStatement statement)
	{
	}
}
