using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security;

public delegate X509Certificate ServerCertificateSelectionCallback(object sender, string? hostName);
