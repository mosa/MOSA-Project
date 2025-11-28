using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

public class StrongBox<T> : IStrongBox
{
	[MaybeNull]
	public T Value;

	object? IStrongBox.Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StrongBox()
	{
	}

	public StrongBox(T value)
	{
	}
}
