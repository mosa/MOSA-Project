namespace System.CodeDom;

public class CodeMethodInvokeExpression : CodeExpression
{
	public CodeMethodReferenceExpression Method
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeExpressionCollection Parameters
	{
		get
		{
			throw null;
		}
	}

	public CodeMethodInvokeExpression()
	{
	}

	public CodeMethodInvokeExpression(CodeExpression targetObject, string methodName, params CodeExpression[] parameters)
	{
	}

	public CodeMethodInvokeExpression(CodeMethodReferenceExpression method, params CodeExpression[] parameters)
	{
	}
}
