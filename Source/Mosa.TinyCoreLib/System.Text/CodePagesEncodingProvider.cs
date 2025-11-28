using System.Collections.Generic;

namespace System.Text;

public sealed class CodePagesEncodingProvider : EncodingProvider
{
	public static EncodingProvider Instance
	{
		get
		{
			throw null;
		}
	}

	internal CodePagesEncodingProvider()
	{
	}

	public override Encoding? GetEncoding(int codepage)
	{
		throw null;
	}

	public override Encoding? GetEncoding(string name)
	{
		throw null;
	}

	public override IEnumerable<EncodingInfo> GetEncodings()
	{
		throw null;
	}
}
