using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security;

public delegate bool RemoteCertificateValidationCallback(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors);
