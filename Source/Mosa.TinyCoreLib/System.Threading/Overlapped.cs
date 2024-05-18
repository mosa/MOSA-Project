namespace System.Threading;

public class Overlapped
{
	public IAsyncResult? AsyncResult
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("Overlapped.EventHandle is not 64-bit compatible and has been deprecated. Use EventHandleIntPtr instead.")]
	public int EventHandle
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IntPtr EventHandleIntPtr
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int OffsetHigh
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int OffsetLow
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Overlapped()
	{
	}

	[Obsolete("This constructor is not 64-bit compatible and has been deprecated. Use the constructor that accepts an IntPtr for the event handle instead.")]
	public Overlapped(int offsetLo, int offsetHi, int hEvent, IAsyncResult? ar)
	{
	}

	public Overlapped(int offsetLo, int offsetHi, IntPtr hEvent, IAsyncResult? ar)
	{
	}

	[CLSCompliant(false)]
	public unsafe static void Free(NativeOverlapped* nativeOverlappedPtr)
	{
	}

	[CLSCompliant(false)]
	[Obsolete("This overload is not safe and has been deprecated. Use Pack(IOCompletionCallback?, object?) instead.")]
	public unsafe NativeOverlapped* Pack(IOCompletionCallback? iocb)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe NativeOverlapped* Pack(IOCompletionCallback? iocb, object? userData)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static Overlapped Unpack(NativeOverlapped* nativeOverlappedPtr)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[Obsolete("This overload is not safe and has been deprecated. Use UnsafePack(IOCompletionCallback?, object?) instead.")]
	public unsafe NativeOverlapped* UnsafePack(IOCompletionCallback? iocb)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe NativeOverlapped* UnsafePack(IOCompletionCallback? iocb, object? userData)
	{
		throw null;
	}
}
