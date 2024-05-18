namespace System.Xml.Serialization;

public interface IXmlTextParser
{
	bool Normalized { get; set; }

	WhitespaceHandling WhitespaceHandling { get; set; }
}
