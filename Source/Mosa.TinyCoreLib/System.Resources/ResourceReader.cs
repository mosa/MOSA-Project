using System.Collections;
using System.IO;

namespace System.Resources;

public sealed class ResourceReader : IEnumerable, IDisposable, IResourceReader
{
	public ResourceReader(Stream stream)
	{
	}

	public ResourceReader(string fileName)
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

	public void GetResourceData(string resourceName, out string resourceType, out byte[] resourceData)
	{
		throw null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
