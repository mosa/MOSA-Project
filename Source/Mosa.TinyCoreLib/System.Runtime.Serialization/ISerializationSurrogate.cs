namespace System.Runtime.Serialization;

[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public interface ISerializationSurrogate
{
	void GetObjectData(object obj, SerializationInfo info, StreamingContext context);

	object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector? selector);
}
