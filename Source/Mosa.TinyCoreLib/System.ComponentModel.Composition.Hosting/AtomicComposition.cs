using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.Composition.Hosting;

public class AtomicComposition : IDisposable
{
	public AtomicComposition()
	{
	}

	public AtomicComposition(AtomicComposition? outerAtomicComposition)
	{
	}

	public void AddCompleteAction(Action completeAction)
	{
	}

	public void AddRevertAction(Action revertAction)
	{
	}

	public void Complete()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void SetValue(object key, object? value)
	{
	}

	public bool TryGetValue<T>(object key, bool localAtomicCompositionOnly, [MaybeNullWhen(false)] out T value)
	{
		throw null;
	}

	public bool TryGetValue<T>(object key, [MaybeNullWhen(false)] out T value)
	{
		throw null;
	}
}
