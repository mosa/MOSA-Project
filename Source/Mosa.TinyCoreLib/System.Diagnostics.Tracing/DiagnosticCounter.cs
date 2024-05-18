using System.Runtime.Versioning;

namespace System.Diagnostics.Tracing;

[UnsupportedOSPlatform("browser")]
public abstract class DiagnosticCounter : IDisposable
{
	public string DisplayName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string DisplayUnits
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EventSource EventSource
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	internal DiagnosticCounter()
	{
	}

	public void AddMetadata(string key, string? value)
	{
	}

	public void Dispose()
	{
	}
}
