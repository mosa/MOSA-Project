using System.Xml;

namespace System.Configuration;

public class NameValueSectionHandler : IConfigurationSectionHandler
{
	protected virtual string KeyAttributeName
	{
		get
		{
			throw null;
		}
	}

	protected virtual string ValueAttributeName
	{
		get
		{
			throw null;
		}
	}

	public object Create(object parent, object context, XmlNode section)
	{
		throw null;
	}
}
