namespace System.Xml;

public class XmlNodeChangedEventArgs : EventArgs
{
	public XmlNodeChangedAction Action
	{
		get
		{
			throw null;
		}
	}

	public XmlNode? NewParent
	{
		get
		{
			throw null;
		}
	}

	public string? NewValue
	{
		get
		{
			throw null;
		}
	}

	public XmlNode? Node
	{
		get
		{
			throw null;
		}
	}

	public XmlNode? OldParent
	{
		get
		{
			throw null;
		}
	}

	public string? OldValue
	{
		get
		{
			throw null;
		}
	}

	public XmlNodeChangedEventArgs(XmlNode? node, XmlNode? oldParent, XmlNode? newParent, string? oldValue, string? newValue, XmlNodeChangedAction action)
	{
	}
}
