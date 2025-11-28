using System.Xml;

namespace System.Configuration;

public class DictionarySectionHandler : IConfigurationSectionHandler
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

	public virtual object Create(object parent, object context, XmlNode section)
	{
		throw null;
	}
}
