namespace System.CodeDom;

public class CodeDelegateInvokeExpression : CodeExpression
{
	public CodeExpressionCollection Parameters
	{
		get
		{
			throw null;
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

	public CodeDelegateInvokeExpression()
	{
	}

	public CodeDelegateInvokeExpression(CodeExpression targetObject)
	{
	}

	public CodeDelegateInvokeExpression(CodeExpression targetObject, params CodeExpression[] parameters)
	{
	}
}
