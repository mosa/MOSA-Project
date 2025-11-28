namespace System.Reflection.Metadata;

public readonly struct LocalScope
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public int EndOffset
	{
		get
		{
			throw null;
		}
	}

	public ImportScopeHandle ImportScope
	{
		get
		{
			throw null;
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public MethodDefinitionHandle Method
	{
		get
		{
			throw null;
		}
	}

	public int StartOffset
	{
		get
		{
			throw null;
		}
	}

	public LocalScopeHandleCollection.ChildrenEnumerator GetChildren()
	{
		throw null;
	}

	public LocalConstantHandleCollection GetLocalConstants()
	{
		throw null;
	}

	public LocalVariableHandleCollection GetLocalVariables()
	{
		throw null;
	}
}
