using System.Xml;

namespace System.Security.Cryptography.Xml;

public abstract class KeyInfoClause
{
	public abstract XmlElement GetXml();

	public abstract void LoadXml(XmlElement element);
}
