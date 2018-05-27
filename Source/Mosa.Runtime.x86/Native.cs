// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.InteropServices;

namespace Mosa.Runtime.x86
{
	/// <summary>
	/// Provides stub methods for selected x86 native assembly instructions.
	/// </summary>
	public static unsafe class Native
	{
		#region Intrinsic

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Lidt, Mosa.Platform.x86")]
		public extern static void Lidt(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Cli, Mosa.Platform.x86")]
		public extern static void Cli();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Lgdt, Mosa.Platform.x86")]
		public extern static void Lgdt(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Sti, Mosa.Platform.x86")]
		public extern static void Sti();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.In8, Mosa.Platform.x86")]
		public extern static byte In8(ushort address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.In16, Mosa.Platform.x86")]
		public extern static ushort In16(ushort address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.In32, Mosa.Platform.x86")]
		public extern static uint In32(ushort address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Out8, Mosa.Platform.x86")]
		public extern static void Out8(ushort address, byte value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Out16, Mosa.Platform.x86")]
		public extern static void Out16(ushort address, ushort value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Out32, Mosa.Platform.x86")]
		public extern static void Out32(ushort address, uint value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Nop, Mosa.Platform.x86")]
		public extern static void Nop();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Hlt, Mosa.Platform.x86")]
		public extern static void Hlt();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Invlpg, Mosa.Platform.x86")]
		public extern static void Invlpg(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.CpuIdEax, Mosa.Platform.x86")]
		public extern static uint CpuIdEax(uint function);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.CpuIdEbx, Mosa.Platform.x86")]
		public extern static uint CpuIdEbx(uint function);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.CpuIdEcx, Mosa.Platform.x86")]
		public extern static uint CpuIdEcx(uint function);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.CpuIdEdx, Mosa.Platform.x86")]
		public extern static uint CpuIdEdx(uint function);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.BochsDebug, Mosa.Platform.x86")]
		public extern static void BochsDebug();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SetCR0, Mosa.Platform.x86")]
		public extern static void SetCR0(uint status);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SetCR2, Mosa.Platform.x86")]
		public extern static void SetCR2(uint status);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SetCR3, Mosa.Platform.x86")]
		public extern static void SetCR3(uint status);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SetCR4, Mosa.Platform.x86")]
		public extern static void SetCR4(uint status);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetCR0, Mosa.Platform.x86")]
		public extern static uint GetCR0();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetCR2, Mosa.Platform.x86")]
		public extern static uint GetCR2();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetCR3, Mosa.Platform.x86")]
		public extern static uint GetCR3();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetCR4, Mosa.Platform.x86")]
		public extern static uint GetCR4();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Get8, Mosa.Platform.x86")]
		public extern static byte Get8(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Get16, Mosa.Platform.x86")]
		public extern static ushort Get16(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Get32, Mosa.Platform.x86")]
		public extern static uint Get32(uint address);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Set8, Mosa.Platform.x86")]
		public extern static void Set8(uint address, byte value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Set16, Mosa.Platform.x86")]
		public extern static void Set16(uint address, ushort value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Set32, Mosa.Platform.x86")]
		public extern static void Set32(uint address, uint value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetEBP, Mosa.Platform.x86")]
		public extern static UIntPtr GetEBP();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetFS, Mosa.Platform.x86")]
		public extern static uint GetFS();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetCS, Mosa.Platform.x86")]
		public extern static uint GetCS();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.SetFS, Mosa.Platform.x86")]
		public extern static void SetFS(uint value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Div, Mosa.Platform.x86")]
		public extern static uint Div(ulong n, uint d);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.CmpXChgLoad32, Mosa.Platform.x86")]
		public extern static int CmpXChgLoad32(ref int location, int value, int comparand);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.XAddLoad32, Mosa.Platform.x86")]
		public extern static int XAddLoad32(ref int location, int value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.XChgLoad32, Mosa.Platform.x86")]
		public extern static int XChgLoad32(ref int location, int value);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Pause, Mosa.Platform.x86")]
		public extern static void Pause();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetExceptionRegister, Mosa.Platform.x86")]
		public extern static uint GetExceptionRegister();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetIDTJumpLocation, Mosa.Platform.x86")]
		public extern static uint GetIDTJumpLocation(uint irq);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetAssemblyListTable, Mosa.Platform.x86")]
		public extern static uint GetAssemblyListTable();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic.GetMethodLookupTable, Mosa.Platform.x86")]
		public extern static UIntPtr GetMethodLookupTable();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetMethodExceptionLookupTable, Mosa.Platform.x86")]
		public extern static UIntPtr GetMethodExceptionLookupTable();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetMultibootEAX, Mosa.Platform.x86")]
		public extern static uint GetMultibootEAX();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.GetMultibootEBX, Mosa.Platform.x86")]
		public extern static uint GetMultibootEBX();

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.FrameJump, Mosa.Platform.x86")]
		public extern static void FrameJump(UIntPtr eip, UIntPtr esp, UIntPtr ebp, uint exceptionRegister);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.FrameCall, Mosa.Platform.x86")]
		public extern static void FrameCall(uint eip);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.FrameCallRetU4, Mosa.Platform.x86")]
		public extern static uint FrameCallRetU4(uint eip);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.FrameCallRetU8, Mosa.Platform.x86")]
		public extern static ulong FrameCallRetU8(uint eip);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.FrameCallRetR8, Mosa.Platform.x86")]
		public extern static ulong FrameCallRetR8(uint eip);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.InterruptReturn, Mosa.Platform.x86")]
		public extern static void InterruptReturn(uint esp);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.AllocateStackSpace, Mosa.Platform.x86")]
		public extern static uint AllocateStackSpace(uint size);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Int, Mosa.Platform.x86")]
		public extern static void Int(byte interrupt);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Remainder, Mosa.Platform.x86")]
		public extern static float Remainder(float dividend, float divisor);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Remainder, Mosa.Platform.x86")]
		public extern static double Remainder(double dividend, double divisor);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Memcpy256, Mosa.Platform.x86")]
		public extern static void Memcpy256(void* destination, void* source);

		[DllImportAttribute(@"Mosa.Platform.x86.Intrinsic.Memclr256, Mosa.Platform.x86")]
		public extern static void Memclr256(void* destination);

		#endregion Intrinsic
	}
}
