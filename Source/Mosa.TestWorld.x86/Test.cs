// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.TestWorld.x86
{
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
}
