namespace System.CodeDom;

public class CodeTypeReference : CodeObject
{
	public CodeTypeReference ArrayElementType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int ArrayRank
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string BaseType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeTypeReferenceOptions Options
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

	public CodeTypeReference()
	{
	}

	public CodeTypeReference(CodeTypeParameter typeParameter)
	{
	}

	public CodeTypeReference(CodeTypeReference arrayType, int rank)
	{
	}

	public CodeTypeReference(string typeName)
	{
	}

	public CodeTypeReference(string typeName, CodeTypeReferenceOptions codeTypeReferenceOption)
	{
	}

	public CodeTypeReference(string typeName, params CodeTypeReference[] typeArguments)
	{
	}

	public CodeTypeReference(string baseType, int rank)
	{
	}

	public CodeTypeReference(Type type)
	{
	}

	public CodeTypeReference(Type type, CodeTypeReferenceOptions codeTypeReferenceOption)
	{
	}
}
