using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Resources;

public class MissingSatelliteAssemblyException : SystemException
{
	public string? CultureName
	{
		get
		{
			throw null;
		}
	}

	public MissingSatelliteAssemblyException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected MissingSatelliteAssemblyException(SerializationInfo info, StreamingContext context)
	{
	}

	public MissingSatelliteAssemblyException(string? message)
	{
	}

	public MissingSatelliteAssemblyException(string? message, Exception? inner)
	{
	}

	public MissingSatelliteAssemblyException(string? message, string? cultureName)
	{
	}
}
