using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Security.Principal;

public sealed class IdentityNotMappedException : SystemException
{
	public IdentityReferenceCollection UnmappedIdentities
	{
		get
		{
			throw null;
		}
	}

	public IdentityNotMappedException()
	{
	}

	public IdentityNotMappedException(string? message)
	{
	}

	public IdentityNotMappedException(string? message, Exception? inner)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
	}
}
