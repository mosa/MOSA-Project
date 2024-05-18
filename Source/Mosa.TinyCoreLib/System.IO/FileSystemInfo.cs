using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

namespace System.IO;

public abstract class FileSystemInfo : MarshalByRefObject, ISerializable
{
	protected string FullPath;

	protected string OriginalPath;

	public FileAttributes Attributes
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime CreationTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime CreationTimeUtc
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public abstract bool Exists { get; }

	public string Extension
	{
		get
		{
			throw null;
		}
	}

	public virtual string FullName
	{
		get
		{
			throw null;
		}
	}

	public DateTime LastAccessTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime LastAccessTimeUtc
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime LastWriteTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTime LastWriteTimeUtc
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? LinkTarget
	{
		get
		{
			throw null;
		}
	}

	public abstract string Name { get; }

	public UnixFileMode UnixFileMode
	{
		get
		{
			throw null;
		}
		[UnsupportedOSPlatform("windows")]
		set
		{
		}
	}

	protected FileSystemInfo()
	{
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected FileSystemInfo(SerializationInfo info, StreamingContext context)
	{
	}

	public void CreateAsSymbolicLink(string pathToTarget)
	{
	}

	public abstract void Delete();

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public void Refresh()
	{
	}

	public FileSystemInfo? ResolveLinkTarget(bool returnFinalTarget)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
