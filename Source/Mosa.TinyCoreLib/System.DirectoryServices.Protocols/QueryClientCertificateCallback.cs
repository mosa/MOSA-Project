using System.Security.Cryptography.X509Certificates;

namespace System.DirectoryServices.Protocols;

public delegate X509Certificate QueryClientCertificateCallback(LdapConnection connection, byte[][] trustedCAs);
