using System.Text.Unicode;

namespace System.Text.Encodings.Web;

public abstract class JavaScriptEncoder : TextEncoder
{
	public static JavaScriptEncoder Default
	{
		get
		{
			throw null;
		}
	}

	public static JavaScriptEncoder UnsafeRelaxedJsonEscaping
	{
		get
		{
			throw null;
		}
	}

	public static JavaScriptEncoder Create(TextEncoderSettings settings)
	{
		throw null;
	}

	public static JavaScriptEncoder Create(params UnicodeRange[] allowedRanges)
	{
		throw null;
	}
}
