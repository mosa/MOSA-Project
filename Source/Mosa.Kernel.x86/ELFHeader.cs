/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Sebastian Loncar (Arakis) <sebastian.loncar@gmail.com>
 */

using Mosa.Kernel.x86.Helpers;
using Mosa.Platform.Internal.x86;
using System;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Not completet yet
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct ELFHeader
	{
		#region Fields

		[FieldOffset(0x00)]
		public uint Magic;

		[FieldOffset(0x04)]
		public byte BitMode;

		[FieldOffset(0x05)]
		public byte BigEndian;

		[FieldOffset(0x06)]
		public byte ELFIdentVersion;

		[FieldOffset(0x07)]
		public byte OperatingSystem;

		[FieldOffset(0x08)]
		public byte ABIVersion;

		[FieldOffset(0x09)]
		public byte PAD; //unused

		[FieldOffset(0x10)]
		public byte Type;

		[FieldOffset(0x12)]
		public byte Machine;

		[FieldOffset(0x14)]
		public uint ELFVersion;

		[FieldOffset(0x18)]
		public uint Entry;

		[FieldOffset(0x1C)]
		public uint Phoff;

		[FieldOffset(0x20)]
		public uint Shoff;

		[FieldOffset(0x24)]
		public uint Flags;

		[FieldOffset(0x28)]
		public ushort HeaderSize;

		[FieldOffset(0x2A)]
		public ushort Phentsize;

		[FieldOffset(0x2C)]
		public ushort Phnum;

		[FieldOffset(0x2E)]
		public ushort Shentsize;

		[FieldOffset(0x30)]
		public ushort Shnum;

		[FieldOffset(0x32)]
		public ushort Shstrndx;

		#endregion Fields

		private const uint defaultMagic = 0x464C457F; //0x7F followed by ELF in ASCII

		public void Validate()
		{
			Assert.True(IsValid, "ELF structure is not valid");
		}

		public bool IsValid
		{
			get { return Magic == defaultMagic; }
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct ProgramTableHeaderEntry
		{
			[FieldOffset(0)]
			public uint Type;

			[FieldOffset(4)]
			public uint Offset;

			[FieldOffset(0)]
			public IntPtr Vaddr;

			[FieldOffset(8)]
			public IntPtr Paddr;

			[FieldOffset(12)]
			public uint Filesz;

			[FieldOffset(16)]
			public uint Memsz;

			[FieldOffset(20)]
			public uint Flags;

			[FieldOffset(24)]
			public uint Align;
		}

		unsafe private uint AddressOfThis
		{
			get
			{
				return (uint)Mosa.Internal.Intrinsic.GetValueTypeAddress(this);
			}
		}

		unsafe internal ProgramTableHeaderEntry* GetProgramHeaderTableEntry(ushort index)
		{
			Assert.InRange(index, Phnum);
			return (ProgramTableHeaderEntry*)(AddressOfThis + Phoff + (Phentsize * index));
		}
	}
}