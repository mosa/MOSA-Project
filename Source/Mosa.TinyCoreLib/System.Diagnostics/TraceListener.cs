using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public abstract class TraceListener : MarshalByRefObject, IDisposable
{
	public StringDictionary Attributes
	{
		get
		{
			throw null;
		}
	}

	public TraceFilter? Filter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int IndentLevel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int IndentSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool IsThreadSafe
	{
		get
		{
			throw null;
		}
	}

	public virtual string Name
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	protected bool NeedIndent
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TraceOptions TraceOutputOptions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected TraceListener()
	{
	}

	protected TraceListener(string? name)
	{
	}

	public virtual void Close()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual void Fail(string? message)
	{
	}

	public virtual void Fail(string? message, string? detailMessage)
	{
	}

	public virtual void Flush()
	{
	}

	protected virtual string[]? GetSupportedAttributes()
	{
		throw null;
	}

	public virtual void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, object? data)
	{
	}

	public virtual void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, params object?[]? data)
	{
	}

	public virtual void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id)
	{
	}

	public virtual void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
	{
	}

	public virtual void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, [StringSyntax("CompositeFormat")] string? format, params object?[]? args)
	{
	}

	public virtual void TraceTransfer(TraceEventCache? eventCache, string source, int id, string? message, Guid relatedActivityId)
	{
	}

	public virtual void Write(object? o)
	{
	}

	public virtual void Write(object? o, string? category)
	{
	}

	public abstract void Write(string? message);

	public virtual void Write(string? message, string? category)
	{
	}

	protected virtual void WriteIndent()
	{
	}

	public virtual void WriteLine(object? o)
	{
	}

	public virtual void WriteLine(object? o, string? category)
	{
	}

	public abstract void WriteLine(string? message);

	public virtual void WriteLine(string? message, string? category)
	{
	}
}
