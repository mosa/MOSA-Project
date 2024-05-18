using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Security;

public class SecurityException : SystemException
{
	public object? Demanded
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object? DenySetInstance
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AssemblyName? FailedAssemblyInfo
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? GrantedSet
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MethodInfo? Method
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? PermissionState
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Type? PermissionType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object? PermitOnlySetInstance
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? RefusedSet
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Url
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SecurityException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected SecurityException(SerializationInfo info, StreamingContext context)
	{
	}

	public SecurityException(string? message)
	{
	}

	public SecurityException(string? message, Exception? inner)
	{
	}

	public SecurityException(string? message, Type? type)
	{
	}

	public SecurityException(string? message, Type? type, string? state)
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
