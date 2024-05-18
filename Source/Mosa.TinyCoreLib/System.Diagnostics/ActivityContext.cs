using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Diagnostics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct ActivityContext : IEquatable<ActivityContext>
{
	public ActivityTraceId TraceId
	{
		get
		{
			throw null;
		}
	}

	public ActivitySpanId SpanId
	{
		get
		{
			throw null;
		}
	}

	public ActivityTraceFlags TraceFlags
	{
		get
		{
			throw null;
		}
	}

	public string? TraceState
	{
		get
		{
			throw null;
		}
	}

	public bool IsRemote
	{
		get
		{
			throw null;
		}
	}

	public ActivityContext(ActivityTraceId traceId, ActivitySpanId spanId, ActivityTraceFlags traceFlags, string? traceState = null, bool isRemote = false)
	{
		throw null;
	}

	public static bool TryParse(string? traceParent, string? traceState, out ActivityContext context)
	{
		throw null;
	}

	public static bool TryParse(string? traceParent, string? traceState, bool isRemote, out ActivityContext context)
	{
		throw null;
	}

	public static ActivityContext Parse(string traceParent, string? traceState)
	{
		throw null;
	}

	public static bool operator ==(ActivityContext left, ActivityContext right)
	{
		throw null;
	}

	public static bool operator !=(ActivityContext left, ActivityContext right)
	{
		throw null;
	}

	public bool Equals(ActivityContext value)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
