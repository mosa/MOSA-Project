using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;

namespace System;

public class Exception : ISerializable
{
	public virtual IDictionary Data
	{
		get
		{
			throw null;
		}
	}

	public virtual string? HelpLink
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int HResult
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Exception? InnerException
	{
		get
		{
			throw null;
		}
	}

	public virtual string Message
	{
		get
		{
			throw null;
		}
	}

	public virtual string? Source
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string? StackTrace
	{
		get
		{
			throw null;
		}
	}

	public MethodBase? TargetSite
	{
		[RequiresUnreferencedCode("Metadata for the method might be incomplete or removed")]
		get
		{
			throw null;
		}
	}

	[Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", DiagnosticId = "SYSLIB0011", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected event EventHandler<SafeSerializationEventArgs>? SerializeObjectState
	{
		add
		{
		}
		remove
		{
		}
	}

	public Exception()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected Exception(SerializationInfo info, StreamingContext context)
	{
	}

	public Exception(string? message)
	{
	}

	public Exception(string? message, Exception? innerException)
	{
	}

	public virtual Exception GetBaseException()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public new Type GetType()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
