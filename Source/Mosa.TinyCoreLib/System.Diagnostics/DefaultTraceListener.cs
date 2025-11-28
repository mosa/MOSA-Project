namespace System.Diagnostics;

public class DefaultTraceListener : TraceListener
{
	public bool AssertUiEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? LogFileName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override void Fail(string? message)
	{
	}

	public override void Fail(string? message, string? detailMessage)
	{
	}

	public override void Write(string? message)
	{
	}

	public override void WriteLine(string? message)
	{
	}
}
