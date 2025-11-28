using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;

namespace System.IO.MemoryMappedFiles;

public class MemoryMappedFile : IDisposable
{
	public SafeMemoryMappedFileHandle SafeMemoryMappedFileHandle
	{
		get
		{
			throw null;
		}
	}

	internal MemoryMappedFile()
	{
	}

	public static MemoryMappedFile CreateFromFile(FileStream fileStream, string? mapName, long capacity, MemoryMappedFileAccess access, HandleInheritability inheritability, bool leaveOpen)
	{
		throw null;
	}

	public static MemoryMappedFile CreateFromFile(SafeFileHandle fileHandle, string? mapName, long capacity, MemoryMappedFileAccess access, HandleInheritability inheritability, bool leaveOpen)
	{
		throw null;
	}

	public static MemoryMappedFile CreateFromFile(string path)
	{
		throw null;
	}

	public static MemoryMappedFile CreateFromFile(string path, FileMode mode)
	{
		throw null;
	}

	public static MemoryMappedFile CreateFromFile(string path, FileMode mode, string? mapName)
	{
		throw null;
	}

	public static MemoryMappedFile CreateFromFile(string path, FileMode mode, string? mapName, long capacity)
	{
		throw null;
	}

	public static MemoryMappedFile CreateFromFile(string path, FileMode mode, string? mapName, long capacity, MemoryMappedFileAccess access)
	{
		throw null;
	}

	public static MemoryMappedFile CreateNew(string? mapName, long capacity)
	{
		throw null;
	}

	public static MemoryMappedFile CreateNew(string? mapName, long capacity, MemoryMappedFileAccess access)
	{
		throw null;
	}

	public static MemoryMappedFile CreateNew(string? mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, HandleInheritability inheritability)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static MemoryMappedFile CreateOrOpen(string mapName, long capacity)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static MemoryMappedFile CreateOrOpen(string mapName, long capacity, MemoryMappedFileAccess access)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static MemoryMappedFile CreateOrOpen(string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, HandleInheritability inheritability)
	{
		throw null;
	}

	public MemoryMappedViewAccessor CreateViewAccessor()
	{
		throw null;
	}

	public MemoryMappedViewAccessor CreateViewAccessor(long offset, long size)
	{
		throw null;
	}

	public MemoryMappedViewAccessor CreateViewAccessor(long offset, long size, MemoryMappedFileAccess access)
	{
		throw null;
	}

	public MemoryMappedViewStream CreateViewStream()
	{
		throw null;
	}

	public MemoryMappedViewStream CreateViewStream(long offset, long size)
	{
		throw null;
	}

	public MemoryMappedViewStream CreateViewStream(long offset, long size, MemoryMappedFileAccess access)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	[SupportedOSPlatform("windows")]
	public static MemoryMappedFile OpenExisting(string mapName)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static MemoryMappedFile OpenExisting(string mapName, MemoryMappedFileRights desiredAccessRights)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static MemoryMappedFile OpenExisting(string mapName, MemoryMappedFileRights desiredAccessRights, HandleInheritability inheritability)
	{
		throw null;
	}
}
