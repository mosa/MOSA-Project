using System.ComponentModel;

namespace System.Net;

[EditorBrowsable(EditorBrowsableState.Never)]
public class WriteStreamClosedEventArgs : EventArgs
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	public Exception? Error
	{
		get
		{
			throw null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	public WriteStreamClosedEventArgs()
	{
	}
}
