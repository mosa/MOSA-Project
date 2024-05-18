namespace System.Net.Security;

[Flags]
public enum SslPolicyErrors
{
	None = 0,
	RemoteCertificateNotAvailable = 1,
	RemoteCertificateNameMismatch = 2,
	RemoteCertificateChainErrors = 4
}
