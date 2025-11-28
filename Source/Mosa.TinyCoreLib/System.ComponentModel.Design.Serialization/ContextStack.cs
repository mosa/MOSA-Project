namespace System.ComponentModel.Design.Serialization;

public sealed class ContextStack
{
	public object? Current
	{
		get
		{
			throw null;
		}
	}

	public object? this[int level]
	{
		get
		{
			throw null;
		}
	}

	public object? this[Type type]
	{
		get
		{
			throw null;
		}
	}

	public void Append(object context)
	{
	}

	public object? Pop()
	{
		throw null;
	}

	public void Push(object context)
	{
	}
}
