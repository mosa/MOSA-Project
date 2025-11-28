using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

namespace System.IO;

public sealed class DriveInfo : ISerializable
{
	public long AvailableFreeSpace
	{
		get
		{
			throw null;
		}
	}

	public string DriveFormat
	{
		get
		{
			throw null;
		}
	}

	public DriveType DriveType
	{
		get
		{
			throw null;
		}
	}

	public bool IsReady
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public DirectoryInfo RootDirectory
	{
		get
		{
			throw null;
		}
	}

	public long TotalFreeSpace
	{
		get
		{
			throw null;
		}
	}

	public long TotalSize
	{
		get
		{
			throw null;
		}
	}

	public string VolumeLabel
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		[param: AllowNull]
		set
		{
		}
	}

	public DriveInfo(string driveName)
	{
	}

	public static DriveInfo[] GetDrives()
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
