namespace System.CodeDom;

public class CodeAttributeDeclaration
{
	public CodeAttributeArgumentCollection Arguments
	{
		get
		{
			throw null;
		}
	}

	public CodeTypeReference AttributeType
	{
		get
		{
			throw null;
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

	public CodeAttributeDeclaration()
	{
	}

	public CodeAttributeDeclaration(CodeTypeReference attributeType)
	{
	}

	public CodeAttributeDeclaration(CodeTypeReference attributeType, params CodeAttributeArgument[] arguments)
	{
	}

	public CodeAttributeDeclaration(string name)
	{
	}

	public CodeAttributeDeclaration(string name, params CodeAttributeArgument[] arguments)
	{
	}
}
