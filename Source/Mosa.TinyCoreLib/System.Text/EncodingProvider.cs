using System.Collections.Generic;

namespace System.Text;

public abstract class EncodingProvider
{
	public EncodingProvider()
	{
	}

	public abstract Encoding? GetEncoding(int codepage);

	public virtual Encoding? GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
	{
		throw null;
	}

	public abstract Encoding? GetEncoding(string name);

	public virtual Encoding? GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
	{
		throw null;
	}

	public virtual IEnumerable<EncodingInfo> GetEncodings()
	{
		throw null;
	}
}
