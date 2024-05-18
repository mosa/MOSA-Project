using System.Runtime.Serialization;

namespace System.ComponentModel;

public class LicenseException : SystemException
{
	public Type? LicensedType
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected LicenseException(SerializationInfo info, StreamingContext context)
	{
	}

	public LicenseException(Type? type)
	{
	}

	public LicenseException(Type? type, object? instance)
	{
	}

	public LicenseException(Type? type, object? instance, string? message)
	{
	}

	public LicenseException(Type? type, object? instance, string? message, Exception? innerException)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
