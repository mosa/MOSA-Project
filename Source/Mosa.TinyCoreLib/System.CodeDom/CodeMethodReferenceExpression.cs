namespace System.CodeDom;

public class CodeMethodReferenceExpression : CodeExpression
{
	public string MethodName
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

	public CodeTypeReferenceCollection TypeArguments
	{
		get
		{
			throw null;
		}
	}

	public CodeMethodReferenceExpression()
	{
	}

	public CodeMethodReferenceExpression(CodeExpression targetObject, string methodName)
	{
	}

	public CodeMethodReferenceExpression(CodeExpression targetObject, string methodName, params CodeTypeReference[] typeParameters)
	{
	}
}
