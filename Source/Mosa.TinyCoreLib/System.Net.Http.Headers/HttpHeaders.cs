using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public abstract class HttpHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
{
	public HttpHeadersNonValidated NonValidated
	{
		get
		{
			throw null;
		}
	}

	public void Add(string name, IEnumerable<string?> values)
	{
	}

	public void Add(string name, string? value)
	{
	}

	public void Clear()
	{
	}

	public bool Contains(string name)
	{
		throw null;
	}

	public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
	{
		throw null;
	}

	public IEnumerable<string> GetValues(string name)
	{
		throw null;
	}

	public bool Remove(string name)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public bool TryAddWithoutValidation(string name, IEnumerable<string?> values)
	{
		throw null;
	}

	public bool TryAddWithoutValidation(string name, string? value)
	{
		throw null;
	}

	public bool TryGetValues(string name, [NotNullWhen(true)] out IEnumerable<string>? values)
	{
		throw null;
	}
}
