using System.Runtime.Versioning;

namespace System.IO.Pipes;

public enum PipeTransmissionMode
{
	Byte,
	[SupportedOSPlatform("windows")]
	Message
}
