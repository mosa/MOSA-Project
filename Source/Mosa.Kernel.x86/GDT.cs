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
using Mosa.Kernel.x86.Helpers;
using Mosa.Platform.Internal.x86;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	unsafe public static class GDT
	{
		private static uint gdtTableAddress = 0x1401000;
		private static DescriptorTable* table;

		/// <summary>
		/// Sets up the GDT table and entries
		/// </summary>
		public static void Setup()
		{
			table = (DescriptorTable*)gdtTableAddress;
			table->Clear();
			table->AdressOfEntries = gdtTableAddress + DescriptorTable.StructSize;

			//Null segment
			var nullEntry = DescriptorTableEntry.CreateNullDescriptor();
			table->AddEntry(nullEntry);

			//code segment
			var codeEntry = DescriptorTableEntry.CreateCode(0, 0xFFFFFFFF);
			codeEntry.CodeSegment_Readable = true;
			codeEntry.PriviligeRing = 0;
			codeEntry.Present = true;
			codeEntry.AddressMode = DescriptorTableEntry.EAddressMode.Bits32;
			codeEntry.Granularity = true;
			table->AddEntry(codeEntry);

			//data segment
			var dataEntry = DescriptorTableEntry.CreateData(0, 0xFFFFFFFF);
			dataEntry.DataSegment_Writable = true;
			dataEntry.PriviligeRing = 0;
			dataEntry.Present = true;
			dataEntry.AddressMode = DescriptorTableEntry.EAddressMode.Bits32;
			dataEntry.Granularity = true;
			table->AddEntry(dataEntry);

			Flush();
		}

		/// <summary>
		/// Flushes the GDT table
		/// </summary>
		public static void Flush()
		{
			Native.Lgdt(gdtTableAddress);
		}
	}

	/// <summary>
	/// Global Descriptor Table and Local Descriptor Table
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	unsafe public struct DescriptorTable
	{
		[FieldOffset(0)]
		private ushort size;

		[FieldOffset(2)]
		public uint AdressOfEntries;

		public const byte StructSize = 0x06;

		private DescriptorTableEntry* entries
		{
			get { return (DescriptorTableEntry*)AdressOfEntries; }
			set { AdressOfEntries = (uint)value; }
		}

		internal DescriptorTableEntry* GetEntryRef(ushort index)
		{
			Assert.InRange(index, Length);
			return entries + index;
		}

		public ushort Length
		{
			get
			{
				if (size == 0)
					return 0;
				else
					return (ushort)((size + 1) / DescriptorTableEntry.EntrySize);
			}
			private set
			{
				if (value == 0)
					size = 0;
				else
					size = (ushort)(value * DescriptorTableEntry.EntrySize - 1);
			}
		}

		public void Clear()
		{
			Length = 0;
		}

		public void AddEntry(DescriptorTableEntry source)
		{
			Length++;
			SetEntry((ushort)(Length - 1), source);
		}

		public void SetEntry(ushort index, DescriptorTableEntry source)
		{
			Assert.InRange(index, Length);
			DescriptorTableEntry.CopyTo(&source, entries + index);
		}

		public DescriptorTableEntry GetEntry(ushort index)
		{
			Assert.InRange(index, Length);
			return *(entries + index);
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	unsafe public struct DescriptorTableEntry
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

		public static DescriptorTableEntry CreateNullDescriptor()
		{
			return new DescriptorTableEntry();
		}

		private static DescriptorTableEntry Create(uint baseAddress, uint limit)
		{
			return new DescriptorTableEntry() { BaseAddress = baseAddress, Limit = limit };
		}

		public static DescriptorTableEntry CreateCode(uint baseAddress, uint limit)
		{
			var seg = Create(baseAddress, limit);
			seg.IsUserType = true;
			seg.UserDescriptor_Executable = true;
			return seg;
		}

		public static DescriptorTableEntry CreateData(uint baseAddress, uint limit)
		{
			var seg = Create(baseAddress, limit);
			seg.IsUserType = true;
			return seg;
		}

		public bool IsNullDescriptor
		{
			get { return limitLow == 0 && baseLow == 0 && baseMiddle == 0 && access == 0 && flags == 0 && baseHigh == 0; }
		}

		public uint BaseAddress
		{
			get { return (uint)(baseLow | (baseMiddle << 16) | (baseHigh << 24)); }
			set
			{
				baseLow = (ushort)(value & 0xFFFF);
				baseMiddle = (byte)((value >> 16) & 0xFF);
				baseHigh = (byte)((value >> 24) & 0xFF);
			}
		}

		public uint Limit
		{
			get { return (uint)(limitLow | flags.GetBits(FlagsByteOffset.Limit, 4)); }
			set
			{
				limitLow = (ushort)(value & 0xFFFF);
				flags = flags.SetBits(FlagsByteOffset.Limit, 4, (byte)((value >> 16) & 0x0F));
			}
		}

		public static void CopyTo(DescriptorTableEntry* source, DescriptorTableEntry* destination)
		{
			Memory.Copy((uint)source, (uint)destination, EntrySize);
		}

		public static void Clear(DescriptorTableEntry* entry)
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
			get { return access.IsBitSet(AccessByteOffset.Pr); }
			set { access = access.SetBit(AccessByteOffset.Pr, value); }
		}

		public bool UserDescriptor_Executable
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
			get { return access.IsBitSet(AccessByteOffset.UserDescriptor); }
			set { access = access.SetBit(AccessByteOffset.UserDescriptor, value); }
		}

		private bool DirectionConfirming
		{
			get { return access.IsBitSet(AccessByteOffset.DC); }
			set { access = access.SetBit(AccessByteOffset.DC, value); }
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

		public bool UserDescriptor_Accessed
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

		#region UserSegment

		#region CodeSegment

		public bool CodeSegment_Readable
		{
			get
			{
				CheckSegment();
				return UserDescriptor_Executable ? ReadWrite : true;
			}
			set
			{
				CheckSegment();
				Assert.False(!UserDescriptor_Executable && !value, "Read access is always allowed for data segments");

				ReadWrite = value;
			}
		}

		public bool CodeSegment_Confirming
		{
			get { return access.IsBitSet(AccessByteOffset.DC); }
			set { access = access.SetBit(AccessByteOffset.DC, value); }
		}

		#endregion CodeSegment

		#region DataSegment

		public bool DataSegment_Writable
		{
			get { return UserDescriptor_Executable ? ReadWrite : false; }
			set
			{
				Assert.False(UserDescriptor_Executable && value, "Write access is never allowed for code segments");

				ReadWrite = value;
			}
		}

		public bool DataSegment_Direction_ExpandDown
		{
			get { return access.IsBitSet(AccessByteOffset.DC); }
			set { access = access.SetBit(AccessByteOffset.DC, value); }
		}

		#endregion DataSegment

		#endregion UserSegment

		#region SystemDescriptor

		public byte SystemDescriptor_Type
		{
			get { return access.GetBits(AccessByteOffset.SegmentType, 4); }
			set { access = access.SetBits(AccessByteOffset.SegmentType, 4, value); }
		}

		#endregion SystemDescriptor

		public byte PriviligeRing
		{
			get { return access.GetBits(AccessByteOffset.Privl, 2); }
			set
			{
				Assert.False(value > 3, "Privilege ring can't be larger than 3");
				access = access.SetBits(AccessByteOffset.Privl, 2, value);
			}
		}

		private void CheckSegment()
		{
			Assert.True(IsUserType, "This attribute can't accessed with this segment type");
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
				Assert.False(value && LongMode, "Size type invalid for long mode");

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
					+ ",Exec=" + this.UserDescriptor_Executable.ToChar()
					+ ",RW=" + ReadWrite.ToChar()
					+ ",AC=" + UserDescriptor_Accessed.ToChar()
					+ ",DC=" + this.DirectionConfirming.ToChar()
				;
			}
			return s + seg;
		}
	}
}