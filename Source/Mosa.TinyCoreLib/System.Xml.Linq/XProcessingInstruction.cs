using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq;

public class XProcessingInstruction : XNode
{
	public string Data
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override XmlNodeType NodeType
	{
		get
		{
			throw null;
		}
	}

	public string Target
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XProcessingInstruction(string target, string data)
	{
	}

	public XProcessingInstruction(XProcessingInstruction other)
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
