namespace System.Xml.Linq;

public class XObjectChangeEventArgs : EventArgs
{
	public static readonly XObjectChangeEventArgs Add;

	public static readonly XObjectChangeEventArgs Name;

	public static readonly XObjectChangeEventArgs Remove;

	public static readonly XObjectChangeEventArgs Value;

	public XObjectChange ObjectChange
	{
		get
		{
			throw null;
		}
	}

	public XObjectChangeEventArgs(XObjectChange objectChange)
	{
	}
}
