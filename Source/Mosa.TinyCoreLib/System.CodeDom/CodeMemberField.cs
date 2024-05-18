namespace System.CodeDom;

public class CodeMemberField : CodeTypeMember
{
	public CodeExpression InitExpression
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeTypeReference Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeMemberField()
	{
	}

	public CodeMemberField(CodeTypeReference type, string name)
	{
	}

	public CodeMemberField(string type, string name)
	{
	}

	public CodeMemberField(Type type, string name)
	{
	}
}
