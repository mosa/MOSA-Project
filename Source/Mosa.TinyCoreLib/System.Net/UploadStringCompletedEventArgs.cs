using System.ComponentModel;

namespace System.Net;

public class UploadStringCompletedEventArgs : AsyncCompletedEventArgs
{
	public string Result
	{
		get
		{
			throw null;
		}
	}

	internal UploadStringCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
