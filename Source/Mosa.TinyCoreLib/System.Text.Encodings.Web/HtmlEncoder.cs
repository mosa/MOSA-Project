using System.Text.Unicode;

namespace System.Text.Encodings.Web;

public abstract class HtmlEncoder : TextEncoder
{
	public static HtmlEncoder Default
	{
		get
		{
			throw null;
		}
	}

	public static HtmlEncoder Create(TextEncoderSettings settings)
	{
		throw null;
	}

	public static HtmlEncoder Create(params UnicodeRange[] allowedRanges)
	{
		throw null;
	}
}
