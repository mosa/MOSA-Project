// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.DebugEngine;

public static class DebugCode
{
	public const byte Connecting = 10;
	public const byte Connected = 11;
	public const byte Disconnected = 12;

	public const byte UnknownData = 99;

	public const byte Alive = 100;
	public const byte Ready = 101;
	public const byte Ping = 102;
	public const byte InformationalMessage = 103;
	public const byte WarningMessage = 104;
	public const byte ErrorMessage = 105;
	public const byte SendNumber = 106;
	public const byte ReadMemory = 110;
	public const byte WriteMemory = 111;
	public const byte ReadCR3 = 112;
	public const byte Scattered32BitReadMemory = 113;
	public const byte ClearMemory = 114;
	public const byte GetMemoryCRC = 115;
	public const byte CompressedWriteMemory = 121;

	public const byte HardJump = 211;

	public const byte ExecuteUnitTest = 200;
	public const byte AbortUnitTest = 201;
}
