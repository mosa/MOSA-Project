using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic;

[StandardModule]
public sealed class Interaction
{
	internal Interaction()
	{
	}

	public static void AppActivate(int ProcessId)
	{
	}

	public static void AppActivate(string Title)
	{
	}

	[SupportedOSPlatform("windows")]
	public static void Beep()
	{
	}

	[RequiresUnreferencedCode("The type of ObjectRef cannot be statically analyzed and its members may be trimmed.")]
	public static object? CallByName(object? ObjectRef, string ProcName, CallType UseCallType, params object?[] Args)
	{
		throw null;
	}

	public static object? Choose(double Index, params object?[] Choice)
	{
		throw null;
	}

	public static string Command()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The COM object to be created cannot be statically analyzed and may be trimmed")]
	[SupportedOSPlatform("windows")]
	public static object CreateObject(string ProgId, string? ServerName = "")
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static void DeleteSetting(string AppName, string? Section = null, string? Key = null)
	{
	}

	public static string Environ(string? Expression)
	{
		throw null;
	}

	public static string Environ(int Expression)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static string[,]? GetAllSettings(string AppName, string Section)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The COM component to be returned cannot be statically analyzed and may be trimmed")]
	[SupportedOSPlatform("windows")]
	public static object? GetObject(string? PathName = null, string? Class = null)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static string? GetSetting(string AppName, string Section, string Key, string? Default = "")
	{
		throw null;
	}

	public static object? IIf(bool Expression, object? TruePart, object? FalsePart)
	{
		throw null;
	}

	public static string InputBox(string Prompt, string Title = "", string DefaultResponse = "", int XPos = -1, int YPos = -1)
	{
		throw null;
	}

	public static MsgBoxResult MsgBox(object Prompt, MsgBoxStyle Buttons = MsgBoxStyle.ApplicationModal, object? Title = null)
	{
		throw null;
	}

	public static string Partition(long Number, long Start, long Stop, long Interval)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static void SaveSetting(string AppName, string Section, string Key, string Setting)
	{
	}

	public static int Shell(string PathName, AppWinStyle Style = AppWinStyle.MinimizedFocus, bool Wait = false, int Timeout = -1)
	{
		throw null;
	}

	public static object? Switch(params object?[]? VarExpr)
	{
		throw null;
	}
}
