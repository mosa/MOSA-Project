using System.Diagnostics.CodeAnalysis;

namespace System.Text;

public sealed class EncoderExceptionFallback : EncoderFallback
{
	public override int MaxCharCount
	{
		get
		{
			throw null;
		}
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
