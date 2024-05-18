using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq;

public class XComment : XNode
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

	public XComment(string value)
	{
	}

	public XComment(XComment other)
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
