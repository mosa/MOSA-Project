using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace System.DirectoryServices.Protocols;

public abstract class DirectoryConnection
{
	public X509CertificateCollection ClientCertificates
	{
		get
		{
			throw null;
		}
	}

	public virtual NetworkCredential Credential
	{
		set
		{
		}
	}

	public virtual DirectoryIdentifier Directory
	{
		get
		{
			throw null;
		}
	}

	public virtual TimeSpan Timeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public abstract DirectoryResponse SendRequest(DirectoryRequest request);
}
