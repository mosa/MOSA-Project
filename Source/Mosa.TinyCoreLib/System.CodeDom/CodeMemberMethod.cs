namespace System.CodeDom;

public class CodeMemberMethod : CodeTypeMember
{
	public CodeTypeReferenceCollection ImplementationTypes
	{
		get
		{
			throw null;
		}
	}

	public CodeParameterDeclarationExpressionCollection Parameters
	{
		get
		{
			throw null;
		}
	}

	public CodeTypeReference PrivateImplementationType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeTypeReference ReturnType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeAttributeDeclarationCollection ReturnTypeCustomAttributes
	{
		get
		{
			throw null;
		}
	}

	public CodeStatementCollection Statements
	{
		get
		{
			throw null;
		}
	}

	public CodeTypeParameterCollection TypeParameters
	{
		get
		{
			throw null;
		}
	}

	public event EventHandler PopulateImplementationTypes
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler PopulateParameters
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler PopulateStatements
	{
		add
		{
		}
		remove
		{
		}
	}
}
