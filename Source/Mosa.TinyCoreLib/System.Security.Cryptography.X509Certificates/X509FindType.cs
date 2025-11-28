namespace System.Security.Cryptography.X509Certificates;

public enum X509FindType
{
	FindByThumbprint,
	FindBySubjectName,
	FindBySubjectDistinguishedName,
	FindByIssuerName,
	FindByIssuerDistinguishedName,
	FindBySerialNumber,
	FindByTimeValid,
	FindByTimeNotYetValid,
	FindByTimeExpired,
	FindByTemplateName,
	FindByApplicationPolicy,
	FindByCertificatePolicy,
	FindByExtension,
	FindByKeyUsage,
	FindBySubjectKeyIdentifier
}
