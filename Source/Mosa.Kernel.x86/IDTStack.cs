// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86;

/// <summary>
/// IDT Stack
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct IDTStack
{
	[FieldOffset(0x00)]
	public readonly uint EDI;

	[FieldOffset(0x04)]
	public readonly uint ESI;

	[FieldOffset(0x08)]
	public readonly uint EBP;

	[FieldOffset(0x0C)]
	public readonly uint ESP;

	[FieldOffset(0x10)]
	public readonly uint EBX;

	[FieldOffset(0x14)]
	public readonly uint EDX;

	[FieldOffset(0x18)]
	public readonly uint ECX;

	[FieldOffset(0x1C)]
	public readonly uint EAX;

	[FieldOffset(0x20)]
	public readonly uint Interrupt;

	[FieldOffset(0x24)]
	public readonly uint ErrorCode;

	[FieldOffset(0x28)]
	public uint EIP;

	[FieldOffset(0x2C)]
	public readonly uint CS;

	[FieldOffset(0x30)]
	public readonly uint EFLAGS;
}
