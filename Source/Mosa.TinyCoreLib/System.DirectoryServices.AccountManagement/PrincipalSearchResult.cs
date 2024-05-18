using System.Collections;
using System.Collections.Generic;

namespace System.DirectoryServices.AccountManagement;

public class PrincipalSearchResult<T> : IEnumerable<T>, IEnumerable, IDisposable
{
	internal PrincipalSearchResult()
	{
	}

	public void Dispose()
	{
	}

	public IEnumerator<T> GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
