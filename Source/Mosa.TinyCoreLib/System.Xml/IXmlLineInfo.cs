namespace System.Xml;

public interface IXmlLineInfo
{
	int LineNumber { get; }

	int LinePosition { get; }

	bool HasLineInfo();
}
