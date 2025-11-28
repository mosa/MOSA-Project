using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System.Diagnostics;

public class StackFrame
{
	public const int OFFSET_UNKNOWN = -1;

	public StackFrame()
	{
	}

	public StackFrame(bool needFileInfo)
	{
	}

	public StackFrame(int skipFrames)
	{
	}

	public StackFrame(int skipFrames, bool needFileInfo)
	{
	}

	public StackFrame(string? fileName, int lineNumber)
	{
	}

	public StackFrame(string? fileName, int lineNumber, int colNumber)
	{
	}

	public virtual int GetFileColumnNumber()
	{
		throw null;
	}

	public virtual int GetFileLineNumber()
	{
		throw null;
	}

	public virtual string? GetFileName()
	{
		throw null;
	}

	public virtual int GetILOffset()
	{
		throw null;
	}

	[RequiresUnreferencedCode("Metadata for the method might be incomplete or removed")]
	public virtual MethodBase? GetMethod()
	{
		throw null;
	}

	public virtual int GetNativeOffset()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
