using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Threading;

public sealed class CompressedStack : ISerializable
{
	internal CompressedStack()
	{
	}

	public static CompressedStack Capture()
	{
		throw null;
	}

	public CompressedStack CreateCopy()
	{
		throw null;
	}

	public static CompressedStack GetCompressedStack()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static void Run(CompressedStack compressedStack, ContextCallback callback, object? state)
	{
	}
}
