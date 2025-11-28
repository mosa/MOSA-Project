using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq;

public class XText : XNode
{
	public override XmlNodeType NodeType
	{
		get
		{
			throw null;
		}
	}

	public string Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XText(string value)
	{
	}

	public XText(XText other)
	{
	}

	public override void WriteTo(XmlWriter writer)
	{
	}

	public override Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken)
	{
		throw null;
	}
}
