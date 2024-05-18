using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Runtime.CompilerServices;

public sealed class RuntimeWrappedException : Exception
{
	public object WrappedException
	{
		get
		{
			throw null;
		}
	}

	public RuntimeWrappedException(object thrownObject)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
