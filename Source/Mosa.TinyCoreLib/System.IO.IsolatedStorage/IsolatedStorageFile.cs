using System.Collections;

namespace System.IO.IsolatedStorage;

public sealed class IsolatedStorageFile : IsolatedStorage, IDisposable
{
	public override long AvailableFreeSpace
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	[Obsolete("IsolatedStorageFile.CurrentSize has been deprecated because it is not CLS Compliant. To get the current size use IsolatedStorageFile.UsedSize instead.")]
	public override ulong CurrentSize
	{
		get
		{
			throw null;
		}
	}

	public static bool IsEnabled
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	[Obsolete("IsolatedStorageFile.MaximumSize has been deprecated because it is not CLS Compliant. To get the maximum size use IsolatedStorageFile.Quota instead.")]
	public override ulong MaximumSize
	{
		get
		{
			throw null;
		}
	}

	public override long Quota
	{
		get
		{
			throw null;
		}
	}

	public override long UsedSize
	{
		get
		{
			throw null;
		}
	}

	internal IsolatedStorageFile()
	{
	}

	public void Close()
	{
	}

	public void CopyFile(string sourceFileName, string destinationFileName)
	{
	}

	public void CopyFile(string sourceFileName, string destinationFileName, bool overwrite)
	{
	}

	public void CreateDirectory(string dir)
	{
	}

	public IsolatedStorageFileStream CreateFile(string path)
	{
		throw null;
	}

	public void DeleteDirectory(string dir)
	{
	}

	public void DeleteFile(string file)
	{
	}

	public bool DirectoryExists(string path)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public bool FileExists(string path)
	{
		throw null;
	}

	public DateTimeOffset GetCreationTime(string path)
	{
		throw null;
	}

	public string[] GetDirectoryNames()
	{
		throw null;
	}

	public string[] GetDirectoryNames(string searchPattern)
	{
		throw null;
	}

	public static IEnumerator GetEnumerator(IsolatedStorageScope scope)
	{
		throw null;
	}

	public string[] GetFileNames()
	{
		throw null;
	}

	public string[] GetFileNames(string searchPattern)
	{
		throw null;
	}

	public DateTimeOffset GetLastAccessTime(string path)
	{
		throw null;
	}

	public DateTimeOffset GetLastWriteTime(string path)
	{
		throw null;
	}

	public static IsolatedStorageFile GetMachineStoreForApplication()
	{
		throw null;
	}

	public static IsolatedStorageFile GetMachineStoreForAssembly()
	{
		throw null;
	}

	public static IsolatedStorageFile GetMachineStoreForDomain()
	{
		throw null;
	}

	public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object? applicationIdentity)
	{
		throw null;
	}

	public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, object? domainIdentity, object? assemblyIdentity)
	{
		throw null;
	}

	public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type? applicationEvidenceType)
	{
		throw null;
	}

	public static IsolatedStorageFile GetStore(IsolatedStorageScope scope, Type? domainEvidenceType, Type? assemblyEvidenceType)
	{
		throw null;
	}

	public static IsolatedStorageFile GetUserStoreForApplication()
	{
		throw null;
	}

	public static IsolatedStorageFile GetUserStoreForAssembly()
	{
		throw null;
	}

	public static IsolatedStorageFile GetUserStoreForDomain()
	{
		throw null;
	}

	public static IsolatedStorageFile GetUserStoreForSite()
	{
		throw null;
	}

	public override bool IncreaseQuotaTo(long newQuotaSize)
	{
		throw null;
	}

	public void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
	{
	}

	public void MoveFile(string sourceFileName, string destinationFileName)
	{
	}

	public IsolatedStorageFileStream OpenFile(string path, FileMode mode)
	{
		throw null;
	}

	public IsolatedStorageFileStream OpenFile(string path, FileMode mode, FileAccess access)
	{
		throw null;
	}

	public IsolatedStorageFileStream OpenFile(string path, FileMode mode, FileAccess access, FileShare share)
	{
		throw null;
	}

	public override void Remove()
	{
	}

	public static void Remove(IsolatedStorageScope scope)
	{
	}
}
