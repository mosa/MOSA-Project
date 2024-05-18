namespace System.Net.Quic;

public abstract class QuicConnectionOptions
{
	public long DefaultCloseErrorCode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long DefaultStreamErrorCode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan IdleTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MaxInboundBidirectionalStreams
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MaxInboundUnidirectionalStreams
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal QuicConnectionOptions()
	{
	}
}
