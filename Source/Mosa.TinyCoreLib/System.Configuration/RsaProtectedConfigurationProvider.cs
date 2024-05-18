using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Xml;

namespace System.Configuration;

public sealed class RsaProtectedConfigurationProvider : ProtectedConfigurationProvider
{
	public string CspProviderName
	{
		get
		{
			throw null;
		}
	}

	public string KeyContainerName
	{
		get
		{
			throw null;
		}
	}

	public RSAParameters RsaPublicKey
	{
		get
		{
			throw null;
		}
	}

	public bool UseFIPS
	{
		get
		{
			throw null;
		}
	}

	public bool UseMachineContainer
	{
		get
		{
			throw null;
		}
	}

	public bool UseOAEP
	{
		get
		{
			throw null;
		}
	}

	public void AddKey(int keySize, bool exportable)
	{
	}

	public override XmlNode Decrypt(XmlNode encryptedNode)
	{
		throw null;
	}

	public void DeleteKey()
	{
	}

	public override XmlNode Encrypt(XmlNode node)
	{
		throw null;
	}

	public void ExportKey(string xmlFileName, bool includePrivateParameters)
	{
	}

	public void ImportKey(string xmlFileName, bool exportable)
	{
	}

	public override void Initialize(string name, NameValueCollection configurationValues)
	{
	}
}
