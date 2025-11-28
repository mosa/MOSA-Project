namespace System.Runtime.Serialization;

[Flags]
[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public enum StreamingContextStates
{
	CrossProcess = 1,
	CrossMachine = 2,
	File = 4,
	Persistence = 8,
	Remoting = 0x10,
	Other = 0x20,
	Clone = 0x40,
	CrossAppDomain = 0x80,
	All = 0xFF
}
