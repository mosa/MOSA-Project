using System.Xml;

namespace System.Configuration;

public interface IConfigurationSectionHandler
{
	object Create(object parent, object configContext, XmlNode section);
}
