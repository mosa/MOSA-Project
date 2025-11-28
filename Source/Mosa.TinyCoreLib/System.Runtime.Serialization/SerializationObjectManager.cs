using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Serialization;

[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public sealed class SerializationObjectManager
{
	public SerializationObjectManager(StreamingContext context)
	{
	}

	public void RaiseOnSerializedEvent()
	{
	}

	[RequiresUnreferencedCode("SerializationObjectManager is not trim compatible because the type of objects being managed cannot be statically discovered.")]
	public void RegisterObject(object obj)
	{
	}
}
