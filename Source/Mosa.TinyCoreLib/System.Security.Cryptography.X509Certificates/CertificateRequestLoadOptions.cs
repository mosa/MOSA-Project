namespace System.Security.Cryptography.X509Certificates;

[Flags]
public enum CertificateRequestLoadOptions
{
	Default = 0,
	SkipSignatureValidation = 1,
	UnsafeLoadCertificateExtensions = 2
}
