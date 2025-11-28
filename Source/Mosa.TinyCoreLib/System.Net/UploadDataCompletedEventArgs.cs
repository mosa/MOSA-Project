using System.ComponentModel;

namespace System.Net;

public class UploadDataCompletedEventArgs : AsyncCompletedEventArgs
{
	public byte[] Result
	{
		get
		{
			throw null;
		}
	}

	internal UploadDataCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
