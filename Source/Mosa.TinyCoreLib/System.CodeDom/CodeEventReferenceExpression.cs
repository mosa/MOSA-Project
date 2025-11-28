namespace System.CodeDom;

public class CodeEventReferenceExpression : CodeExpression
{
	public string EventName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeExpression TargetObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeEventReferenceExpression()
	{
	}

	public CodeEventReferenceExpression(CodeExpression targetObject, string eventName)
	{
	}
}
