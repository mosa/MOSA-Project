using System.Security.Cryptography.X509Certificates;

namespace System.DirectoryServices.Protocols;

public delegate bool VerifyServerCertificateCallback(LdapConnection connection, X509Certificate certificate);
