using System.ComponentModel;

namespace System.Net.NetworkInformation;

public class PingCompletedEventArgs : AsyncCompletedEventArgs
{
	public PingReply? Reply
	{
		get
		{
			throw null;
		}
	}

	internal PingCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
