using System.IO;

namespace System.Resources.Extensions;

public sealed class PreserializedResourceWriter : IDisposable, IResourceWriter
{
	public PreserializedResourceWriter(Stream stream)
	{
	}

	public PreserializedResourceWriter(string fileName)
	{
	}

	public void AddActivatorResource(string name, Stream value, string typeName, bool closeAfterWrite = false)
	{
	}

	[Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", DiagnosticId = "SYSLIB0011", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public void AddBinaryFormattedResource(string name, byte[] value, string? typeName = null)
	{
	}

	public void AddResource(string name, byte[]? value)
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

	public void AddResource(string name, string value, string typeName)
	{
	}

	public void AddTypeConverterResource(string name, byte[] value, string typeName)
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
