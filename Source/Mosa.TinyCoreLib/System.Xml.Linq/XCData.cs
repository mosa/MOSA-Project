using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq;

public class XCData : XText
{
	public override XmlNodeType NodeType
	{
		get
		{
			throw null;
		}
	}

	public XCData(string value)
		: base((string)null)
	{
	}

	public XCData(XCData other)
		: base((string)null)
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
