using System.Configuration.Provider;
using System.Xml;

namespace System.Configuration;

public abstract class ProtectedConfigurationProvider : ProviderBase
{
	public abstract XmlNode Decrypt(XmlNode encryptedNode);

	public abstract XmlNode Encrypt(XmlNode node);
}
