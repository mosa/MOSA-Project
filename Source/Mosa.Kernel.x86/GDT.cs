/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.Helpers;
using Mosa.Platform.Internal.x86;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	//public static class GDT_OLD
	//{
	//	private static uint gdtTable = 0x1401000;
	//	private static uint gdtEntries = gdtTable + 6;

	//	#region Data members

	//	internal struct Offset
	//	{
	//		internal const byte LimitLow = 0x00;
	//		internal const byte BaseLow = 0x02;
	//		internal const byte BaseMiddle = 0x04;
	//		internal const byte Access = 0x05;
	//		internal const byte Granularity = 0x06;
	//		internal const byte BaseHigh = 0x07;
	//		internal const byte TotalSize = 0x08;
	//	}

	//	#endregion Data members

	//	public static void Setup()
	//	{
	//		Memory.Clear(gdtTable, 6);
	//		Native.Set16(gdtTable, (Offset.TotalSize * 3) - 1);
	//		Native.Set32(gdtTable + 2, gdtEntries);

	//		Set(0, 0, 0, 0, 0);                // Null segment
	//		Set(1, 0, 0xFFFFFFFF, 0x9A, 0xCF); // Code segment
	//		Set(2, 0, 0xFFFFFFFF, 0x92, 0xCF); // Data segment
	//		//Panic.DumpMemory(gdtEntries);
	//		Native.Lgdt(gdtTable);
	//	}

	//	private static void Set(uint index, uint address, uint limit, byte access, byte granularity)
	//	{
	//		uint entry = GetEntryLocation(index);
	//		Native.Set16(entry + Offset.BaseLow, (ushort)(address & 0xFFFF));
	//		Native.Set8(entry + Offset.BaseMiddle, (byte)((address >> 16) & 0xFF));
	//		Native.Set8(entry + Offset.BaseHigh, (byte)((address >> 24) & 0xFF));
	//		Native.Set16(entry + Offset.LimitLow, (ushort)(limit & 0xFFFF));
	//		Native.Set8(entry + Offset.Granularity, (byte)(((byte)(limit >> 16) & 0x0F) | (granularity & 0xF0)));
	//		Native.Set8(entry + Offset.Access, access);
	//	}

	//	/// <summary>
	//	/// Gets the gdt entry location.
	//	/// </summary>
	//	/// <param name="index">The index.</param>
	//	/// <returns></returns>
	//	private static uint GetEntryLocation(uint index)
	//	{
	//		return (uint)(gdtEntries + (index * Offset.TotalSize));
	//	}
	//}

	//internal static class GDT4
	//{
	//	#region Data members

	//	//internal struct Offset
	//	//{
	//	//	internal const byte TotalSize = 0x08;
	//	//}

	//	#endregion Data members

	//	public static void Setup()
	//	{
	//		//Memory.Clear(0, 6);
	//		//Native.Set16(0, 0);

	//		Memory.Clear(0, 0);
	//		Native.Set16(0, Native.Get16(0));

	//		Native.Lgdt(0);
	//	}
	//}

	unsafe public static class GDT
	{
		private static uint gdtTableAddress = 0x1401000;

		private static ushort _Length = 0;

		private static GDTEntry* entries;

		public static GDTEntry* GetEntry(ushort index)
		{
			return entries + index;
		}

		private static ushort SizeInternal
		{
			get
			{
				return *(ushort*)gdtTableAddress;
			}
			set
			{
				*(ushort*)gdtTableAddress = value;
			}
		}

		public static uint AdressOfEntries
		{
			get
			{
				return *(uint*)(gdtTableAddress + 2);
			}
			private set
			{
				*(uint*)(gdtTableAddress + 2) = value;
			}
		}

		public static ushort Length
		{
			get
			{
				return _Length;
			}
			private set
			{
				_Length = value;
				SizeInternal = (ushort)(value * GDTEntry.EntrySize - 1);
			}
		}

		public static void Setup()
		{
			entries = (GDTEntry*)(gdtTableAddress + 6);
			AdressOfEntries = (uint)entries;

			//Null segment
			var entry = AppendEmptyEntry();

			//code segment
			entry = AppendEmptyEntry();
			entry->BaseAddress = 0;
			entry->Limit = 0xFFFFFFFF;
			entry->Segment = true;
			entry->Executable = true;
			entry->Readable = true;
			entry->PriviligeRing = 0;
			entry->Present = true;
			entry->AddressMode = GDTEntry.EAddressMode.Bits32;
			entry->Granularity = true;

			//data segment
			entry = AppendEmptyEntry();
			entry->BaseAddress = 0;
			entry->Limit = 0xFFFFFFFF;
			entry->Segment = true;
			entry->Writable = true;
			entry->PriviligeRing = 0;
			entry->Present = true;
			entry->AddressMode = GDTEntry.EAddressMode.Bits32;
			entry->Granularity = true;

			Flush();
		}

		public static void Flush()
		{
			#region REMOVE_ME!

			//FIXME: Both statements are not needed, they are here only because of a compiler bug
			Memory.Clear(0, 0);
			Native.Set16(0, Native.Get16(0));

			#endregion REMOVE_ME!

			Native.Lgdt(gdtTableAddress);
		}

		public static void AppendEntry(GDTEntry* source)
		{
			Length++;
			StoreEntry((ushort)(Length - 1), source);
		}

		public static GDTEntry* AppendEmptyEntry()
		{
			Length++;
			var entry = entries + Length - 1;
			GDTEntry.Clear(entry);
			return entry;
		}

		public static void StoreEntry(ushort index, GDTEntry* source)
		{
			GDTEntry.CopyTo(source, entries + index);
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	unsafe public struct GDTEntry
	{
		[FieldOffset(0)]
		private ushort LimitLow;

		[FieldOffset(2)]
		private ushort BaseLow;

		[FieldOffset(4)]
		private byte BaseMiddle;

		[FieldOffset(5)]
		private byte Access;

		[FieldOffset(6)]
		private byte Flags;

		[FieldOffset(7)]
		private byte BaseHigh;

		public const byte EntrySize = 0x08;

		//public void* AdressOfEntry
		//{
		//	get
		//	{
		//		fixed (void* p = &this)
		//			return p;
		//	}
		//}

		public bool IsNullDescriptor
		{
			get
			{
				return LimitLow == 0 && BaseLow == 0 && BaseMiddle == 0 && Access == 0 && Flags == 0 && BaseHigh == 0;
			}
		}

		public uint BaseAddress
		{
			get
			{
				return (uint)(BaseLow | (BaseMiddle << 16) | (BaseHigh << 24));
			}
			set
			{
				BaseLow = (ushort)(value & 0xFFFF);
				BaseMiddle = (byte)((value >> 16) & 0xFF);
				BaseHigh = (byte)((value >> 24) & 0xFF);
			}
		}

		public uint Limit
		{
			get
			{
				return (uint)(LimitLow | Flags.GetBits(FlagsByteOffset.Limit, 4));
			}
			set
			{
				LimitLow = (ushort)(value & 0xFFFF);
				Flags = Flags.SetBits((byte)((value >> 16) & 0x0F), FlagsByteOffset.Limit, 4);
			}
		}

		public static void CopyTo(GDTEntry* source, GDTEntry* destination)
		{
			Memory.Copy((uint)source, (uint)destination, EntrySize);
		}

		public static void Clear(GDTEntry* entry)
		{
			Memory.Clear((uint)entry, EntrySize);
		}

		private class AccessByteOffset
		{
			public const byte Ac = 0;
			public const byte RW = 1;
			public const byte DC = 2;
			public const byte Ex = 3;
			public const byte IsCodeOrData = 4;
			public const byte Privl = 5;
			public const byte Pr = 7;
			public const byte SegmentType = 0; //bits 0-4
		}

		private class FlagsByteOffset
		{
			public const byte Gr = 7;
			public const byte Sz = 6;
			public const byte LongMode = 5;
			public const byte Custom = 4;
			public const byte Limit = 0;
		}

		#region AccessByte

		public bool Present
		{
			get
			{
				return Access.IsFlagSet(AccessByteOffset.Pr);
			}
			set
			{
				Access = Access.SetFlag(AccessByteOffset.Pr, value);
			}
		}

		public bool Executable
		{
			get
			{
				CheckSegment();
				return Access.IsFlagSet(AccessByteOffset.Ex);
			}
			set
			{
				CheckSegment();
				Access = Access.SetFlag(AccessByteOffset.Ex, value);
			}
		}

		public bool Segment
		{
			get
			{
				return Access.IsFlagSet(AccessByteOffset.IsCodeOrData);
			}
			set
			{
				Access = Access.SetFlag(AccessByteOffset.IsCodeOrData, value);
			}
		}

		public bool DirectionConfirming
		{
			get
			{
				CheckSegment();
				return Access.IsFlagSet(AccessByteOffset.DC);
			}
			set
			{
				CheckSegment();
				Access = Access.SetFlag(AccessByteOffset.DC, value);
			}
		}

		public bool ReadWrite
		{
			get
			{
				CheckSegment();
				return Access.IsFlagSet(AccessByteOffset.RW);
			}
			set
			{
				CheckSegment();
				Access = Access.SetFlag(AccessByteOffset.RW, value);
			}
		}

		public bool Accessed
		{
			get
			{
				CheckSegment();
				return Access.IsFlagSet(AccessByteOffset.Ac);
			}
			set
			{
				CheckSegment();

				Access = Access.SetFlag(AccessByteOffset.Ac, value);
			}
		}

		public bool Readable
		{
			get
			{
				CheckSegment();
				return Executable ? ReadWrite : true;
			}
			set
			{
				CheckSegment();
				if (!Executable && !value)
					Panic.Error("Read access is always allowed for data segments");

				ReadWrite = value;
			}
		}

		public bool Writable
		{
			get
			{
				return Executable ? ReadWrite : false;
			}
			set
			{
				if (Executable && value)
					Panic.Error("Write access is never allowed for code segments");

				ReadWrite = value;
			}
		}

		public byte PriviligeRing
		{
			get
			{
				return Access.GetBits(AccessByteOffset.Privl, 2);
			}
			set
			{
				if (value > 3)
					Panic.Error("Privilege ring can't be larger than 3");

				Access = Access.SetBits(value, AccessByteOffset.Privl, 2);
			}
		}

		private void CheckSegment()
		{
			if (!Segment)
				Panic.Error("This attribute can't accessed with this segment type");
		}

		#endregion AccessByte

		#region Flags

		public bool Custom
		{
			get
			{
				return Flags.IsFlagSet(FlagsByteOffset.Custom);
			}
			set
			{
				Flags = Flags.SetFlag(FlagsByteOffset.Custom, value);
			}
		}

		private bool LongMode
		{
			get
			{
				return Flags.IsFlagSet(FlagsByteOffset.LongMode);
			}
			set
			{
				Flags = Flags.SetFlag(FlagsByteOffset.LongMode, value);
				if (value)
					SizeBit = false;
			}
		}

		private bool SizeBit
		{
			get
			{
				return Flags.IsFlagSet(FlagsByteOffset.Sz);
			}
			set
			{
				if (value && LongMode)
					Panic.Error("Size type invalid for long mode");

				Flags = Flags.SetFlag(FlagsByteOffset.Sz, value);
			}
		}

		public EAddressMode AddressMode
		{
			get
			{
				if (SizeBit)
					return EAddressMode.Bits32;
				else
					if (LongMode)
						return EAddressMode.Bits64;
					else
						return EAddressMode.Bits16;
			}
			set
			{
				if (value == EAddressMode.Bits32)
				{
					SizeBit = true;
					LongMode = false;
				}
				else
					if (value == EAddressMode.Bits16)
					{
						LongMode = false;
						SizeBit = false;
					}
					else
					{
						LongMode = true;
						SizeBit = false;
					}
			}
		}

		public bool Granularity
		{
			get
			{
				return Flags.IsFlagSet(FlagsByteOffset.Gr);
			}
			set
			{
				Flags = Flags.SetFlag(FlagsByteOffset.Gr, value);
			}
		}

		public enum EAddressMode : byte
		{
			Bits16 = 16,
			Bits32 = 32,
			Bits64 = 64
		}

		#endregion Flags

		public override string ToString()
		{
			if (IsNullDescriptor)
				return "NullDescriptor";

			var s = ""
				+ "BA=" + BaseAddress.ToHex()
				+ ",Limit=" + Limit.ToHex()
				+ ",Ring=" + this.PriviligeRing.ToString()
				+ ",Mode=" + AddressMode.ToStringNumber()
				+ ",Present=" + this.Present.ToChar()
				+ ",Segment=" + this.Segment.ToChar()
				+ ",Cust=" + this.Custom.ToChar()
			;
			string seg = "";
			if (Segment)
			{
				seg = ""
					+ ",Exec=" + this.Executable.ToChar()
					+ ",RW=" + ReadWrite.ToChar()
					+ ",AC=" + Accessed.ToChar()
					+ ",DC=" + this.DirectionConfirming.ToChar()
				;
			}
			return s + seg;
		}
	}
}