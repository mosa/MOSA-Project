using System.Collections.Generic;

namespace System.Diagnostics;

public class StackTrace
{
	public const int METHODS_TO_SKIP = 0;

	public virtual int FrameCount
	{
		get
		{
			throw null;
		}
	}

	public StackTrace()
	{
	}

	public StackTrace(bool fNeedFileInfo)
	{
	}

	public StackTrace(IEnumerable<StackFrame> frames)
	{
	}

	public StackTrace(StackFrame frame)
	{
	}

	public StackTrace(Exception e)
	{
	}

	public StackTrace(Exception e, bool fNeedFileInfo)
	{
	}

	public StackTrace(Exception e, int skipFrames)
	{
	}

	public StackTrace(Exception e, int skipFrames, bool fNeedFileInfo)
	{
	}

	public StackTrace(int skipFrames)
	{
	}

	public StackTrace(int skipFrames, bool fNeedFileInfo)
	{
	}

	public virtual StackFrame? GetFrame(int index)
	{
		throw null;
	}

	public virtual StackFrame[] GetFrames()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
