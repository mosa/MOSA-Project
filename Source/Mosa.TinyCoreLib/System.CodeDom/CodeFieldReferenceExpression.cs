namespace System.CodeDom;

public class CodeFieldReferenceExpression : CodeExpression
{
	public string FieldName
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

	public CodeFieldReferenceExpression()
	{
	}

	public CodeFieldReferenceExpression(CodeExpression targetObject, string fieldName)
	{
	}
}
