namespace System.CodeDom;

public class CodeRemoveEventStatement : CodeStatement
{
	public CodeEventReferenceExpression Event
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeExpression Listener
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeRemoveEventStatement()
	{
	}

	public CodeRemoveEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener)
	{
	}

	public CodeRemoveEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener)
	{
	}
}
