using System.ComponentModel;
using System.IO;

namespace System.Net;

public class OpenReadCompletedEventArgs : AsyncCompletedEventArgs
{
	public Stream Result
	{
		get
		{
			throw null;
		}
	}

	internal OpenReadCompletedEventArgs()
		: base(null, cancelled: false, null)
	{
	}
}
