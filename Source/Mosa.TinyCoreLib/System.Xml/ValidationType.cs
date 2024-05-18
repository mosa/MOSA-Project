namespace System.Xml;

public enum ValidationType
{
	None,
	[Obsolete("ValidationType.Auto has been deprecated. Use DTD or Schema instead.")]
	Auto,
	DTD,
	[Obsolete("XDR Validation through XmlValidatingReader has been deprecated and is not supported.")]
	XDR,
	Schema
}
