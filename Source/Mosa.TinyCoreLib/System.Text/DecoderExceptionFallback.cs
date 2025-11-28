using System.Diagnostics.CodeAnalysis;

namespace System.Text;

public sealed class DecoderExceptionFallback : DecoderFallback
{
	public override int MaxCharCount
	{
		get
		{
			throw null;
		}
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
