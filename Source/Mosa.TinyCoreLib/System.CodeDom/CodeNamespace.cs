namespace System.CodeDom;

public class CodeNamespace : CodeObject
{
	public CodeCommentStatementCollection Comments
	{
		get
		{
			throw null;
		}
	}

	public CodeNamespaceImportCollection Imports
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

	public CodeTypeDeclarationCollection Types
	{
		get
		{
			throw null;
		}
	}

	public event EventHandler PopulateComments
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler PopulateImports
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler PopulateTypes
	{
		add
		{
		}
		remove
		{
		}
	}

	public CodeNamespace()
	{
	}

	public CodeNamespace(string name)
	{
	}
}
