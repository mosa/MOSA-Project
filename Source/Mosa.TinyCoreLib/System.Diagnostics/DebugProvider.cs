using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public class DebugProvider
{
	[DoesNotReturn]
	public virtual void Fail(string? message, string? detailMessage)
	{
		throw null;
	}

	public static void FailCore(string stackTrace, string? message, string? detailMessage, string errorSource)
	{
	}

	public virtual void OnIndentLevelChanged(int indentLevel)
	{
	}

	public virtual void OnIndentSizeChanged(int indentSize)
	{
	}

	public virtual void Write(string? message)
	{
	}

	public static void WriteCore(string message)
	{
	}

	public virtual void WriteLine(string? message)
	{
	}
}
