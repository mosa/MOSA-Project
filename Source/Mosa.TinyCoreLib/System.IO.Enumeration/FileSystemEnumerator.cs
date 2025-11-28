using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace System.IO.Enumeration;

public abstract class FileSystemEnumerator<TResult> : CriticalFinalizerObject, IEnumerator<TResult>, IEnumerator, IDisposable
{
	public TResult Current
	{
		get
		{
			throw null;
		}
	}

	object? IEnumerator.Current
	{
		get
		{
			throw null;
		}
	}

	public FileSystemEnumerator(string directory, EnumerationOptions? options = null)
	{
	}

	protected virtual bool ContinueOnError(int error)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public bool MoveNext()
	{
		throw null;
	}

	protected virtual void OnDirectoryFinished(ReadOnlySpan<char> directory)
	{
	}

	public void Reset()
	{
	}

	protected virtual bool ShouldIncludeEntry(ref FileSystemEntry entry)
	{
		throw null;
	}

	protected virtual bool ShouldRecurseIntoEntry(ref FileSystemEntry entry)
	{
		throw null;
	}

	protected abstract TResult TransformEntry(ref FileSystemEntry entry);
}
