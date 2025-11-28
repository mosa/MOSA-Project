using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public class KeyInfoX509Data : KeyInfoClause
{
	public ArrayList? Certificates
	{
		get
		{
			throw null;
		}
	}

	public byte[]? CRL
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ArrayList? IssuerSerials
	{
		get
		{
			throw null;
		}
	}

	public ArrayList? SubjectKeyIds
	{
		get
		{
			throw null;
		}
	}

	public ArrayList? SubjectNames
	{
		get
		{
			throw null;
		}
	}

	public KeyInfoX509Data()
	{
	}

	public KeyInfoX509Data(byte[] rgbCert)
	{
	}

	public KeyInfoX509Data(X509Certificate cert)
	{
	}

	public KeyInfoX509Data(X509Certificate cert, X509IncludeOption includeOption)
	{
	}

	public void AddCertificate(X509Certificate certificate)
	{
	}

	public void AddIssuerSerial(string issuerName, string serialNumber)
	{
	}

	public void AddSubjectKeyId(byte[] subjectKeyId)
	{
	}

	public void AddSubjectKeyId(string subjectKeyId)
	{
	}

	public void AddSubjectName(string subjectName)
	{
	}

	public override XmlElement GetXml()
	{
		throw null;
	}

	public override void LoadXml(XmlElement element)
	{
	}
}
