// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime.x64;

/// <summary>
/// Provides stub methods for selected x64 native assembly instructions.
/// </summary>
public static unsafe class Native
{
	#region Intrinsic Instructions

	[DllImport("Mosa.Compiler.x64.Intrinsic::Lidt")]
	public static extern void Lidt(ulong address);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Cli")]
	public static extern void Cli();

	[DllImport("Mosa.Compiler.x64.Intrinsic::Lgdt")]
	public static extern void Lgdt(ulong address);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Sti")]
	public static extern void Sti();

	[DllImport("Mosa.Compiler.x64.Intrinsic::In8")]
	public static extern byte In8(ushort address);

	[DllImport("Mosa.Compiler.x64.Intrinsic::In16")]
	public static extern ushort In16(ushort address);

	[DllImport("Mosa.Compiler.x64.Intrinsic::In32")]
	public static extern uint In32(ushort address);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Out8")]
	public static extern void Out8(ushort address, byte value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Out16")]
	public static extern void Out16(ushort address, ushort value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Out32")]
	public static extern void Out32(ushort address, uint value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Nop")]
	public static extern void Nop();

	[DllImport("Mosa.Compiler.x64.Intrinsic::Hlt")]
	public static extern void Hlt();

	[DllImport("Mosa.Compiler.x64.Intrinsic::Invlpg")]
	public static extern void Invlpg(ulong address);

	[DllImport("Mosa.Compiler.x64.Intrinsic::CpuIdRAX")]
	public static extern ulong CpuIdRAX(ulong eax, ulong rcx);

	[DllImport("Mosa.Compiler.x64.Intrinsic::CpuIdRBX")]
	public static extern ulong CpuIdRBX(ulong eax, ulong rcx);

	[DllImport("Mosa.Compiler.x64.Intrinsic::CpuIdRCX")]
	public static extern ulong CpuIdRCX(ulong eax, ulong rcx);

	[DllImport("Mosa.Compiler.x64.Intrinsic::CpuIdRDX")]
	public static extern ulong CpuIdRDX(ulong eax, ulong rcx);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Pause")]
	public static extern void Pause();

	[DllImport("Mosa.Compiler.x64.Intrinsic::Int")]
	public static extern void Int(byte interrupt);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Blsr32")]
	public static extern uint Blsr32(uint esp);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Blsr64")]
	public static extern uint Blsr64(uint esp);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Sqrtsd")]
	public static extern double Sqrtsd(double destination);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Sqrtss")]
	public static extern float Sqrtss(float destination);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Roundsd2Negative")]
	public static extern double Roundsd2Negative(double destination);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Roundsd2Positive")]
	public static extern double Roundsd2Positive(double destination);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Roundss2Negative")]
	public static extern float Roundss2Negative(float destination);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Roundss2Positive")]
	public static extern float Roundss2Positive(float destination);

	//[DllImport("Mosa.Compiler.x64.Intrinsic::Popcnt32")]
	//public static extern uint Popcnt32(uint rsp);

	//[DllImport("Mosa.Compiler.x64.Intrinsic::Popcnt64")]
	//public static extern uint Popcnt64(uint rsp);

	//[DllImport("Mosa.Compiler.x64.Intrinsic::Lzcnt32")]
	//public static extern uint Lzcnt32(uint rsp);

	//[DllImport("Mosa.Compiler.x64.Intrinsic::Lzcnt64")]
	//public static extern uint Lzcnt64(uint rsp);

	//[DllImport("Mosa.Compiler.x64.Intrinsic::Tzcnt32")]
	//public static extern uint Tzcnt32(uint rsp);

	//[DllImport("Mosa.Compiler.x64.Intrinsic::Tzcnt64")]
	//public static extern uint Tzcnt64(uint rsp);

	#endregion Intrinsic Instructions

	#region Intrinsic Methods

	[DllImport("Mosa.Compiler.x64.Intrinsic::BochsDebug")]
	public static extern void BochsDebug();

	[DllImport("Mosa.Compiler.x64.Intrinsic::SetCR0")]
	public static extern void SetCR0(ulong status);

	[DllImport("Mosa.Compiler.x64.Intrinsic::SetCR2")]
	public static extern void SetCR2(ulong status);

	[DllImport("Mosa.Compiler.x64.Intrinsic::SetCR3")]
	public static extern void SetCR3(ulong status);

	[DllImport("Mosa.Compiler.x64.Intrinsic::SetCR4")]
	public static extern void SetCR4(ulong status);

	[DllImport("Mosa.Compiler.x64.Intrinsic::GetCR0")]
	public static extern ulong GetCR0();

	[DllImport("Mosa.Compiler.x64.Intrinsic::GetCR2")]
	public static extern ulong GetCR2();

	[DllImport("Mosa.Compiler.x64.Intrinsic::GetCR3")]
	public static extern ulong GetCR3();

	[DllImport("Mosa.Compiler.x64.Intrinsic::GetCR4")]
	public static extern ulong GetCR4();

	[DllImport("Mosa.Compiler.x64.Intrinsic::Get8")]
	public static extern byte Get8(ulong address);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Get16")]
	public static extern ushort Get16(ulong address);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Get32")]
	public static extern uint Get32(ulong address);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Set8")]
	public static extern void Set8(ulong address, byte value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Set16")]
	public static extern void Set16(ulong address, ushort value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Set32")]
	public static extern void Set32(ulong address, uint value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::GetFS")]
	public static extern ulong GetFS();

	[DllImport("Mosa.Compiler.x64.Intrinsic::GetCS")]
	public static extern ulong GetCS();

	[DllImport("Mosa.Compiler.x64.Intrinsic::SetFS")]
	public static extern void SetFS(ulong value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Div")]
	public static extern uint Div(ulong n, uint d);

	[DllImport("Mosa.Compiler.x64.Intrinsic::CmpXChgLoad64")]
	public static extern long CmpXChgLoad64(ref long location, long value, long comparand);

	[DllImport("Mosa.Compiler.x64.Intrinsic::CmpXChgLoad64")]
	public static extern long CmpXChgLoad64(long location, long value, long comparand);

	[DllImport("Mosa.Compiler.x64.Intrinsic::XAddLoad32")]
	public static extern long XAddLoad32(ref int location, int value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::XAddLoad64")]
	public static extern long XAddLoad64(ref long location, long value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::XAddLoad64")]
	public static extern long XAddLoad64(long location, long value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::XChgLoad32")]
	public static extern long XChgLoad32(ref int location, int value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::XChgLoad64")]
	public static extern long XChgLoad64(ref long location, long value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::XChgLoad64")]
	public static extern long XChgLoad64(long location, long value);

	[DllImport("Mosa.Compiler.x64.Intrinsic::GetIDTJumpLocation")]
	public static extern long GetIDTJumpLocation(uint irq);

	[DllImport("Mosa.Compiler.x64.Intrinsic::FrameJump")]
	public static extern void FrameJump(Pointer rip, Pointer rsp, Pointer rbp, int exceptionRegister);

	[DllImport("Mosa.Compiler.x64.Intrinsic::FrameCall")]
	public static extern void FrameCall(ulong rip);

	[DllImport("Mosa.Compiler.x64.Intrinsic::FrameCallRetU8")]
	public static extern ulong FrameCallRetU8(ulong rip);

	[DllImport("Mosa.Compiler.x64.Intrinsic::FrameCallRetR8")]
	public static extern ulong FrameCallRetR8(ulong rip);

	[DllImport("Mosa.Compiler.x64.Intrinsic::InterruptReturn")]
	public static extern void InterruptReturn(ulong esp);

	[DllImport("Mosa.Compiler.x64.Intrinsic::AllocateStackSpace")]
	public static extern uint AllocateStackSpace(ulong size);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Remainder")]
	public static extern float Remainder(float dividend, float divisor);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Remainder")]
	public static extern double Remainder(double dividend, double divisor);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Memcpy256")]
	public static extern void Memcpy256(void* destination, void* source);

	[DllImport("Mosa.Compiler.x64.Intrinsic::Memclr256")]
	public static extern void Memclr256(void* destination);

	#endregion Intrinsic Methods
}
