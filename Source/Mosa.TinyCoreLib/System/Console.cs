using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;
using System.Text;

namespace System;

public static class Console
{
	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static ConsoleColor BackgroundColor
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int BufferHeight
	{
		[UnsupportedOSPlatform("android")]
		[UnsupportedOSPlatform("browser")]
		[UnsupportedOSPlatform("ios")]
		[UnsupportedOSPlatform("tvos")]
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public static int BufferWidth
	{
		[UnsupportedOSPlatform("android")]
		[UnsupportedOSPlatform("browser")]
		[UnsupportedOSPlatform("ios")]
		[UnsupportedOSPlatform("tvos")]
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	[SupportedOSPlatform("windows")]
	public static bool CapsLock
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static int CursorLeft
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int CursorSize
	{
		[UnsupportedOSPlatform("android")]
		[UnsupportedOSPlatform("browser")]
		[UnsupportedOSPlatform("ios")]
		[UnsupportedOSPlatform("tvos")]
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static int CursorTop
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static bool CursorVisible
	{
		[SupportedOSPlatform("windows")]
		get
		{
			throw null;
		}
		[UnsupportedOSPlatform("android")]
		[UnsupportedOSPlatform("browser")]
		[UnsupportedOSPlatform("ios")]
		[UnsupportedOSPlatform("tvos")]
		set
		{
		}
	}

	public static TextWriter Error
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static ConsoleColor ForegroundColor
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static TextReader In
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static Encoding InputEncoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static bool IsErrorRedirected
	{
		get
		{
			throw null;
		}
	}

	public static bool IsInputRedirected
	{
		get
		{
			throw null;
		}
	}

	public static bool IsOutputRedirected
	{
		get
		{
			throw null;
		}
	}

	public static bool KeyAvailable
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static int LargestWindowHeight
	{
		get
		{
			throw null;
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static int LargestWindowWidth
	{
		get
		{
			throw null;
		}
	}

	[SupportedOSPlatform("windows")]
	public static bool NumberLock
	{
		get
		{
			throw null;
		}
	}

	public static TextWriter Out
	{
		get
		{
			throw null;
		}
	}

	public static Encoding OutputEncoding
	{
		get
		{
			throw null;
		}
		[UnsupportedOSPlatform("android")]
		[UnsupportedOSPlatform("ios")]
		[UnsupportedOSPlatform("tvos")]
		set
		{
		}
	}

	public static string Title
	{
		[SupportedOSPlatform("windows")]
		get
		{
			throw null;
		}
		[UnsupportedOSPlatform("android")]
		[UnsupportedOSPlatform("browser")]
		[UnsupportedOSPlatform("ios")]
		[UnsupportedOSPlatform("tvos")]
		set
		{
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static bool TreatControlCAsInput
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static int WindowHeight
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int WindowLeft
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	public static int WindowTop
	{
		get
		{
			throw null;
		}
		[SupportedOSPlatform("windows")]
		set
		{
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static int WindowWidth
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static event ConsoleCancelEventHandler? CancelKeyPress
	{
		add
		{
		}
		remove
		{
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static void Beep()
	{
	}

	[SupportedOSPlatform("windows")]
	public static void Beep(int frequency, int duration)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static void Clear()
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static (int Left, int Top) GetCursorPosition()
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
	{
	}

	[SupportedOSPlatform("windows")]
	public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
	{
	}

	public static Stream OpenStandardError()
	{
		throw null;
	}

	public static Stream OpenStandardError(int bufferSize)
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static Stream OpenStandardInput()
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	public static Stream OpenStandardInput(int bufferSize)
	{
		throw null;
	}

	public static Stream OpenStandardOutput()
	{
		throw null;
	}

	public static Stream OpenStandardOutput(int bufferSize)
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	public static int Read()
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static ConsoleKeyInfo ReadKey()
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static ConsoleKeyInfo ReadKey(bool intercept)
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	public static string? ReadLine()
	{
		throw null;
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static void ResetColor()
	{
	}

	[SupportedOSPlatform("windows")]
	public static void SetBufferSize(int width, int height)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static void SetCursorPosition(int left, int top)
	{
	}

	public static void SetError(TextWriter newError)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static void SetIn(TextReader newIn)
	{
	}

	public static void SetOut(TextWriter newOut)
	{
	}

	[SupportedOSPlatform("windows")]
	public static void SetWindowPosition(int left, int top)
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static void SetWindowSize(int width, int height)
	{
	}

	public static void Write(bool value)
	{
	}

	public static void Write(char value)
	{
	}

	public static void Write(char[]? buffer)
	{
	}

	public static void Write(char[] buffer, int index, int count)
	{
	}

	public static void Write(decimal value)
	{
	}

	public static void Write(double value)
	{
	}

	public static void Write(int value)
	{
	}

	public static void Write(long value)
	{
	}

	public static void Write(object? value)
	{
	}

	public static void Write(float value)
	{
	}

	public static void Write(string? value)
	{
	}

	public static void Write([StringSyntax("CompositeFormat")] string format, object? arg0)
	{
	}

	public static void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
	{
	}

	public static void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
	{
	}

	public static void Write([StringSyntax("CompositeFormat")] string format, params object?[]? arg)
	{
	}

	[CLSCompliant(false)]
	public static void Write(uint value)
	{
	}

	[CLSCompliant(false)]
	public static void Write(ulong value)
	{
	}

	public static void WriteLine()
	{
	}

	public static void WriteLine(bool value)
	{
	}

	public static void WriteLine(char value)
	{
	}

	public static void WriteLine(char[]? buffer)
	{
	}

	public static void WriteLine(char[] buffer, int index, int count)
	{
	}

	public static void WriteLine(decimal value)
	{
	}

	public static void WriteLine(double value)
	{
	}

	public static void WriteLine(int value)
	{
	}

	public static void WriteLine(long value)
	{
	}

	public static void WriteLine(object? value)
	{
	}

	public static void WriteLine(float value)
	{
	}

	public static void WriteLine(string? value)
	{
	}

	public static void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0)
	{
	}

	public static void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
	{
	}

	public static void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
	{
	}

	public static void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[]? arg)
	{
	}

	[CLSCompliant(false)]
	public static void WriteLine(uint value)
	{
	}

	[CLSCompliant(false)]
	public static void WriteLine(ulong value)
	{
	}
}
