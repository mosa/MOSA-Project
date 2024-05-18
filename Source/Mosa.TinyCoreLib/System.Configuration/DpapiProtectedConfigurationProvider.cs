using System.Collections.Specialized;
using System.Runtime.Versioning;
using System.Xml;

namespace System.Configuration;

[SupportedOSPlatform("windows")]
public sealed class DpapiProtectedConfigurationProvider : ProtectedConfigurationProvider
{
	public bool UseMachineProtection
	{
		get
		{
			throw null;
		}
	}

	public override XmlNode Decrypt(XmlNode encryptedNode)
	{
		throw null;
	}

	public override XmlNode Encrypt(XmlNode node)
	{
		throw null;
	}

	public override void Initialize(string name, NameValueCollection configurationValues)
	{
	}
}
