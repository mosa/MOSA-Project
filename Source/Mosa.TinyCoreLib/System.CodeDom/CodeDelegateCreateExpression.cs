namespace System.CodeDom;

public class CodeDelegateCreateExpression : CodeExpression
{
	public CodeTypeReference DelegateType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public CodeDelegateCreateExpression()
	{
	}

	public CodeDelegateCreateExpression(CodeTypeReference delegateType, CodeExpression targetObject, string methodName)
	{
	}
}
