namespace System.Net.Quic;

[Flags]
public enum QuicAbortDirection
{
	Read = 1,
	Write = 2,
	Both = 3
}
