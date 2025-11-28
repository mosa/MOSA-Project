using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System;

public static class Environment
{
	public enum SpecialFolder
	{
		Desktop = 0,
		Programs = 2,
		MyDocuments = 5,
		Personal = 5,
		Favorites = 6,
		Startup = 7,
		Recent = 8,
		SendTo = 9,
		StartMenu = 11,
		MyMusic = 13,
		MyVideos = 14,
		DesktopDirectory = 16,
		MyComputer = 17,
		NetworkShortcuts = 19,
		Fonts = 20,
		Templates = 21,
		CommonStartMenu = 22,
		CommonPrograms = 23,
		CommonStartup = 24,
		CommonDesktopDirectory = 25,
		ApplicationData = 26,
		PrinterShortcuts = 27,
		LocalApplicationData = 28,
		InternetCache = 32,
		Cookies = 33,
		History = 34,
		CommonApplicationData = 35,
		Windows = 36,
		System = 37,
		ProgramFiles = 38,
		MyPictures = 39,
		UserProfile = 40,
		SystemX86 = 41,
		ProgramFilesX86 = 42,
		CommonProgramFiles = 43,
		CommonProgramFilesX86 = 44,
		CommonTemplates = 45,
		CommonDocuments = 46,
		CommonAdminTools = 47,
		AdminTools = 48,
		CommonMusic = 53,
		CommonPictures = 54,
		CommonVideos = 55,
		Resources = 56,
		LocalizedResources = 57,
		CommonOemLinks = 58,
		CDBurning = 59
	}

	public enum SpecialFolderOption
	{
		None = 0,
		DoNotVerify = 0x4000,
		Create = 0x8000
	}

	public static string CommandLine
	{
		get
		{
			throw null;
		}
	}

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

	public static int CurrentManagedThreadId
	{
		get
		{
			throw null;
		}
	}

	public static int ExitCode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static bool HasShutdownStarted
	{
		get
		{
			throw null;
		}
	}

	public static bool Is64BitOperatingSystem
	{
		get
		{
			throw null;
		}
	}

	public static bool Is64BitProcess
	{
		get
		{
			throw null;
		}
	}

	public static bool IsPrivilegedProcess
	{
		get
		{
			throw null;
		}
	}

	public static string MachineName
	{
		get
		{
			throw null;
		}
	}

	public static string NewLine
	{
		get
		{
			throw null;
		}
	}

	public static OperatingSystem OSVersion
	{
		get
		{
			throw null;
		}
	}

	public static int ProcessId
	{
		get
		{
			throw null;
		}
	}

	public static int ProcessorCount
	{
		get
		{
			throw null;
		}
	}

	public static string? ProcessPath
	{
		get
		{
			throw null;
		}
	}

	public static string StackTrace
	{
		get
		{
			throw null;
		}
	}

	public static string SystemDirectory
	{
		get
		{
			throw null;
		}
	}

	public static int SystemPageSize
	{
		get
		{
			throw null;
		}
	}

	public static int TickCount
	{
		get
		{
			throw null;
		}
	}

	public static long TickCount64
	{
		get
		{
			throw null;
		}
	}

	public static string UserDomainName
	{
		get
		{
			throw null;
		}
	}

	public static bool UserInteractive
	{
		get
		{
			throw null;
		}
	}

	public static string UserName
	{
		get
		{
			throw null;
		}
	}

	public static Version Version
	{
		get
		{
			throw null;
		}
	}

	public static long WorkingSet
	{
		get
		{
			throw null;
		}
	}

	[DoesNotReturn]
	public static void Exit(int exitCode)
	{
		throw null;
	}

	public static string ExpandEnvironmentVariables(string name)
	{
		throw null;
	}

	[DoesNotReturn]
	public static void FailFast(string? message)
	{
		throw null;
	}

	[DoesNotReturn]
	public static void FailFast(string? message, Exception? exception)
	{
		throw null;
	}

	public static string[] GetCommandLineArgs()
	{
		throw null;
	}

	public static string? GetEnvironmentVariable(string variable)
	{
		throw null;
	}

	public static string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
	{
		throw null;
	}

	public static IDictionary GetEnvironmentVariables()
	{
		throw null;
	}

	public static IDictionary GetEnvironmentVariables(EnvironmentVariableTarget target)
	{
		throw null;
	}

	public static string GetFolderPath(SpecialFolder folder)
	{
		throw null;
	}

	public static string GetFolderPath(SpecialFolder folder, SpecialFolderOption option)
	{
		throw null;
	}

	public static string[] GetLogicalDrives()
	{
		throw null;
	}

	public static void SetEnvironmentVariable(string variable, string? value)
	{
	}

	public static void SetEnvironmentVariable(string variable, string? value, EnvironmentVariableTarget target)
	{
	}
}
