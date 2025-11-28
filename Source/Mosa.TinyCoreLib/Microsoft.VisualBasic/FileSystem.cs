using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic;

[StandardModule]
public sealed class FileSystem
{
	internal FileSystem()
	{
	}

	public static void ChDir(string Path)
	{
	}

	[SupportedOSPlatform("windows")]
	public static void ChDrive(char Drive)
	{
	}

	[SupportedOSPlatform("windows")]
	public static void ChDrive(string? Drive)
	{
	}

	public static string CurDir()
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static string CurDir(char Drive)
	{
		throw null;
	}

	public static string Dir()
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static string Dir(string PathName, FileAttribute Attributes = FileAttribute.Normal)
	{
		throw null;
	}

	public static bool EOF(int FileNumber)
	{
		throw null;
	}

	public static OpenMode FileAttr(int FileNumber)
	{
		throw null;
	}

	public static void FileClose(params int[] FileNumbers)
	{
	}

	public static void FileCopy(string Source, string Destination)
	{
	}

	public static DateTime FileDateTime(string PathName)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The target object type could not be statically analyzed and may be trimmed")]
	public static void FileGet(int FileNumber, ref Array Value, long RecordNumber = -1L, bool ArrayIsDynamic = false, bool StringIsFixedLength = false)
	{
	}

	public static void FileGet(int FileNumber, ref bool Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref byte Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref char Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref DateTime Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref decimal Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref double Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref short Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref int Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref long Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref float Value, long RecordNumber = -1L)
	{
	}

	public static void FileGet(int FileNumber, ref string Value, long RecordNumber = -1L, bool StringIsFixedLength = false)
	{
	}

	[RequiresUnreferencedCode("The target object type could not be statically analyzed and may be trimmed")]
	public static void FileGet(int FileNumber, ref ValueType Value, long RecordNumber = -1L)
	{
	}

	[RequiresUnreferencedCode("The target object type could not be statically analyzed and may be trimmed")]
	public static void FileGetObject(int FileNumber, ref object Value, long RecordNumber = -1L)
	{
	}

	public static long FileLen(string PathName)
	{
		throw null;
	}

	public static void FileOpen(int FileNumber, string FileName, OpenMode Mode, OpenAccess Access = OpenAccess.Default, OpenShare Share = OpenShare.Default, int RecordLength = -1)
	{
	}

	[RequiresUnreferencedCode("The origin object type could not be statically analyzed and may be trimmed")]
	public static void FilePut(int FileNumber, Array Value, long RecordNumber = -1L, bool ArrayIsDynamic = false, bool StringIsFixedLength = false)
	{
	}

	public static void FilePut(int FileNumber, bool Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, byte Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, char Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, DateTime Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, decimal Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, double Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, short Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, int Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, long Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, float Value, long RecordNumber = -1L)
	{
	}

	public static void FilePut(int FileNumber, string Value, long RecordNumber = -1L, bool StringIsFixedLength = false)
	{
	}

	[RequiresUnreferencedCode("The origin object type could not be statically analyzed and may be trimmed")]
	public static void FilePut(int FileNumber, ValueType Value, long RecordNumber = -1L)
	{
	}

	[Obsolete("FileSystem.FilePut has been deprecated. Use FilePutObject to write Object types, or coerce FileNumber and RecordNumber to Integer for writing non-Object types.")]
	public static void FilePut(object FileNumber, object Value, object RecordNumber)
	{
	}

	[RequiresUnreferencedCode("The origin object type could not be statically analyzed and may be trimmed")]
	public static void FilePutObject(int FileNumber, object Value, long RecordNumber = -1L)
	{
	}

	public static void FileWidth(int FileNumber, int RecordWidth)
	{
	}

	public static int FreeFile()
	{
		throw null;
	}

	public static FileAttribute GetAttr(string PathName)
	{
		throw null;
	}

	public static void Input(int FileNumber, ref bool Value)
	{
	}

	public static void Input(int FileNumber, ref byte Value)
	{
	}

	public static void Input(int FileNumber, ref char Value)
	{
	}

	public static void Input(int FileNumber, ref DateTime Value)
	{
	}

	public static void Input(int FileNumber, ref decimal Value)
	{
	}

	public static void Input(int FileNumber, ref double Value)
	{
	}

	public static void Input(int FileNumber, ref short Value)
	{
	}

	public static void Input(int FileNumber, ref int Value)
	{
	}

	public static void Input(int FileNumber, ref long Value)
	{
	}

	[RequiresUnreferencedCode("The target object type could not be statically analyzed and may be trimmed")]
	public static void Input(int FileNumber, ref object Value)
	{
	}

	public static void Input(int FileNumber, ref float Value)
	{
	}

	public static void Input(int FileNumber, ref string Value)
	{
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("macos")]
	[UnsupportedOSPlatform("tvos")]
	public static string InputString(int FileNumber, int CharCount)
	{
		throw null;
	}

	public static void Kill(string PathName)
	{
	}

	public static string LineInput(int FileNumber)
	{
		throw null;
	}

	public static long Loc(int FileNumber)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("macos")]
	[UnsupportedOSPlatform("tvos")]
	public static void Lock(int FileNumber)
	{
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("macos")]
	[UnsupportedOSPlatform("tvos")]
	public static void Lock(int FileNumber, long Record)
	{
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("macos")]
	[UnsupportedOSPlatform("tvos")]
	public static void Lock(int FileNumber, long FromRecord, long ToRecord)
	{
	}

	public static long LOF(int FileNumber)
	{
		throw null;
	}

	public static void MkDir(string Path)
	{
	}

	public static void Print(int FileNumber, params object[] Output)
	{
	}

	public static void PrintLine(int FileNumber, params object[] Output)
	{
	}

	[SupportedOSPlatform("windows")]
	public static void Rename(string OldPath, string NewPath)
	{
	}

	public static void Reset()
	{
	}

	public static void RmDir(string Path)
	{
	}

	public static long Seek(int FileNumber)
	{
		throw null;
	}

	public static void Seek(int FileNumber, long Position)
	{
	}

	public static void SetAttr(string PathName, FileAttribute Attributes)
	{
	}

	public static SpcInfo SPC(short Count)
	{
		throw null;
	}

	public static TabInfo TAB()
	{
		throw null;
	}

	public static TabInfo TAB(short Column)
	{
		throw null;
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("macos")]
	[UnsupportedOSPlatform("tvos")]
	public static void Unlock(int FileNumber)
	{
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("macos")]
	[UnsupportedOSPlatform("tvos")]
	public static void Unlock(int FileNumber, long Record)
	{
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("macos")]
	[UnsupportedOSPlatform("tvos")]
	public static void Unlock(int FileNumber, long FromRecord, long ToRecord)
	{
	}

	public static void Write(int FileNumber, params object[] Output)
	{
	}

	public static void WriteLine(int FileNumber, params object[] Output)
	{
	}
}
