namespace System.Runtime.Serialization;

public interface ISerializable
{
	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	void GetObjectData(SerializationInfo info, StreamingContext context);
}
