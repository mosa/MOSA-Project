// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime.x64
{
	/// <summary>
	/// Provides stub methods for selected x64 native assembly instructions.
	/// </summary>
	public static unsafe class Native
	{
		#region Intrinsic

		[DllImport("Mosa.Platform.x64.Intrinsic::Lidt")]
		public extern static void Lidt(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Cli")]
		public extern static void Cli();

		[DllImport("Mosa.Platform.x64.Intrinsic::Lgdt")]
		public extern static void Lgdt(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Sti")]
		public extern static void Sti();

		[DllImport("Mosa.Platform.x64.Intrinsic::In8")]
		public extern static byte In8(ushort address);

		[DllImport("Mosa.Platform.x64.Intrinsic::In16")]
		public extern static ushort In16(ushort address);

		[DllImport("Mosa.Platform.x64.Intrinsic::In32")]
		public extern static uint In32(ushort address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Out8")]
		public extern static void Out8(ushort address, byte value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Out16")]
		public extern static void Out16(ushort address, ushort value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Out32")]
		public extern static void Out32(ushort address, uint value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Nop")]
		public extern static void Nop();

		[DllImport("Mosa.Platform.x64.Intrinsic::Hlt")]
		public extern static void Hlt();

		[DllImport("Mosa.Platform.x64.Intrinsic::Invlpg")]
		public extern static void Invlpg(uint address);

		[DllImport("Mosa.Platform.x64.Intrinsic::CpuIdEax")]
		public extern static uint CpuIdEax(uint function);

		[DllImport("Mosa.Platform.x64.Intrinsic::CpuIdEbx")]
		public extern static uint CpuIdEbx(uint function);

		[DllImport("Mosa.Platform.x64.Intrinsic::CpuIdEcx")]
		public extern static uint CpuIdEcx(uint function);

		[DllImport("Mosa.Platform.x64.Intrinsic::CpuIdEdx")]
		public extern static uint CpuIdEdx(uint function);

		[DllImport("Mosa.Platform.x64.Intrinsic::BochsDebug")]
		public extern static void BochsDebug();

		[DllImport("Mosa.Platform.x64.Intrinsic::SetCR0")]
		public extern static void SetCR0(ulong status);

		[DllImport("Mosa.Platform.x64.Intrinsic::SetCR2")]
		public extern static void SetCR2(ulong status);

		[DllImport("Mosa.Platform.x64.Intrinsic::SetCR3")]
		public extern static void SetCR3(ulong status);

		[DllImport("Mosa.Platform.x64.Intrinsic::SetCR4")]
		public extern static void SetCR4(ulong status);

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCR0")]
		public extern static uint GetCR0();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCR2")]
		public extern static uint GetCR2();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCR3")]
		public extern static uint GetCR3();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCR4")]
		public extern static uint GetCR4();

		[DllImport("Mosa.Platform.x64.Intrinsic::Get8")]
		public extern static byte Get8(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Get16")]
		public extern static ushort Get16(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Get32")]
		public extern static uint Get32(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Set8")]
		public extern static void Set8(ulong address, byte value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Set16")]
		public extern static void Set16(ulong address, ushort value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Set32")]
		public extern static void Set32(ulong address, uint value);

		[DllImport("Mosa.Platform.x64.Intrinsic::GetEBP")]
		public extern static Pointer GetEBP();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetFS")]
		public extern static ulong GetFS();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCS")]
		public extern static ulong GetCS();

		[DllImport("Mosa.Platform.x64.Intrinsic::SetFS")]
		public extern static void SetFS(ulong value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Div")]
		public extern static uint Div(ulong n, uint d);

		[DllImport("Mosa.Platform.x64.Intrinsic::CmpXChgLoad64")]
		public extern static long CmpXChgLoad64(ref long location, long value, long comparand);

		[DllImport("Mosa.Platform.x64.Intrinsic::CmpXChgLoad64")]
		public extern static long CmpXChgLoad64(long location, long value, long comparand);

		[DllImport("Mosa.Platform.x64.Intrinsic::XAddLoad64")]
		public extern static long XAddLoad64(ref long location, long value);

		[DllImport("Mosa.Platform.x64.Intrinsic::XAddLoad64")]
		public extern static long XAddLoad64(long location, long value);

		[DllImport("Mosa.Platform.x64.Intrinsic::XChgLoad64")]
		public extern static long XChgLoad64(ref long location, long value);

		[DllImport("Mosa.Platform.x64.Intrinsic::XChgLoad64")]
		public extern static long XChgLoad64(long location, long value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Pause")]
		public extern static void Pause();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetExceptionRegister")]
		public extern static long GetExceptionRegister();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetIDTJumpLocation")]
		public extern static long GetIDTJumpLocation(uint irq);

		[DllImport("Mosa.Platform.x64.Intrinsic::GetAssemblyListTable")]
		public extern static long GetAssemblyListTable();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetMethodLookupTable")]
		public extern static Pointer GetMethodLookupTable();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetMethodExceptionLookupTable")]
		public extern static Pointer GetMethodExceptionLookupTable();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetMultibootEAX")]
		public extern static ulong GetMultibootEAX();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetMultibootEBX")]
		public extern static ulong GetMultibootEBX();

		[DllImport("Mosa.Platform.x64.Intrinsic::FrameJump")]
		public extern static void FrameJump(Pointer eip, Pointer esp, Pointer ebp, int exceptionRegister);

		[DllImport("Mosa.Platform.x64.Intrinsic::FrameCall")]
		public extern static void FrameCall(ulong eip);

		[DllImport("Mosa.Platform.x64.Intrinsic::FrameCallRetU4")]
		public extern static ulong FrameCallRetU4(ulong eip);

		[DllImport("Mosa.Platform.x64.Intrinsic::FrameCallRetU8")]
		public extern static ulong FrameCallRetU8(ulong eip);

		[DllImport("Mosa.Platform.x64.Intrinsic::FrameCallRetR8")]
		public extern static ulong FrameCallRetR8(ulong eip);

		[DllImport("Mosa.Platform.x64.Intrinsic::InterruptReturn")]
		public extern static void InterruptReturn(ulong esp);

		[DllImport("Mosa.Platform.x64.Intrinsic::AllocateStackSpace")]
		public extern static uint AllocateStackSpace(ulong size);

		[DllImport("Mosa.Platform.x64.Intrinsic::Int")]
		public extern static void Int(byte interrupt);

		[DllImport("Mosa.Platform.x64.Intrinsic::Remainder")]
		public extern static float Remainder(float dividend, float divisor);

		[DllImport("Mosa.Platform.x64.Intrinsic::Remainder")]
		public extern static double Remainder(double dividend, double divisor);

		[DllImport("Mosa.Platform.x64.Intrinsic::Memcpy256")]
		public extern static void Memcpy256(void* destination, void* source);

		[DllImport("Mosa.Platform.x64.Intrinsic::Memclr256")]
		public extern static void Memclr256(void* destination);

		#endregion Intrinsic
	}
}
