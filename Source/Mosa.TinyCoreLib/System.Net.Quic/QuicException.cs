using System.IO;

namespace System.Net.Quic;

public sealed class QuicException : IOException
{
	public long? ApplicationErrorCode
	{
		get
		{
			throw null;
		}
	}

	public long? TransportErrorCode
	{
		get
		{
			throw null;
		}
	}

	public QuicError QuicError
	{
		get
		{
			throw null;
		}
	}

	public QuicException(QuicError error, long? applicationErrorCode, string message)
	{
	}
}
