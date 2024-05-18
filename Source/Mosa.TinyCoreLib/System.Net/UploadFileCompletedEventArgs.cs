using System.ComponentModel;

namespace System.Net;

public class UploadFileCompletedEventArgs : AsyncCompletedEventArgs
{
	public byte[] Result
	{
		get
		{
			throw null;
		}
	}

	internal UploadFileCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
