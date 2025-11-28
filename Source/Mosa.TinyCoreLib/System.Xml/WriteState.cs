namespace System.Xml;

public enum WriteState
{
	Start,
	Prolog,
	Element,
	Attribute,
	Content,
	Closed,
	Error
}
