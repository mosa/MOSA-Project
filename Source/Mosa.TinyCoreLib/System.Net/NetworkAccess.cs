namespace System.Net;

[Flags]
public enum NetworkAccess
{
	Connect = 0x40,
	Accept = 0x80
}
