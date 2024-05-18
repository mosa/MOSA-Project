using System.Xml;

namespace System.ServiceModel.Syndication;

public class TextSyndicationContent : SyndicationContent
{
	public string Text
	{
		get
		{
			throw null;
		}
	}

	public override string Type
	{
		get
		{
			throw null;
		}
	}

	protected TextSyndicationContent(TextSyndicationContent source)
	{
	}

	public TextSyndicationContent(string text)
	{
	}

	public TextSyndicationContent(string text, TextSyndicationContentKind textKind)
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
