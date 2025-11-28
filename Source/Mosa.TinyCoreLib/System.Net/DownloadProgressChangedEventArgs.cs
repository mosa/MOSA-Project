using System.ComponentModel;

namespace System.Net;

public class DownloadProgressChangedEventArgs : ProgressChangedEventArgs
{
	public long BytesReceived
	{
		get
		{
			throw null;
		}
	}

	public long TotalBytesToReceive
	{
		get
		{
			throw null;
		}
	}

	internal DownloadProgressChangedEventArgs()
		: base(0, null)
	{
	}
}
