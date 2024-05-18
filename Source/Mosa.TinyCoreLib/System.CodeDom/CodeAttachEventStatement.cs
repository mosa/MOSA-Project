namespace System.CodeDom;

public class CodeAttachEventStatement : CodeStatement
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

	public CodeAttachEventStatement()
	{
	}

	public CodeAttachEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener)
	{
	}

	public CodeAttachEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener)
	{
	}
}
