namespace System.CodeDom;

public class CodePropertyReferenceExpression : CodeExpression
{
	public string PropertyName
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

	public CodePropertyReferenceExpression()
	{
	}

	public CodePropertyReferenceExpression(CodeExpression targetObject, string propertyName)
	{
	}
}
