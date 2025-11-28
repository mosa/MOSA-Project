using System.Collections;

namespace System.CodeDom.Compiler;

public class CompilerErrorCollection : CollectionBase
{
	public bool HasErrors
	{
		get
		{
			throw null;
		}
	}

	public bool HasWarnings
	{
		get
		{
			throw null;
		}
	}

	public CompilerError this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CompilerErrorCollection()
	{
	}

	public CompilerErrorCollection(CompilerErrorCollection value)
	{
	}

	public CompilerErrorCollection(CompilerError[] value)
	{
	}

	public int Add(CompilerError value)
	{
		throw null;
	}

	public void AddRange(CompilerErrorCollection value)
	{
	}

	public void AddRange(CompilerError[] value)
	{
	}

	public bool Contains(CompilerError value)
	{
		throw null;
	}

	public void CopyTo(CompilerError[] array, int index)
	{
	}

	public int IndexOf(CompilerError value)
	{
		throw null;
	}

	public void Insert(int index, CompilerError value)
	{
	}

	public void Remove(CompilerError value)
	{
	}
}
