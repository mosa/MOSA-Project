/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 *  Sebastion Loncar (Arakis) <sebastion.loncar@gmail.com>
 */

using Mosa.Kernel.Helpers;
using Mosa.Platform.Internal.x86;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	//public static class GDT_
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

		private static ushort length = 0;

		private static DTEntry* entries;

		private static DTEntry* GetEntryRef(ushort index)
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

		private static uint AddressOfEntries
		{
			get
			{
				return *(uint*)(gdtTableAddress + 2);
			}
			set
			{
				*(uint*)(gdtTableAddress + 2) = value;
			}
		}

		public static ushort Length
		{
			get
			{
				return length;
			}
			private set
			{
				length = value;
				SizeInternal = (ushort)(value * DTEntry.EntrySize - 1);
			}
		}

		/// <summary>
		/// Sets up the GDT table and entries
		/// </summary>
		public static void Setup()
		{
			entries = (DTEntry*)(gdtTableAddress + 6);
			AddressOfEntries = (uint)entries;

			//Null segment
			var nullEntry = DTEntry.CreateNullDescriptor();
			AddEntry((DTEntry)nullEntry);

			//code segment
			var codeEntry = DTEntry.CreateCode(0, 0xFFFFFFFF);
			codeEntry.Readable = true;
			codeEntry.PriviligeRing = 0;
			codeEntry.Present = true;
			codeEntry.AddressMode = DTEntry.EAddressMode.Bits32;
			codeEntry.Granularity = true;
			AddEntry((DTEntry)codeEntry);

			//data segment
			var dataEntry = DTEntry.CreateData(0, 0xFFFFFFFF);
			dataEntry.Writable = true;
			dataEntry.PriviligeRing = 0;
			dataEntry.Present = true;
			dataEntry.AddressMode = DTEntry.EAddressMode.Bits32;
			dataEntry.Granularity = true;
			AddEntry((DTEntry)dataEntry);

			//Panic.DumpMemory((uint)entries);

			Flush();
		}

		/// <summary>
		/// Flushes the GDT table
		/// </summary>
		public static void Flush()
		{
			#region REMOVE_ME!

			//FIXME: Both statements are not needed, they are here only because of a compiler bug
			Memory.Clear(0, 0);
			Native.Set16(0, Native.Get16(0));

			#endregion REMOVE_ME!

			Native.Lgdt(gdtTableAddress);
		}

		public static void AddEntry(DTEntry source)
		{
			Length++;
			SetEntry((ushort)(Length - 1), source);
		}

		public static void SetEntry(ushort index, DTEntry source)
		{
			if (index < 0 || index >= length)
				Panic.OutOfRange();

			DTEntry.CopyTo(&source, entries + index);
		}

		public static DTEntry GetEntry(ushort index)
		{
			if (index < 0 || index >= length)
				Panic.OutOfRange();

			return *(entries + index);
		}

		public static T GetValueTypeFromAddress<T>(uint address) where T : struct
		{
			return default(T);
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	unsafe public struct DTEntry : IDTEntry, IDTCodeSegment, IDTDataSegment, IDTUserDescriptor, IDTSystemDescriptor
	{
		[FieldOffset(0)]
		private ushort limitLow;

		[FieldOffset(2)]
		private ushort baseLow;

		[FieldOffset(4)]
		private byte baseMiddle;

		[FieldOffset(5)]
		private byte access;

		[FieldOffset(6)]
		private byte flags;

		[FieldOffset(7)]
		private byte baseHigh;

		public const byte EntrySize = 0x08;

		public static DTEntry CreateNullDescriptor()
		{
			return new DTEntry();
		}

		private static DTEntry Create(uint baseAddress, uint limit)
		{
			return new DTEntry() { BaseAddress = baseAddress, Limit = limit };
		}

		public static IDTCodeSegment CreateCode(uint baseAddress, uint limit)
		{
			IDTCodeSegment seg = Create(baseAddress, limit);
			seg.IsUserType = true;
			seg.Executable = true;
			return seg;
		}

		public static IDTDataSegment CreateData(uint baseAddress, uint limit)
		{
			IDTDataSegment seg = Create(baseAddress, limit);
			seg.IsUserType = true;
			return seg;
		}

		public bool IsNullDescriptor
		{
			get
			{
				return limitLow == 0 && baseLow == 0 && baseMiddle == 0 && access == 0 && flags == 0 && baseHigh == 0;
			}
		}

		public uint BaseAddress
		{
			get
			{
				return (uint)(baseLow | (baseMiddle << 16) | (baseHigh << 24));
			}
			set
			{
				baseLow = (ushort)(value & 0xFFFF);
				baseMiddle = (byte)((value >> 16) & 0xFF);
				baseHigh = (byte)((value >> 24) & 0xFF);
			}
		}

		public uint Limit
		{
			get
			{
				return (uint)(limitLow | flags.GetBits(FlagsByteOffset.Limit, 4));
			}
			set
			{
				limitLow = (ushort)(value & 0xFFFF);
				flags = flags.SetBits((byte)((value >> 16) & 0x0F), FlagsByteOffset.Limit, 4);
			}
		}

		public static void CopyTo(DTEntry* source, DTEntry* destination)
		{
			Memory.Copy((uint)source, (uint)destination, EntrySize);
		}

		public static void Clear(DTEntry* entry)
		{
			Memory.Clear((uint)entry, EntrySize);
		}

		private class AccessByteOffset
		{
			public const byte Ac = 0;
			public const byte RW = 1;
			public const byte DC = 2;
			public const byte Ex = 3;
			public const byte UserDescriptor = 4;
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
				return access.IsBitSet(AccessByteOffset.Pr);
			}
			set
			{
				access = access.SetBit(AccessByteOffset.Pr, value);
			}
		}

		public bool Executable
		{
			get
			{
				CheckSegment();
				return access.IsBitSet(AccessByteOffset.Ex);
			}
			set
			{
				CheckSegment();
				access = access.SetBit(AccessByteOffset.Ex, value);
			}
		}

		public bool IsUserType
		{
			get
			{
				return access.IsBitSet(AccessByteOffset.UserDescriptor);
			}
			set
			{
				access = access.SetBit(AccessByteOffset.UserDescriptor, value);
			}
		}

		private bool DirectionConfirming
		{
			get
			{
				return access.IsBitSet(AccessByteOffset.DC);
			}
			set
			{
				access = access.SetBit(AccessByteOffset.DC, value);
			}
		}

		public bool Confirming
		{
			get
			{
				return access.IsBitSet(AccessByteOffset.DC);
			}
			set
			{
				access = access.SetBit(AccessByteOffset.DC, value);
			}
		}

		public bool Direction_ExpandDown
		{
			get
			{
				return access.IsBitSet(AccessByteOffset.DC);
			}
			set
			{
				access = access.SetBit(AccessByteOffset.DC, value);
			}
		}

		public byte Type
		{
			get
			{
				return access.GetBits(AccessByteOffset.SegmentType, 4);
			}
			set
			{
				access = access.SetBits(AccessByteOffset.SegmentType, value, 4);
			}
		}

		public bool ReadWrite
		{
			get
			{
				CheckSegment();
				return access.IsBitSet(AccessByteOffset.RW);
			}
			set
			{
				CheckSegment();
				access = access.SetBit(AccessByteOffset.RW, value);
			}
		}

		public bool Accessed
		{
			get
			{
				CheckSegment();
				return access.IsBitSet(AccessByteOffset.Ac);
			}
			set
			{
				CheckSegment();

				access = access.SetBit(AccessByteOffset.Ac, value);
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
				return access.GetBits(AccessByteOffset.Privl, 2);
			}
			set
			{
				if (value > 3)
					Panic.Error("Privilege ring can't be larger than 3");

				access = access.SetBits(value, AccessByteOffset.Privl, 2);
			}
		}

		private void CheckSegment()
		{
			if (!IsUserType)
				Panic.Error("This attribute can't accessed with this segment type");
		}

		#endregion AccessByte

		#region Flags

		public bool Custom
		{
			get
			{
				return flags.IsBitSet(FlagsByteOffset.Custom);
			}
			set
			{
				flags = flags.SetBit(FlagsByteOffset.Custom, value);
			}
		}

		private bool LongMode
		{
			get
			{
				return flags.IsBitSet(FlagsByteOffset.LongMode);
			}
			set
			{
				flags = flags.SetBit(FlagsByteOffset.LongMode, value);
				if (value)
					SizeBit = false;
			}
		}

		private bool SizeBit
		{
			get
			{
				return flags.IsBitSet(FlagsByteOffset.Sz);
			}
			set
			{
				if (value && LongMode)
					Panic.Error("Size type invalid for long mode");

				flags = flags.SetBit(FlagsByteOffset.Sz, value);
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
				return flags.IsBitSet(FlagsByteOffset.Gr);
			}
			set
			{
				flags = flags.SetBit(FlagsByteOffset.Gr, value);
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
				+ ",Segment=" + this.IsUserType.ToChar()
				+ ",Cust=" + this.Custom.ToChar()
			;
			string seg = "";
			if (IsUserType)
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

	public interface IDTEntry
	{
		bool IsNullDescriptor { get; }

		uint BaseAddress { get; set; }

		uint Limit { get; set; }

		#region AccessByte

		bool Present { get; set; }

		bool IsUserType { get; set; }

		byte PriviligeRing { get; set; }

		#endregion AccessByte

		#region Flags

		bool Custom { get; set; }

		DTEntry.EAddressMode AddressMode { get; set; }

		bool Granularity { get; set; }

		#endregion Flags
	}

	public interface IDTUserDescriptor : IDTEntry
	{
		bool Accessed { get; set; }

		bool Executable { get; set; }
	}

	public interface IDTCodeSegment : IDTUserDescriptor
	{
		bool Readable { get; set; }

		bool Confirming { get; set; }
	}

	public interface IDTDataSegment : IDTUserDescriptor
	{
		bool Direction_ExpandDown { get; set; }

		bool Writable { get; set; }
	}

	public interface IDTSystemDescriptor : IDTEntry
	{
		byte Type { get; set; }
	}
}