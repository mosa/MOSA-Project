using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics.Tracing;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class EventSource : IDisposable
{
	public readonly struct EventSourcePrimitive
	{
		public static implicit operator EventSourcePrimitive(bool value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(byte value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(short value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(int value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(long value)
		{
			throw null;
		}

		[CLSCompliant(false)]
		public static implicit operator EventSourcePrimitive(sbyte value)
		{
			throw null;
		}

		[CLSCompliant(false)]
		public static implicit operator EventSourcePrimitive(ushort value)
		{
			throw null;
		}

		[CLSCompliant(false)]
		public static implicit operator EventSourcePrimitive(uint value)
		{
			throw null;
		}

		[CLSCompliant(false)]
		public static implicit operator EventSourcePrimitive(ulong value)
		{
			throw null;
		}

		[CLSCompliant(false)]
		public static implicit operator EventSourcePrimitive(UIntPtr value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(float value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(double value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(decimal value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(string? value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(byte[]? value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(Guid value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(DateTime value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(IntPtr value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(char value)
		{
			throw null;
		}

		public static implicit operator EventSourcePrimitive(Enum value)
		{
			throw null;
		}
	}

	protected internal struct EventData
	{
		private int _dummyPrimitive;

		public IntPtr DataPointer
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}

		public int Size
		{
			get
			{
				throw null;
			}
			set
			{
			}
		}
	}

	public Exception? ConstructionException
	{
		get
		{
			throw null;
		}
	}

	public static Guid CurrentThreadActivityId
	{
		get
		{
			throw null;
		}
	}

	public Guid Guid
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

	public EventSourceSettings Settings
	{
		get
		{
			throw null;
		}
	}

	public event EventHandler<EventCommandEventArgs>? EventCommandExecuted
	{
		add
		{
		}
		remove
		{
		}
	}

	protected EventSource()
	{
	}

	protected EventSource(bool throwOnEventWriteErrors)
	{
	}

	protected EventSource(EventSourceSettings settings)
	{
	}

	protected EventSource(EventSourceSettings settings, params string[]? traits)
	{
	}

	public EventSource(string eventSourceName)
	{
	}

	public EventSource(string eventSourceName, EventSourceSettings config)
	{
	}

	public EventSource(string eventSourceName, EventSourceSettings config, params string[]? traits)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~EventSource()
	{
	}

	public static string? GenerateManifest([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type eventSourceType, string? assemblyPathToIncludeInManifest)
	{
		throw null;
	}

	public static string? GenerateManifest([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type eventSourceType, string? assemblyPathToIncludeInManifest, EventManifestOptions flags)
	{
		throw null;
	}

	public static Guid GetGuid(Type eventSourceType)
	{
		throw null;
	}

	public static string GetName(Type eventSourceType)
	{
		throw null;
	}

	public static IEnumerable<EventSource> GetSources()
	{
		throw null;
	}

	public string? GetTrait(string key)
	{
		throw null;
	}

	public bool IsEnabled()
	{
		throw null;
	}

	public bool IsEnabled(EventLevel level, EventKeywords keywords)
	{
		throw null;
	}

	public bool IsEnabled(EventLevel level, EventKeywords keywords, EventChannel channel)
	{
		throw null;
	}

	protected virtual void OnEventCommand(EventCommandEventArgs command)
	{
	}

	public static void SendCommand(EventSource eventSource, EventCommand command, IDictionary<string, string?>? commandArguments)
	{
	}

	public static void SetCurrentThreadActivityId(Guid activityId)
	{
	}

	public static void SetCurrentThreadActivityId(Guid activityId, out Guid oldActivityThatWillContinue)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public void Write(string? eventName)
	{
	}

	public void Write(string? eventName, EventSourceOptions options)
	{
	}

	protected void WriteEvent(int eventId)
	{
	}

	protected void WriteEvent(int eventId, byte[]? arg1)
	{
	}

	protected void WriteEvent(int eventId, int arg1)
	{
	}

	protected void WriteEvent(int eventId, int arg1, int arg2)
	{
	}

	protected void WriteEvent(int eventId, int arg1, int arg2, int arg3)
	{
	}

	protected void WriteEvent(int eventId, int arg1, string? arg2)
	{
	}

	protected void WriteEvent(int eventId, long arg1)
	{
	}

	protected void WriteEvent(int eventId, long arg1, byte[]? arg2)
	{
	}

	protected void WriteEvent(int eventId, long arg1, long arg2)
	{
	}

	protected void WriteEvent(int eventId, long arg1, long arg2, long arg3)
	{
	}

	protected void WriteEvent(int eventId, long arg1, string? arg2)
	{
	}

	protected void WriteEvent(int eventId, params EventSourcePrimitive[] args)
	{
	}

	[RequiresUnreferencedCode("EventSource will serialize the whole object graph. Trimmer will not safely handle this case because properties may be trimmed. This can be suppressed if the object is a primitive type")]
	protected void WriteEvent(int eventId, params object?[] args)
	{
	}

	protected void WriteEvent(int eventId, string? arg1)
	{
	}

	protected void WriteEvent(int eventId, string? arg1, int arg2)
	{
	}

	protected void WriteEvent(int eventId, string? arg1, int arg2, int arg3)
	{
	}

	protected void WriteEvent(int eventId, string? arg1, long arg2)
	{
	}

	protected void WriteEvent(int eventId, string? arg1, string? arg2)
	{
	}

	protected void WriteEvent(int eventId, string? arg1, string? arg2, string? arg3)
	{
	}

	[RequiresUnreferencedCode("EventSource will serialize the whole object graph. Trimmer will not safely handle this case because properties may be trimmed. This can be suppressed if the object is a primitive type")]
	[CLSCompliant(false)]
	protected unsafe void WriteEventCore(int eventId, int eventDataCount, EventData* data)
	{
	}

	[RequiresUnreferencedCode("EventSource will serialize the whole object graph. Trimmer will not safely handle this case because properties may be trimmed. This can be suppressed if the object is a primitive type")]
	protected void WriteEventWithRelatedActivityId(int eventId, Guid relatedActivityId, params object?[] args)
	{
	}

	[RequiresUnreferencedCode("EventSource will serialize the whole object graph. Trimmer will not safely handle this case because properties may be trimmed. This can be suppressed if the object is a primitive type")]
	[CLSCompliant(false)]
	protected unsafe void WriteEventWithRelatedActivityIdCore(int eventId, Guid* relatedActivityId, int eventDataCount, EventData* data)
	{
	}

	[RequiresUnreferencedCode("EventSource will serialize the whole object graph. Trimmer will not safely handle this case because properties may be trimmed. This can be suppressed if the object is a primitive type")]
	public void Write<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(string? eventName, EventSourceOptions options, T data)
	{
	}

	[RequiresUnreferencedCode("EventSource will serialize the whole object graph. Trimmer will not safely handle this case because properties may be trimmed. This can be suppressed if the object is a primitive type")]
	public void Write<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(string? eventName, ref EventSourceOptions options, ref Guid activityId, ref Guid relatedActivityId, ref T data)
	{
	}

	[RequiresUnreferencedCode("EventSource will serialize the whole object graph. Trimmer will not safely handle this case because properties may be trimmed. This can be suppressed if the object is a primitive type")]
	public void Write<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(string? eventName, ref EventSourceOptions options, ref T data)
	{
	}

	[RequiresUnreferencedCode("EventSource will serialize the whole object graph. Trimmer will not safely handle this case because properties may be trimmed. This can be suppressed if the object is a primitive type")]
	public void Write<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(string? eventName, T data)
	{
	}
}
