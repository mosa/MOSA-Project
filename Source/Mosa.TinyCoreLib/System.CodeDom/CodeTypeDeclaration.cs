using System.Reflection;

namespace System.CodeDom;

public class CodeTypeDeclaration : CodeTypeMember
{
	public CodeTypeReferenceCollection BaseTypes
	{
		get
		{
			throw null;
		}
	}

	public bool IsClass
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsEnum
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsInterface
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsPartial
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsStruct
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeTypeMemberCollection Members
	{
		get
		{
			throw null;
		}
	}

	public TypeAttributes TypeAttributes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeTypeParameterCollection TypeParameters
	{
		get
		{
			throw null;
		}
	}

	public event EventHandler PopulateBaseTypes
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler PopulateMembers
	{
		add
		{
		}
		remove
		{
		}
	}

	public CodeTypeDeclaration()
	{
	}

	public CodeTypeDeclaration(string name)
	{
	}
}
