﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Demo.TestWorld.x86;

public static class Serial
{
	public const ushort COM1 = 0x3F8; // Kernel log
	public const ushort COM2 = 0x2F8; // Mosa Debugger
	public const ushort COM3 = 0x3E8;
	public const ushort COM4 = 0x2E8;

	private const byte COM_Data = 0x00;
	private const byte COM_Interrupt = 0x01;
	private const byte COM_LineControl = 0x02;
	private const byte COM_ModemControl = 0x03;
	private const byte COM_LineStatus = 0x04;
	private const byte COM_ModemStatus = 0x05;
	private const byte COM_Scratch = 0x06;

	public static void Setup(ushort com)
	{
		// Disable all interrupts
		Native.Out8((ushort)(com + COM_Interrupt), 0x00);

		// Enable DLAB (set baud rate divisor)
		Native.Out8((ushort)(com + COM_ModemControl), 0x80);

		// Set divisor to 1 (lo byte) 115200 baud
		Native.Out8((ushort)(com + COM_Data), 0x01);

		// (hi byte)
		Native.Out8((ushort)(com + COM_Interrupt), 0x00);

		// 8 bits, no parity, one stop bit
		Native.Out8((ushort)(com + COM_ModemControl), 0x03);

		// Enable FIFO, clear them, with 14-byte threshold
		Native.Out8((ushort)(com + COM_LineControl), 0xC7);

		// IRQs enabled, RTS/DSR set
		Native.Out8((ushort)(com + COM_LineStatus), 0x0B);

		// Enable all interrupts
		Native.Out8((ushort)(com + COM_Interrupt), 0x0F);
	}

	public static void Write(ushort com, byte b)
	{
		// Wait for the serial to become ready for writing
		while ((Native.In8((ushort)(com + COM_ModemStatus)) & 0x20) == 0x0) Native.Hlt();

		Native.Out8(com, b);
	}
}
