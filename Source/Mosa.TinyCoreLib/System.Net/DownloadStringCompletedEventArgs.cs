using System.ComponentModel;

namespace System.Net;

public class DownloadStringCompletedEventArgs : AsyncCompletedEventArgs
{
	public string Result
	{
		get
		{
			throw null;
		}
	}

	internal DownloadStringCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
