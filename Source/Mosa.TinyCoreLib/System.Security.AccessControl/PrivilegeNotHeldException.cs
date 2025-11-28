using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Security.AccessControl;

public sealed class PrivilegeNotHeldException : UnauthorizedAccessException, ISerializable
{
	public string? PrivilegeName
	{
		get
		{
			throw null;
		}
	}

	public PrivilegeNotHeldException()
	{
	}

	public PrivilegeNotHeldException(string? privilege)
	{
	}

	public PrivilegeNotHeldException(string? privilege, Exception? inner)
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
