using System.IO;

namespace System.Resources;

public sealed class ResourceWriter : IDisposable, IResourceWriter
{
	public Func<Type, string>? TypeNameConverter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ResourceWriter(Stream stream)
	{
	}

	public ResourceWriter(string fileName)
	{
	}

	public void AddResource(string name, byte[]? value)
	{
	}

	public void AddResource(string name, Stream? value)
	{
	}

	public void AddResource(string name, Stream? value, bool closeAfterWrite = false)
	{
	}

	public void AddResource(string name, object? value)
	{
	}

	public void AddResource(string name, string? value)
	{
	}

	public void AddResourceData(string name, string typeName, byte[] serializedData)
	{
	}

	public void Close()
	{
	}

	public void Dispose()
	{
	}

	public void Generate()
	{
	}
}
