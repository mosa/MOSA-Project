using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Runtime.Serialization.Formatters.Binary;

[Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", DiagnosticId = "SYSLIB0011", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class BinaryFormatter : IFormatter
{
	public FormatterAssemblyStyle AssemblyFormat
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SerializationBinder? Binder
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StreamingContext Context
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TypeFilterLevel FilterLevel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ISurrogateSelector? SurrogateSelector
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public FormatterTypeStyle TypeFormat
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public BinaryFormatter()
	{
	}

	public BinaryFormatter(ISurrogateSelector? selector, StreamingContext context)
	{
	}

	[RequiresDynamicCode("BinaryFormatter serialization uses dynamic code generation, the type of objects being processed cannot be statically discovered.")]
	[RequiresUnreferencedCode("BinaryFormatter serialization is not trim compatible because the type of objects being processed cannot be statically discovered.")]
	public object Deserialize(Stream serializationStream)
	{
		throw null;
	}

	[RequiresUnreferencedCode("BinaryFormatter serialization is not trim compatible because the type of objects being processed cannot be statically discovered.")]
	public void Serialize(Stream serializationStream, object graph)
	{
	}
}
