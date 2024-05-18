using System.ComponentModel;
using System.IO;

namespace System.Net;

public class OpenWriteCompletedEventArgs : AsyncCompletedEventArgs
{
	public Stream Result
	{
		get
		{
			throw null;
		}
	}

	internal OpenWriteCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
