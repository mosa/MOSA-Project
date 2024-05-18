using System.ComponentModel;

namespace System.Net;

public class UploadProgressChangedEventArgs : ProgressChangedEventArgs
{
	public long BytesReceived
	{
		get
		{
			throw null;
		}
	}

	public long BytesSent
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

	public long TotalBytesToSend
	{
		get
		{
			throw null;
		}
	}

	internal UploadProgressChangedEventArgs()
		: base(0, null)
	{
	}
}
