namespace System.Net;

[Flags]
public enum DecompressionMethods
{
	All = -1,
	None = 0,
	GZip = 1,
	Deflate = 2,
	Brotli = 4
}
