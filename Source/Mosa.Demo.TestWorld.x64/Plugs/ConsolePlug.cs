// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;

namespace Mosa.Demo.TestWorld.x64.Plugs;

public static class ConsolePlug
{
	[Plug("System.Console::ResetColor")]
	internal static void ResetColor()
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::Clear")]
	internal static void Clear()
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::WriteLine")]
	internal static void WriteLine()
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::WriteLine")]
	internal static void WriteLine(string value)
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::Write")]
	internal static void Write(string value)
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::Write")]
	internal static void Write(char value)
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::SetCursorPosition")]
	internal static void SetCursorPosition(int left, int top)
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::GetForegroundColor")]
	internal static ConsoleColor GetForegroundColor()
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::GetBackgroundColor")]
	internal static ConsoleColor GetBackgroundColor()
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::SetForegroundColor")]
	internal static void SetForegroundColor(ConsoleColor color)
	{
		throw new NotImplementedException();
	}

	[Plug("System.Console::SetBackgroundColor")]
	internal static void SetBackgroundColor(ConsoleColor color)
	{
		throw new NotImplementedException();
	}
}
