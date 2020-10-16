// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// IDT Stack
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct IDTStack
	{
		[FieldOffset(0x00)]
		public uint EDI;

		[FieldOffset(0x04)]
		public uint ESI;

		[FieldOffset(0x08)]
		public uint EBP;

		[FieldOffset(0x0C)]
		public uint ESP;

		[FieldOffset(0x10)]
		public uint EBX;

		[FieldOffset(0x14)]
		public uint EDX;

		[FieldOffset(0x18)]
		public uint ECX;

		[FieldOffset(0x1C)]
		public uint EAX;

		[FieldOffset(0x20)]
		public uint Interrupt;

		[FieldOffset(0x24)]
		public uint ErrorCode;

		[FieldOffset(0x28)]
		public uint EIP;

		[FieldOffset(0x2C)]
		public uint CS;

		[FieldOffset(0x30)]
		public uint EFLAGS;
	}
}
