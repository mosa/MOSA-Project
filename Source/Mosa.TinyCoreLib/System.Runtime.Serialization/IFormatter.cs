using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Runtime.Serialization;

[Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", DiagnosticId = "SYSLIB0011", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public interface IFormatter
{
	SerializationBinder? Binder { get; set; }

	StreamingContext Context { get; set; }

	ISurrogateSelector? SurrogateSelector { get; set; }

	[RequiresDynamicCode("BinaryFormatter serialization uses dynamic code generation, the type of objects being processed cannot be statically discovered.")]
	[RequiresUnreferencedCode("BinaryFormatter serialization is not trim compatible because the type of objects being processed cannot be statically discovered.")]
	object Deserialize(Stream serializationStream);

	[RequiresUnreferencedCode("BinaryFormatter serialization is not trim compatible because the type of objects being processed cannot be statically discovered.")]
	void Serialize(Stream serializationStream, object graph);
}
