using System.ComponentModel;

namespace System.Net;

public class UploadValuesCompletedEventArgs : AsyncCompletedEventArgs
{
	public byte[] Result
	{
		get
		{
			throw null;
		}
	}

	internal UploadValuesCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
