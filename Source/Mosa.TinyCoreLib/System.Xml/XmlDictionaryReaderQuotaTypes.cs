namespace System.Xml;

[Flags]
public enum XmlDictionaryReaderQuotaTypes
{
	MaxDepth = 1,
	MaxStringContentLength = 2,
	MaxArrayLength = 4,
	MaxBytesPerRead = 8,
	MaxNameTableCharCount = 0x10
}
