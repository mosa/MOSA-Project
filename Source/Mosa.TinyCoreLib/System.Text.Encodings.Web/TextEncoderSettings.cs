using System.Collections.Generic;
using System.Text.Unicode;

namespace System.Text.Encodings.Web;

public class TextEncoderSettings
{
	public TextEncoderSettings()
	{
	}

	public TextEncoderSettings(TextEncoderSettings other)
	{
	}

	public TextEncoderSettings(params UnicodeRange[] allowedRanges)
	{
	}

	public virtual void AllowCharacter(char character)
	{
	}

	public virtual void AllowCharacters(params char[] characters)
	{
	}

	public virtual void AllowCodePoints(IEnumerable<int> codePoints)
	{
	}

	public virtual void AllowRange(UnicodeRange range)
	{
	}

	public virtual void AllowRanges(params UnicodeRange[] ranges)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual void ForbidCharacter(char character)
	{
	}

	public virtual void ForbidCharacters(params char[] characters)
	{
	}

	public virtual void ForbidRange(UnicodeRange range)
	{
	}

	public virtual void ForbidRanges(params UnicodeRange[] ranges)
	{
	}

	public virtual IEnumerable<int> GetAllowedCodePoints()
	{
		throw null;
	}
}
