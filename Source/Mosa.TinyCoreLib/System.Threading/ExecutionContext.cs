using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading;

public sealed class ExecutionContext : IDisposable, ISerializable
{
	internal ExecutionContext()
	{
	}

	public static ExecutionContext? Capture()
	{
		throw null;
	}

	public ExecutionContext CreateCopy()
	{
		throw null;
	}

	public void Dispose()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static bool IsFlowSuppressed()
	{
		throw null;
	}

	public static void Restore(ExecutionContext executionContext)
	{
	}

	public static void RestoreFlow()
	{
	}

	public static void Run(ExecutionContext executionContext, ContextCallback callback, object? state)
	{
	}

	public static AsyncFlowControl SuppressFlow()
	{
		throw null;
	}
}
