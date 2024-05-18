using System.Xml;

namespace System.Configuration;

public class SingleTagSectionHandler : IConfigurationSectionHandler
{
	public virtual object Create(object parent, object context, XmlNode section)
	{
		throw null;
	}
}
