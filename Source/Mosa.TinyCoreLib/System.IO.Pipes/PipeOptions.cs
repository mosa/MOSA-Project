namespace System.IO.Pipes;

[Flags]
public enum PipeOptions
{
	WriteThrough = int.MinValue,
	None = 0,
	CurrentUserOnly = 0x20000000,
	Asynchronous = 0x40000000,
	FirstPipeInstance = 0x80000
}
