using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security;

public delegate X509Certificate LocalCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate? remoteCertificate, string[] acceptableIssuers);
