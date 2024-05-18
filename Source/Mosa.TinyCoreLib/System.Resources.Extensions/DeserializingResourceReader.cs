using System.Collections;
using System.IO;

namespace System.Resources.Extensions;

public sealed class DeserializingResourceReader : IEnumerable, IDisposable, IResourceReader
{
	public DeserializingResourceReader(Stream stream)
	{
	}

	public DeserializingResourceReader(string fileName)
	{
	}

	public void Close()
	{
	}

	public void Dispose()
	{
	}

	public IDictionaryEnumerator GetEnumerator()
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
