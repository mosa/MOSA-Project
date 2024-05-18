using System.Text.Unicode;

namespace System.Text.Encodings.Web;

public abstract class UrlEncoder : TextEncoder
{
	public static UrlEncoder Default
	{
		get
		{
			throw null;
		}
	}

	public static UrlEncoder Create(TextEncoderSettings settings)
	{
		throw null;
	}

	public static UrlEncoder Create(params UnicodeRange[] allowedRanges)
	{
		throw null;
	}
}
