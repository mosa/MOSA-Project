// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime.x64
{
	/// <summary>
	/// Provides stub methods for selected x64 native assembly instructions.
	/// </summary>
	public static unsafe class Native
	{
		#region Intrinsic

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Lidt, Mosa.Platform.x64")]
		public extern static void Lidt(ulong address);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Cli, Mosa.Platform.x64")]
		public extern static void Cli();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Lgdt, Mosa.Platform.x64")]
		public extern static void Lgdt(ulong address);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Sti, Mosa.Platform.x64")]
		public extern static void Sti();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.In8, Mosa.Platform.x64")]
		public extern static byte In8(ushort address);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.In16, Mosa.Platform.x64")]
		public extern static ushort In16(ushort address);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.In32, Mosa.Platform.x64")]
		public extern static uint In32(ushort address);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Out8, Mosa.Platform.x64")]
		public extern static void Out8(ushort address, byte value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Out16, Mosa.Platform.x64")]
		public extern static void Out16(ushort address, ushort value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Out32, Mosa.Platform.x64")]
		public extern static void Out32(ushort address, uint value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Nop, Mosa.Platform.x64")]
		public extern static void Nop();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Hlt, Mosa.Platform.x64")]
		public extern static void Hlt();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Invlpg, Mosa.Platform.x64")]
		public extern static void Invlpg(uint address);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.CpuIdEax, Mosa.Platform.x64")]
		public extern static uint CpuIdEax(uint function);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.CpuIdEbx, Mosa.Platform.x64")]
		public extern static uint CpuIdEbx(uint function);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.CpuIdEcx, Mosa.Platform.x64")]
		public extern static uint CpuIdEcx(uint function);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.CpuIdEdx, Mosa.Platform.x64")]
		public extern static uint CpuIdEdx(uint function);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.BochsDebug, Mosa.Platform.x64")]
		public extern static void BochsDebug();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.SetCR0, Mosa.Platform.x64")]
		public extern static void SetCR0(ulong status);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.SetCR2, Mosa.Platform.x64")]
		public extern static void SetCR2(ulong status);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.SetCR3, Mosa.Platform.x64")]
		public extern static void SetCR3(ulong status);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.SetCR4, Mosa.Platform.x64")]
		public extern static void SetCR4(ulong status);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetCR0, Mosa.Platform.x64")]
		public extern static uint GetCR0();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetCR2, Mosa.Platform.x64")]
		public extern static uint GetCR2();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetCR3, Mosa.Platform.x64")]
		public extern static uint GetCR3();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetCR4, Mosa.Platform.x64")]
		public extern static uint GetCR4();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Get8, Mosa.Platform.x64")]
		public extern static byte Get8(ulong address);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Get16, Mosa.Platform.x64")]
		public extern static ushort Get16(ulong address);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Get32, Mosa.Platform.x64")]
		public extern static uint Get32(ulong address);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Set8, Mosa.Platform.x64")]
		public extern static void Set8(ulong address, byte value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Set16, Mosa.Platform.x64")]
		public extern static void Set16(ulong address, ushort value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Set32, Mosa.Platform.x64")]
		public extern static void Set32(ulong address, uint value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetEBP, Mosa.Platform.x64")]
		public extern static IntPtr GetEBP();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetFS, Mosa.Platform.x64")]
		public extern static ulong GetFS();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetCS, Mosa.Platform.x64")]
		public extern static ulong GetCS();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.SetFS, Mosa.Platform.x64")]
		public extern static void SetFS(ulong value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Div, Mosa.Platform.x64")]
		public extern static uint Div(ulong n, uint d);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.CmpXChgLoad64, Mosa.Platform.x64")]
		public extern static long CmpXChgLoad64(ref long location, long value, long comparand);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.CmpXChgLoad64, Mosa.Platform.x64")]
		public extern static long CmpXChgLoad64(long location, long value, long comparand);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.XAddLoad64, Mosa.Platform.x64")]
		public extern static long XAddLoad64(ref long location, long value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.XAddLoad64, Mosa.Platform.x64")]
		public extern static long XAddLoad64(long location, long value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.XChgLoad64, Mosa.Platform.x64")]
		public extern static long XChgLoad64(ref long location, long value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.XChgLoad64, Mosa.Platform.x64")]
		public extern static long XChgLoad64(long location, long value);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Pause, Mosa.Platform.x64")]
		public extern static void Pause();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetExceptionRegister, Mosa.Platform.x64")]
		public extern static long GetExceptionRegister();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetIDTJumpLocation, Mosa.Platform.x64")]
		public extern static long GetIDTJumpLocation(uint irq);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetAssemblyListTable, Mosa.Platform.x64")]
		public extern static long GetAssemblyListTable();

		[DllImportAttribute("Mosa.Platform.x64.Intrinsic.GetMethodLookupTable, Mosa.Platform.x64")]
		public extern static IntPtr GetMethodLookupTable();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetMethodExceptionLookupTable, Mosa.Platform.x64")]
		public extern static IntPtr GetMethodExceptionLookupTable();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetMultibootEAX, Mosa.Platform.x64")]
		public extern static ulong GetMultibootEAX();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.GetMultibootEBX, Mosa.Platform.x64")]
		public extern static ulong GetMultibootEBX();

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.FrameJump, Mosa.Platform.x64")]
		public extern static void FrameJump(IntPtr eip, IntPtr esp, IntPtr ebp, int exceptionRegister);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.FrameCall, Mosa.Platform.x64")]
		public extern static void FrameCall(ulong eip);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.FrameCallRetU4, Mosa.Platform.x64")]
		public extern static ulong FrameCallRetU4(ulong eip);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.FrameCallRetU8, Mosa.Platform.x64")]
		public extern static ulong FrameCallRetU8(ulong eip);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.FrameCallRetR8, Mosa.Platform.x64")]
		public extern static ulong FrameCallRetR8(ulong eip);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.InterruptReturn, Mosa.Platform.x64")]
		public extern static void InterruptReturn(ulong esp);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.AllocateStackSpace, Mosa.Platform.x64")]
		public extern static uint AllocateStackSpace(ulong size);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Int, Mosa.Platform.x64")]
		public extern static void Int(byte interrupt);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Remainder, Mosa.Platform.x64")]
		public extern static float Remainder(float dividend, float divisor);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Remainder, Mosa.Platform.x64")]
		public extern static double Remainder(double dividend, double divisor);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Memcpy256, Mosa.Platform.x64")]
		public extern static void Memcpy256(void* destination, void* source);

		[DllImportAttribute(@"Mosa.Platform.x64.Intrinsic.Memclr256, Mosa.Platform.x64")]
		public extern static void Memclr256(void* destination);

		#endregion Intrinsic
	}
}
