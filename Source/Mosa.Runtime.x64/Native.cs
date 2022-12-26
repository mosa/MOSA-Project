// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime.x64
{
	/// <summary>
	/// Provides stub methods for selected x64 native assembly instructions.
	/// </summary>
	public static unsafe class Native
	{
		#region Intrinsic Instructions

		[DllImport("Mosa.Platform.x64.Intrinsic::Lidt")]
		public static extern void Lidt(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Cli")]
		public static extern void Cli();

		[DllImport("Mosa.Platform.x64.Intrinsic::Lgdt")]
		public static extern void Lgdt(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Sti")]
		public static extern void Sti();

		[DllImport("Mosa.Platform.x64.Intrinsic::In8")]
		public static extern byte In8(ushort address);

		[DllImport("Mosa.Platform.x64.Intrinsic::In16")]
		public static extern ushort In16(ushort address);

		[DllImport("Mosa.Platform.x64.Intrinsic::In32")]
		public static extern uint In32(ushort address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Out8")]
		public static extern void Out8(ushort address, byte value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Out16")]
		public static extern void Out16(ushort address, ushort value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Out32")]
		public static extern void Out32(ushort address, uint value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Nop")]
		public static extern void Nop();

		[DllImport("Mosa.Platform.x64.Intrinsic::Hlt")]
		public static extern void Hlt();

		[DllImport("Mosa.Platform.x64.Intrinsic::Invlpg")]
		public static extern void Invlpg(uint address);

		[DllImport("Mosa.Platform.x64.Intrinsic::CpuIdEax")]
		public static extern uint CpuIdEax(uint function);

		[DllImport("Mosa.Platform.x64.Intrinsic::CpuIdEbx")]
		public static extern uint CpuIdEbx(uint function);

		[DllImport("Mosa.Platform.x64.Intrinsic::CpuIdEcx")]
		public static extern uint CpuIdEcx(uint function);

		[DllImport("Mosa.Platform.x64.Intrinsic::CpuIdEdx")]
		public static extern uint CpuIdEdx(uint function);

		[DllImport("Mosa.Platform.x64.Intrinsic::Pause")]
		public static extern void Pause();

		[DllImport("Mosa.Platform.x64.Intrinsic::Int")]
		public static extern void Int(byte interrupt);

		[DllImport("Mosa.Platform.x86.Intrinsic::Blsr32")]
		public static extern uint Blsr32(uint esp);

		[DllImport("Mosa.Platform.x64.Intrinsic::Blsr64")]
		public static extern uint Blsr64(uint esp);

		//[DllImport("Mosa.Platform.x64.Intrinsic::Popcnt32")]
		//public static extern uint Popcnt32(uint esp);

		//[DllImport("Mosa.Platform.x64.Intrinsic::Popcnt64")]
		//public static extern uint Popcnt64(uint esp);

		//[DllImport("Mosa.Platform.x64.Intrinsic::Lzcnt32")]
		//public static extern uint Lzcnt32(uint esp);

		//[DllImport("Mosa.Platform.x64.Intrinsic::Lzcnt64")]
		//public static extern uint Lzcnt64(uint esp);

		//[DllImport("Mosa.Platform.x86.Intrinsic::Tzcnt32")]
		//public static extern uint Tzcnt32(uint esp);

		//[DllImport("Mosa.Platform.x86.Intrinsic::Tzcnt64")]
		//public static extern uint Tzcnt64(uint esp);

		#endregion Intrinsic Instructions

		#region Intrinsic Methods

		[DllImport("Mosa.Platform.x64.Intrinsic::BochsDebug")]
		public static extern void BochsDebug();

		[DllImport("Mosa.Platform.x64.Intrinsic::SetCR0")]
		public static extern void SetCR0(ulong status);

		[DllImport("Mosa.Platform.x64.Intrinsic::SetCR2")]
		public static extern void SetCR2(ulong status);

		[DllImport("Mosa.Platform.x64.Intrinsic::SetCR3")]
		public static extern void SetCR3(ulong status);

		[DllImport("Mosa.Platform.x64.Intrinsic::SetCR4")]
		public static extern void SetCR4(ulong status);

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCR0")]
		public static extern uint GetCR0();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCR2")]
		public static extern uint GetCR2();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCR3")]
		public static extern uint GetCR3();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCR4")]
		public static extern uint GetCR4();

		[DllImport("Mosa.Platform.x64.Intrinsic::Get8")]
		public static extern byte Get8(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Get16")]
		public static extern ushort Get16(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Get32")]
		public static extern uint Get32(ulong address);

		[DllImport("Mosa.Platform.x64.Intrinsic::Set8")]
		public static extern void Set8(ulong address, byte value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Set16")]
		public static extern void Set16(ulong address, ushort value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Set32")]
		public static extern void Set32(ulong address, uint value);

		[DllImport("Mosa.Platform.x64.Intrinsic::GetFS")]
		public static extern ulong GetFS();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetCS")]
		public static extern ulong GetCS();

		[DllImport("Mosa.Platform.x64.Intrinsic::SetFS")]
		public static extern void SetFS(ulong value);

		[DllImport("Mosa.Platform.x64.Intrinsic::Div")]
		public static extern uint Div(ulong n, uint d);

		[DllImport("Mosa.Platform.x64.Intrinsic::CmpXChgLoad64")]
		public static extern long CmpXChgLoad64(ref long location, long value, long comparand);

		[DllImport("Mosa.Platform.x64.Intrinsic::CmpXChgLoad64")]
		public static extern long CmpXChgLoad64(long location, long value, long comparand);

		[DllImport("Mosa.Platform.x64.Intrinsic::XAddLoad64")]
		public static extern long XAddLoad64(ref long location, long value);

		[DllImport("Mosa.Platform.x64.Intrinsic::XAddLoad64")]
		public static extern long XAddLoad64(long location, long value);

		[DllImport("Mosa.Platform.x64.Intrinsic::XChgLoad64")]
		public static extern long XChgLoad64(ref long location, long value);

		[DllImport("Mosa.Platform.x64.Intrinsic::XChgLoad64")]
		public static extern long XChgLoad64(long location, long value);

		[DllImport("Mosa.Platform.x64.Intrinsic::GetIDTJumpLocation")]
		public static extern long GetIDTJumpLocation(uint irq);

		[DllImport("Mosa.Platform.x64.Intrinsic::GetMultibootEAX")]
		public static extern ulong GetMultibootEAX();

		[DllImport("Mosa.Platform.x64.Intrinsic::GetMultibootEBX")]
		public static extern ulong GetMultibootEBX();

		[DllImport("Mosa.Platform.x64.Intrinsic::FrameJump")]
		public static extern void FrameJump(Pointer eip, Pointer esp, Pointer ebp, int exceptionRegister);

		[DllImport("Mosa.Platform.x64.Intrinsic::FrameCall")]
		public static extern void FrameCall(ulong eip);

		[DllImport("Mosa.Platform.x64.Intrinsic::FrameCallRetU8")]
		public static extern ulong FrameCallRetU8(ulong eip);

		[DllImport("Mosa.Platform.x64.Intrinsic::FrameCallRetR8")]
		public static extern ulong FrameCallRetR8(ulong eip);

		[DllImport("Mosa.Platform.x64.Intrinsic::InterruptReturn")]
		public static extern void InterruptReturn(ulong esp);

		[DllImport("Mosa.Platform.x64.Intrinsic::AllocateStackSpace")]
		public static extern uint AllocateStackSpace(ulong size);

		[DllImport("Mosa.Platform.x64.Intrinsic::Remainder")]
		public static extern float Remainder(float dividend, float divisor);

		[DllImport("Mosa.Platform.x64.Intrinsic::Remainder")]
		public static extern double Remainder(double dividend, double divisor);

		[DllImport("Mosa.Platform.x64.Intrinsic::Memcpy256")]
		public static extern void Memcpy256(void* destination, void* source);

		[DllImport("Mosa.Platform.x64.Intrinsic::Memclr256")]
		public static extern void Memclr256(void* destination);

		#endregion Intrinsic Methods
	}
}
