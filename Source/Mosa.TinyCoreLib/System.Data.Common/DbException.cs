using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Data.Common;

public abstract class DbException : ExternalException
{
	public virtual bool IsTransient
	{
		get
		{
			throw null;
		}
	}

	public virtual string? SqlState
	{
		get
		{
			throw null;
		}
	}

	public DbBatchCommand? BatchCommand
	{
		get
		{
			throw null;
		}
	}

	protected virtual DbBatchCommand? DbBatchCommand
	{
		get
		{
			throw null;
		}
	}

	protected DbException()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected DbException(SerializationInfo info, StreamingContext context)
	{
	}

	protected DbException(string? message)
	{
	}

	protected DbException(string? message, Exception? innerException)
	{
	}

	protected DbException(string? message, int errorCode)
	{
	}
}
