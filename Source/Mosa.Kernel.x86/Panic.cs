// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86;

/// <summary>
/// Panic
/// </summary>
public static class Panic
{
	private static bool firstError = true;

	public static uint EBP = 0;
	public static uint EIP = 0;
	public static uint EAX = 0;
	public static uint EBX = 0;
	public static uint ECX = 0;
	public static uint EDX = 0;
	public static uint EDI = 0;
	public static uint ESI = 0;
	public static uint ESP = 0;
	public static uint Interrupt = 0;
	public static uint ErrorCode = 0;
	public static uint CS = 0;
	public static uint EFLAGS = 0;
	public static uint CR2 = 0;
	public static uint FS = 0;

	public static void Setup()
	{
	}

	public static void Error(string message)
	{
		IDT.SetInterruptHandler(null);

		if (!firstError)
		{
			Screen.Goto(0, 0);
			Screen.Write("(multiple)");

			while (true)
			{
				Native.Hlt();
			}
		}

		firstError = false;

		Screen.BackgroundColor = ScreenColor.Blue;

		Screen.Clear();
		Screen.Goto(1, 0);
		Screen.Color = ScreenColor.White;
		Screen.Write("*** Kernel Panic ***");

		if (firstError)
			firstError = false;
		else
			Screen.Write(" (multiple)");

		Screen.NextLine();
		Screen.NextLine();
		Screen.Write(message);
		Screen.NextLine();
		Screen.NextLine();
		Screen.Write("REGISTERS:");
		Screen.NextLine();
		Screen.NextLine();
		DumpRegisters();
		Screen.NextLine();
		Screen.Write("STACK TRACE:");
		Screen.NextLine();
		Screen.NextLine();
		DumpStackTrace();

		while (true)
		{
			UnitTestEngine.Process();

			Native.Hlt();
		}
	}

	public static void DumpRegisters()
	{
		Screen.Write("EIP: ");
		Screen.Write(EIP, 16, 8);
		Screen.Write(" ESP: ");
		Screen.Write(ESP, 16, 8);
		Screen.Write(" EBP: ");
		Screen.Write(EBP, 16, 8);
		Screen.Write(" EFLAGS: ");
		Screen.Write(EFLAGS, 16, 8);
		Screen.Write(" CR2: ");
		Screen.Write(CR2, 16, 8);
		Screen.NextLine();
		Screen.Write("EAX: ");
		Screen.Write(EAX, 16, 8);
		Screen.Write(" EBX: ");
		Screen.Write(EBX, 16, 8);
		Screen.Write(" ECX: ");
		Screen.Write(ECX, 16, 8);
		Screen.Write(" CS: ");
		Screen.Write(CS, 16, 8);
		Screen.Write(" FS: ");
		Screen.Write(FS, 16, 8);
		Screen.NextLine();
		Screen.Write("EDX: ");
		Screen.Write(EDX, 16, 8);
		Screen.Write(" EDI: ");
		Screen.Write(EDI, 16, 8);
		Screen.Write(" ESI: ");
		Screen.Write(ESI, 16, 8);
		Screen.Write(" ERROR: ");
		Screen.Write(ErrorCode, 16, 2);
		Screen.Write(" IRQ: ");
		Screen.Write(Interrupt, 16, 2);
		Screen.NextLine();
	}

	public static void DumpStackTrace()
	{
		DumpStackTrace(0);
	}

	private static void DumpStackTrace(uint depth)
	{
		while (true)
		{
			var entry = Runtime.Internal.GetStackTraceEntry(depth, new Pointer(EBP), new Pointer(EIP));

			if (!entry.Valid)
				return;

			if (!entry.Skip)
			{
				Screen.Write(entry.ToString());
				Screen.Row++;
				Screen.Column = 0;
			}

			depth++;
		}
	}
}
