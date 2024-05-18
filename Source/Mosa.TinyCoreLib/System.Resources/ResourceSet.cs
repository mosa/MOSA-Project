using System.Collections;
using System.IO;

namespace System.Resources;

public class ResourceSet : IEnumerable, IDisposable
{
	protected ResourceSet()
	{
	}

	public ResourceSet(Stream stream)
	{
	}

	public ResourceSet(IResourceReader reader)
	{
	}

	public ResourceSet(string fileName)
	{
	}

	public virtual void Close()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual Type GetDefaultReader()
	{
		throw null;
	}

	public virtual Type GetDefaultWriter()
	{
		throw null;
	}

	public virtual IDictionaryEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual object? GetObject(string name)
	{
		throw null;
	}

	public virtual object? GetObject(string name, bool ignoreCase)
	{
		throw null;
	}

	public virtual string? GetString(string name)
	{
		throw null;
	}

	public virtual string? GetString(string name, bool ignoreCase)
	{
		throw null;
	}

	protected virtual void ReadResources()
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
