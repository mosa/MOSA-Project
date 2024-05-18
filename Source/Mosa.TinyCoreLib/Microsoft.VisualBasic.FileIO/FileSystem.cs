using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Microsoft.VisualBasic.FileIO;

public class FileSystem
{
	public static string CurrentDirectory
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static ReadOnlyCollection<DriveInfo> Drives
	{
		get
		{
			throw null;
		}
	}

	public static string CombinePath(string baseDirectory, string? relativePath)
	{
		throw null;
	}

	public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName)
	{
	}

	public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, UIOption showUI)
	{
	}

	public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, UIOption showUI, UICancelOption onUserCancel)
	{
	}

	public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwrite)
	{
	}

	public static void CopyFile(string sourceFileName, string destinationFileName)
	{
	}

	public static void CopyFile(string sourceFileName, string destinationFileName, UIOption showUI)
	{
	}

	public static void CopyFile(string sourceFileName, string destinationFileName, UIOption showUI, UICancelOption onUserCancel)
	{
	}

	public static void CopyFile(string sourceFileName, string destinationFileName, bool overwrite)
	{
	}

	public static void CreateDirectory(string directory)
	{
	}

	public static void DeleteDirectory(string directory, DeleteDirectoryOption onDirectoryNotEmpty)
	{
	}

	public static void DeleteDirectory(string directory, UIOption showUI, RecycleOption recycle)
	{
	}

	public static void DeleteDirectory(string directory, UIOption showUI, RecycleOption recycle, UICancelOption onUserCancel)
	{
	}

	public static void DeleteFile(string file)
	{
	}

	public static void DeleteFile(string file, UIOption showUI, RecycleOption recycle)
	{
	}

	public static void DeleteFile(string file, UIOption showUI, RecycleOption recycle, UICancelOption onUserCancel)
	{
	}

	public static bool DirectoryExists(string directory)
	{
		throw null;
	}

	public static bool FileExists(string file)
	{
		throw null;
	}

	public static ReadOnlyCollection<string> FindInFiles(string directory, string containsText, bool ignoreCase, SearchOption searchType)
	{
		throw null;
	}

	public static ReadOnlyCollection<string> FindInFiles(string directory, string containsText, bool ignoreCase, SearchOption searchType, params string[] fileWildcards)
	{
		throw null;
	}

	public static ReadOnlyCollection<string> GetDirectories(string directory)
	{
		throw null;
	}

	public static ReadOnlyCollection<string> GetDirectories(string directory, SearchOption searchType, params string[] wildcards)
	{
		throw null;
	}

	public static DirectoryInfo GetDirectoryInfo(string directory)
	{
		throw null;
	}

	public static DriveInfo GetDriveInfo(string drive)
	{
		throw null;
	}

	public static FileInfo GetFileInfo(string file)
	{
		throw null;
	}

	public static ReadOnlyCollection<string> GetFiles(string directory)
	{
		throw null;
	}

	public static ReadOnlyCollection<string> GetFiles(string directory, SearchOption searchType, params string[] wildcards)
	{
		throw null;
	}

	public static string GetName(string path)
	{
		throw null;
	}

	public static string GetParentPath(string path)
	{
		throw null;
	}

	public static string GetTempFileName()
	{
		throw null;
	}

	public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
	{
	}

	public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName, UIOption showUI)
	{
	}

	public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName, UIOption showUI, UICancelOption onUserCancel)
	{
	}

	public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName, bool overwrite)
	{
	}

	public static void MoveFile(string sourceFileName, string destinationFileName)
	{
	}

	public static void MoveFile(string sourceFileName, string destinationFileName, UIOption showUI)
	{
	}

	public static void MoveFile(string sourceFileName, string destinationFileName, UIOption showUI, UICancelOption onUserCancel)
	{
	}

	public static void MoveFile(string sourceFileName, string destinationFileName, bool overwrite)
	{
	}

	public static TextFieldParser OpenTextFieldParser(string file)
	{
		throw null;
	}

	public static TextFieldParser OpenTextFieldParser(string file, params int[] fieldWidths)
	{
		throw null;
	}

	public static TextFieldParser OpenTextFieldParser(string file, params string[] delimiters)
	{
		throw null;
	}

	public static StreamReader OpenTextFileReader(string file)
	{
		throw null;
	}

	public static StreamReader OpenTextFileReader(string file, Encoding encoding)
	{
		throw null;
	}

	public static StreamWriter OpenTextFileWriter(string file, bool append)
	{
		throw null;
	}

	public static StreamWriter OpenTextFileWriter(string file, bool append, Encoding encoding)
	{
		throw null;
	}

	public static byte[] ReadAllBytes(string file)
	{
		throw null;
	}

	public static string ReadAllText(string file)
	{
		throw null;
	}

	public static string ReadAllText(string file, Encoding encoding)
	{
		throw null;
	}

	public static void RenameDirectory(string directory, string newName)
	{
	}

	public static void RenameFile(string file, string newName)
	{
	}

	public static void WriteAllBytes(string file, byte[] data, bool append)
	{
	}

	public static void WriteAllText(string file, string text, bool append)
	{
	}

	public static void WriteAllText(string file, string text, bool append, Encoding encoding)
	{
	}
}
