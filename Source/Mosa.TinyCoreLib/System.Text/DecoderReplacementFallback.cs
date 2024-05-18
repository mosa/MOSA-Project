using System.Diagnostics.CodeAnalysis;

namespace System.Text;

public sealed class DecoderReplacementFallback : DecoderFallback
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

	public DecoderReplacementFallback()
	{
	}

	public DecoderReplacementFallback(string replacement)
	{
	}

	public override DecoderFallbackBuffer CreateFallbackBuffer()
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
