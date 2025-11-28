namespace System.DirectoryServices.Protocols;

public enum SecurityProtocol
{
	Pct1Server = 1,
	Pct1Client = 2,
	Ssl2Server = 4,
	Ssl2Client = 8,
	Ssl3Server = 0x10,
	Ssl3Client = 0x20,
	Tls1Server = 0x40,
	Tls1Client = 0x80
}
