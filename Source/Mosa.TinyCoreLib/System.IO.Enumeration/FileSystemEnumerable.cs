using System.Collections;
using System.Collections.Generic;

namespace System.IO.Enumeration;

public class FileSystemEnumerable<TResult> : IEnumerable<TResult>, IEnumerable
{
	public delegate bool FindPredicate(ref FileSystemEntry entry);

	public delegate TResult FindTransform(ref FileSystemEntry entry);

	public FindPredicate? ShouldIncludePredicate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public FindPredicate? ShouldRecursePredicate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public FileSystemEnumerable(string directory, FindTransform transform, EnumerationOptions? options = null)
	{
	}

	public IEnumerator<TResult> GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
