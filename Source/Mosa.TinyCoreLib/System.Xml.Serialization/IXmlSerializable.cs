using System.ComponentModel;
using System.Xml.Schema;

namespace System.Xml.Serialization;

public interface IXmlSerializable
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	XmlSchema? GetSchema();

	void ReadXml(XmlReader reader);

	void WriteXml(XmlWriter writer);
}
