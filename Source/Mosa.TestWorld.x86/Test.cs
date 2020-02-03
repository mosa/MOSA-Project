// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Mosa.TestWorld.x86
{
	public struct USize
	{
		public static readonly USize Zero;

		public static unsafe int Size
		{
			[NonVersionable]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return sizeof(void*);
			}
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe USize(uint value)
		{
			_value = (void*)value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe USize(int value)
		{
			_value = (void*)value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe USize(ulong value)
		{
			_value = (void*)(uint)value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe USize(long value)
		{
			_value = (void*)value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe USize(void* value)
		{
			_value = value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static USize Add(USize pointer, int offset)
		{
			return pointer + offset;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe implicit operator uint(USize value)
		{
			return (uint)value._value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe implicit operator ulong(USize value)
		{
			return (ulong)value._value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator USize(uint value)
		{
			return new USize(value);
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator USize(ulong value)
		{
			return new USize(value);
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CA2225 // Operator overloads have named alternates
		public static unsafe implicit operator USize(void* value)
#pragma warning restore CA2225 // Operator overloads have named alternates
		{
			return new USize(value);
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CA2225 // Operator overloads have named alternates
		public static unsafe implicit operator void*(USize value)
#pragma warning restore CA2225 // Operator overloads have named alternates
		{
			return value._value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe USize operator -(USize pointer, int offset)
		{
			return new USize((ulong)((long)pointer._value - offset));
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe bool operator !=(USize value1, USize value2)
		{
			return value1._value != value2._value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe USize operator +(USize pointer, int offset)
		{
			return new USize((ulong)((long)pointer._value + offset));
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe bool operator ==(USize value1, USize value2)
		{
			return value1._value == value2._value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static USize Subtract(USize pointer, int offset)
		{
			return pointer - offset;
		}

		public unsafe override bool Equals(object obj)
		{
			if (obj is USize)
			{
				return _value == ((USize)obj)._value;
			}
			return false;
		}

		public unsafe override int GetHashCode()
		{
			return (int)_value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void* ToPointer()
		{
			return _value;
		}

		public unsafe override string ToString()
		{
			return ((long)_value).ToString();
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe uint ToUInt32()
		{
			return (uint)_value;
		}

		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ulong ToUInt64()
		{
			return (ulong)_value;
		}

		private unsafe void* _value; // Do not rename (binary serialization)

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static USize FromUInt32(uint value) => new USize(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static USize FromInt32(int value) => new USize(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static USize FromUInt64(ulong value) => new USize(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static USize FromInt64(long value) => new USize(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe int ToInt32() => (int)_value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe long ToInt64() => (long)_value;
	}

	internal class Test
	{
		public static ulong KernelBootStartCycles { get; private set; }

		public static void Setup()
		{
			//KernelBootStartCycles = CpuCyclesSinceSystemBoot();
		}

		public static void Setup(ulong kernelBootStartCycles)
		{
			// It will generate invalid opcode!!
			KernelBootStartCycles = kernelBootStartCycles;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ulong CpuCyclesSinceSystemBoot()
		{
			return 0;
		}

		public static ulong CpuCyclesSinceKernelBoot()
		{
			//return CpuCyclesSinceSystemBoot() - KernelBootStartCycles;
			return CpuCyclesSinceSystemBoot() - 12500000000L;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct StringBuffer
	{
		private uint _length;

		public const int MaxLength = 132;
		public const int EntrySize = (MaxLength * 2) + 4;

		private unsafe fixed byte _chars[MaxLength];

		public static unsafe StringBuffer CreateFromNullTerminatedString(uint start)
		{
			return CreateFromNullTerminatedString((byte*)start);
		}

		public static unsafe StringBuffer CreateFromNullTerminatedString(byte* start)
		{
			var buf = new StringBuffer();
			while (*start != 0)
			{
				buf.Append((char)*start++);
			}
			return buf;
		}

		public unsafe void Append(char value)
		{
			if (_length + 1 >= MaxLength)
			{
				//TODO: Error
				return;
			}

			//isSet = 1;
			_length++;
			this[_length - 1] = value;
		}

		public unsafe char this[uint index]
		{
			get
			{
				if (index >= Length) //TODO: Error
					return '\x0';
				fixed (byte* ptr = _chars)
					return (char)ptr[index];
			}

			set
			{
				if (index >= Length) //TODO: Error
					return;
				fixed (byte* ptr = _chars)
					ptr[index] = (byte)value;
			}
		}

		public uint Length
		{
			get
			{
				return _length;
			}

			set
			{
				if (value > MaxLength)
				{
					//TODO: Error
					value = MaxLength;
				}
				_length = value;
			}
		}
	}

	public static class Test2
	{
		private static unsafe void InlineTest()
		{
			USize addr = 0x1000;
			USize addr2 = 0x1000u;
			USize addr3 = addr + addr2;
		}
	}
}
