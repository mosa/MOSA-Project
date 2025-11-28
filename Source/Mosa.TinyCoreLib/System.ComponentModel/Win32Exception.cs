using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.ComponentModel;

public class Win32Exception : ExternalException
{
	public int NativeErrorCode
	{
		get
		{
			throw null;
		}
	}

	public Win32Exception()
	{
	}

	public Win32Exception(int error)
	{
	}

	public Win32Exception(int error, string? message)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected Win32Exception(SerializationInfo info, StreamingContext context)
	{
	}

	public Win32Exception(string? message)
	{
	}

	public Win32Exception(string? message, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
