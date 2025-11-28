using System.Diagnostics.CodeAnalysis;

namespace System.Text;

public sealed class EncoderReplacementFallback : EncoderFallback
{
	public string DefaultString
	{
		get
		{
			throw null;
		}
	}

	public override int MaxCharCount
	{
		get
		{
			throw null;
		}
	}

	public EncoderReplacementFallback()
	{
	}

	public EncoderReplacementFallback(string replacement)
	{
	}

	public override EncoderFallbackBuffer CreateFallbackBuffer()
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
