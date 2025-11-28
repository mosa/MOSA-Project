using System.Xml;

namespace System.ServiceModel.Syndication;

public class UrlSyndicationContent : SyndicationContent
{
	public override string Type
	{
		get
		{
			throw null;
		}
	}

	public Uri Url
	{
		get
		{
			throw null;
		}
	}

	protected UrlSyndicationContent(UrlSyndicationContent source)
	{
	}

	public UrlSyndicationContent(Uri url, string mediaType)
	{
	}

	public override SyndicationContent Clone()
	{
		throw null;
	}

	protected override void WriteContentsTo(XmlWriter writer)
	{
	}
}
