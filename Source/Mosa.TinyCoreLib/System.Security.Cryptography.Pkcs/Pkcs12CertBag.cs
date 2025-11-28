using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs12CertBag : Pkcs12SafeBag
{
	public ReadOnlyMemory<byte> EncodedCertificate
	{
		get
		{
			throw null;
		}
	}

	public bool IsX509Certificate
	{
		get
		{
			throw null;
		}
	}

	public Pkcs12CertBag(Oid certificateType, ReadOnlyMemory<byte> encodedCertificate)
		: base(null, default(ReadOnlyMemory<byte>))
	{
	}

	public X509Certificate2 GetCertificate()
	{
		throw null;
	}

	public Oid GetCertificateType()
	{
		throw null;
	}
}
