using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq;

public class XDocumentType : XNode
{
	public string? InternalSubset
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
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

	public string? PublicId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? SystemId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XDocumentType(string name, string? publicId, string? systemId, string? internalSubset)
	{
	}

	public XDocumentType(XDocumentType other)
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
