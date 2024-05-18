namespace System.CodeDom;

public class CodeParameterDeclarationExpression : CodeExpression
{
	public CodeAttributeDeclarationCollection CustomAttributes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public FieldDirection Direction
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
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

	public CodeParameterDeclarationExpression()
	{
	}

	public CodeParameterDeclarationExpression(CodeTypeReference type, string name)
	{
	}

	public CodeParameterDeclarationExpression(string type, string name)
	{
	}

	public CodeParameterDeclarationExpression(Type type, string name)
	{
	}
}
