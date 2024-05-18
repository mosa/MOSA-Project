namespace System.Data;

public enum SerializationFormat
{
	Xml,
	[Obsolete("SerializationFormat.Binary is obsolete and should not be used. See https://aka.ms/serializationformat-binary-obsolete for more information.", DiagnosticId = "SYSLIB0038")]
	Binary
}
