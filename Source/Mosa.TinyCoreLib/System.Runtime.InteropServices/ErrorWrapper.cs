using System.ComponentModel;

namespace System.Runtime.InteropServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ErrorWrapper
{
	public int ErrorCode
	{
		get
		{
			throw null;
		}
	}

	public ErrorWrapper(Exception e)
	{
	}

	public ErrorWrapper(int errorCode)
	{
	}

	public ErrorWrapper(object errorCode)
	{
	}
}
