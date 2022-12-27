// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mosa.Runtime.x86
{
	/// <summary>
	/// Provides stub methods for selected x86 native assembly instructions.
	/// </summary>
	public static unsafe class Native
	{
		#region Intrinsic Instructions

		[DllImport("Mosa.Platform.x86.Intrinsic::Lidt")]
		public static extern void Lidt(uint address);

		[DllImport("Mosa.Platform.x86.Intrinsic::Cli")]
		public static extern void Cli();

		[DllImport("Mosa.Platform.x86.Intrinsic::Lgdt")]
		public static extern void Lgdt(uint address);

		[DllImport("Mosa.Platform.x86.Intrinsic::Sti")]
		public static extern void Sti();

		[DllImport("Mosa.Platform.x86.Intrinsic::In8")]
		public static extern byte In8(ushort address);

		[DllImport("Mosa.Platform.x86.Intrinsic::In16")]
		public static extern ushort In16(ushort address);

		[DllImport("Mosa.Platform.x86.Intrinsic::In32")]
		public static extern uint In32(ushort address);

		[DllImport("Mosa.Platform.x86.Intrinsic::Out8")]
		public static extern void Out8(ushort address, byte value);

		[DllImport("Mosa.Platform.x86.Intrinsic::Out16")]
		public static extern void Out16(ushort address, ushort value);

		[DllImport("Mosa.Platform.x86.Intrinsic::Out32")]
		public static extern void Out32(ushort address, uint value);

		[DllImport("Mosa.Platform.x86.Intrinsic::Nop")]
		public static extern void Nop();

		[DllImport("Mosa.Platform.x86.Intrinsic::Hlt")]
		public static extern void Hlt();

		[DllImport("Mosa.Platform.x86.Intrinsic::Invlpg")]
		public static extern void Invlpg(uint address);

		[DllImport("Mosa.Platform.x86.Intrinsic::CpuIdEAX")]
		public static extern uint CpuIdEAX(uint eax, uint ecx);

		[DllImport("Mosa.Platform.x86.Intrinsic::CpuIdEBX")]
		public static extern uint CpuIdEBX(uint eax, uint ecx);

		[DllImport("Mosa.Platform.x86.Intrinsic::CpuIdECX")]
		public static extern uint CpuIdECX(uint eax, uint ecx);

		[DllImport("Mosa.Platform.x86.Intrinsic::CpuIdEDX")]
		public static extern uint CpuIdEDX(uint eax, uint ecx);

		[DllImport("Mosa.Platform.x86.Intrinsic::WrMSR")]
		public static extern void WrMSR(uint mrs, ulong value);

		[DllImport("Mosa.Platform.x86.Intrinsic::RdMSR")]
		public static extern ulong RdMSR(uint mrs);

		[DllImport("Mosa.Platform.x86.Intrinsic::Pause")]
		public static extern void Pause();

		[DllImport("Mosa.Platform.x86.Intrinsic::Int")]
		public static extern void Int(byte interrupt);

		[DllImport("Mosa.Platform.x86.Intrinsic::Sqrtsd")]
		public static extern double Sqrtsd(double destination);

		[DllImport("Mosa.Platform.x86.Intrinsic::Sqrtss")]
		public static extern float Sqrtss(float destination);

		[DllImport("Mosa.Platform.x86.Intrinsic::Roundsd2Negative")]
		public static extern double Roundsd2Negative(double destination);

		[DllImport("Mosa.Platform.x86.Intrinsic::Roundsd2Positive")]
		public static extern double Roundsd2Positive(double destination);

		[DllImport("Mosa.Platform.x86.Intrinsic::Roundss2Negative")]
		public static extern float Roundss2Negative(float destination);

		[DllImport("Mosa.Platform.x86.Intrinsic::Roundss2Positive")]
		public static extern float Roundss2Positive(float destination);

		[DllImport("Mosa.Platform.x86.Intrinsic::Blsr32")]
		public static extern uint Blsr32(uint esp);

		[DllImport("Mosa.Platform.x86.Intrinsic::Popcnt32")]
		public static extern uint Popcnt32(uint esp);

		[DllImport("Mosa.Platform.x86.Intrinsic::Lzcnt32")]
		public static extern uint Lzcnt32(uint esp);

		[DllImport("Mosa.Platform.x86.Intrinsic::Tzcnt32")]
		public static extern uint Tzcnt32(uint esp);

		#endregion Intrinsic Instructions

		#region Intrinsic Methods

		[DllImport("Mosa.Platform.x86.Intrinsic::SetSegments")]
		public static extern void SetSegments(ushort ds, ushort es, ushort fs, ushort gs, ushort ss);

		[DllImport("Mosa.Platform.x86.Intrinsic::FarJump")]
		public static extern void FarJump();

		[DllImport("Mosa.Platform.x86.Intrinsic::BochsDebug")]
		public static extern void BochsDebug();

		[DllImport("Mosa.Platform.x86.Intrinsic::SetCR0")]
		public static extern void SetCR0(uint status);

		[DllImport("Mosa.Platform.x86.Intrinsic::SetCR2")]
		public static extern void SetCR2(uint status);

		[DllImport("Mosa.Platform.x86.Intrinsic::SetCR3")]
		public static extern void SetCR3(uint status);

		[DllImport("Mosa.Platform.x86.Intrinsic::SetCR4")]
		public static extern void SetCR4(uint status);

		[DllImport("Mosa.Platform.x86.Intrinsic::GetCR0")]
		public static extern uint GetCR0();

		[DllImport("Mosa.Platform.x86.Intrinsic::GetCR2")]
		public static extern uint GetCR2();

		[DllImport("Mosa.Platform.x86.Intrinsic::GetCR3")]
		public static extern uint GetCR3();

		[DllImport("Mosa.Platform.x86.Intrinsic::GetCR4")]
		public static extern uint GetCR4();

		[DllImport("Mosa.Platform.x86.Intrinsic::Get8")]
		public static extern byte Get8(uint address);

		[DllImport("Mosa.Platform.x86.Intrinsic::Get16")]
		public static extern ushort Get16(uint address);

		[DllImport("Mosa.Platform.x86.Intrinsic::Get32")]
		public static extern uint Get32(uint address);

		[DllImport("Mosa.Platform.x86.Intrinsic::Set8")]
		public static extern void Set8(uint address, byte value);

		[DllImport("Mosa.Platform.x86.Intrinsic::Set16")]
		public static extern void Set16(uint address, ushort value);

		[DllImport("Mosa.Platform.x86.Intrinsic::Set32")]
		public static extern void Set32(uint address, uint value);

		[DllImport("Mosa.Platform.x86.Intrinsic::GetFS")]
		public static extern uint GetFS();

		[DllImport("Mosa.Platform.x86.Intrinsic::GetCS")]
		public static extern uint GetCS();

		[DllImport("Mosa.Platform.x86.Intrinsic::SetFS")]
		public static extern void SetFS(uint value);

		[DllImport("Mosa.Platform.x86.Intrinsic::Div")]
		public static extern uint Div(ulong n, uint d);

		[DllImport("Mosa.Platform.x86.Intrinsic::CmpXChgLoad32")]
		public static extern int CmpXChgLoad32(ref int location, int value, int comparand);

		[DllImport("Mosa.Platform.x86.Intrinsic::CmpXChgLoad32")]
		public static extern int CmpXChgLoad32(int location, int value, int comparand);

		[DllImport("Mosa.Platform.x86.Intrinsic::XAddLoad32")]
		public static extern int XAddLoad32(ref int location, int value);

		[DllImport("Mosa.Platform.x86.Intrinsic::XAddLoad32")]
		public static extern int XAddLoad32(int location, int value);

		[DllImport("Mosa.Platform.x86.Intrinsic::XChgLoad32")]
		public static extern int XChgLoad32(ref int location, int value);

		[DllImport("Mosa.Platform.x86.Intrinsic::XChgLoad32")]
		public static extern int XChgLoad32(int location, int value);

		[DllImport("Mosa.Platform.x86.Intrinsic::GetIDTJumpLocation")]
		public static extern uint GetIDTJumpLocation(uint irq);

		[DllImport("Mosa.Platform.x86.Intrinsic::GetMultibootEAX")]
		public static extern uint GetMultibootEAX();

		[DllImport("Mosa.Platform.x86.Intrinsic::GetMultibootEBX")]
		public static extern uint GetMultibootEBX();

		[DllImport("Mosa.Platform.x86.Intrinsic::FrameJump")]
		public static extern void FrameJump(Pointer eip, Pointer esp, Pointer ebp, int exceptionRegister);

		[DllImport("Mosa.Platform.x86.Intrinsic::FrameCall")]
		public static extern void FrameCall(uint eip);

		[DllImport("Mosa.Platform.x86.Intrinsic::FrameCallRetU4")]
		public static extern uint FrameCallRetU4(uint eip);

		[DllImport("Mosa.Platform.x86.Intrinsic::FrameCallRetU8")]
		public static extern ulong FrameCallRetU8(uint eip);

		[DllImport("Mosa.Platform.x86.Intrinsic::FrameCallRetR8")]
		public static extern ulong FrameCallRetR8(uint eip);

		[DllImport("Mosa.Platform.x86.Intrinsic::InterruptReturn")]
		public static extern void InterruptReturn(uint esp);

		[DllImport("Mosa.Platform.x86.Intrinsic::AllocateStackSpace")]
		public static extern uint AllocateStackSpace(uint size);

		[DllImport("Mosa.Platform.x86.Intrinsic::Remainder")]
		public static extern float Remainder(float dividend, float divisor);

		[DllImport("Mosa.Platform.x86.Intrinsic::Remainder")]
		public static extern double Remainder(double dividend, double divisor);

		[DllImport("Mosa.Platform.x86.Intrinsic::Memcpy256")]
		public static extern void Memcpy256(void* destination, void* source);

		[DllImport("Mosa.Platform.x86.Intrinsic::Memclr256")]
		public static extern void Memclr256(void* destination);

		#endregion Intrinsic Methods

		#region IRQs Intrinsic

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ0")]
		public static extern void IRQ0();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ1")]
		public static extern void IRQ1();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ2")]
		public static extern void IRQ2();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ3")]
		public static extern void IRQ3();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ4")]
		public static extern void IRQ4();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ5")]
		public static extern void IRQ5();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ6")]
		public static extern void IRQ6();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ7")]
		public static extern void IRQ7();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ8")]
		public static extern void IRQ8();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ9")]
		public static extern void IRQ9();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ10")]
		public static extern void IRQ10();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ11")]
		public static extern void IRQ11();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ12")]
		public static extern void IRQ12();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ13")]
		public static extern void IRQ13();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ14")]
		public static extern void IRQ14();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ15")]
		public static extern void IRQ15();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ16")]
		public static extern void IRQ16();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ17")]
		public static extern void IRQ17();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ18")]
		public static extern void IRQ18();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ19")]
		public static extern void IRQ19();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ20")]
		public static extern void IRQ20();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ21")]
		public static extern void IRQ21();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ22")]
		public static extern void IRQ22();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ23")]
		public static extern void IRQ23();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ24")]
		public static extern void IRQ24();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ25")]
		public static extern void IRQ25();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ26")]
		public static extern void IRQ26();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ27")]
		public static extern void IRQ27();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ28")]
		public static extern void IRQ28();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ29")]
		public static extern void IRQ29();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ30")]
		public static extern void IRQ30();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ31")]
		public static extern void IRQ31();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ32")]
		public static extern void IRQ32();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ33")]
		public static extern void IRQ33();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ34")]
		public static extern void IRQ34();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ35")]
		public static extern void IRQ35();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ36")]
		public static extern void IRQ36();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ37")]
		public static extern void IRQ37();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ38")]
		public static extern void IRQ38();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ39")]
		public static extern void IRQ39();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ40")]
		public static extern void IRQ40();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ41")]
		public static extern void IRQ41();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ42")]
		public static extern void IRQ42();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ43")]
		public static extern void IRQ43();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ44")]
		public static extern void IRQ44();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ45")]
		public static extern void IRQ45();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ46")]
		public static extern void IRQ46();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ47")]
		public static extern void IRQ47();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ48")]
		public static extern void IRQ48();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ49")]
		public static extern void IRQ49();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ50")]
		public static extern void IRQ50();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ51")]
		public static extern void IRQ51();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ52")]
		public static extern void IRQ52();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ53")]
		public static extern void IRQ53();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ54")]
		public static extern void IRQ54();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ55")]
		public static extern void IRQ55();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ56")]
		public static extern void IRQ56();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ57")]
		public static extern void IRQ57();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ58")]
		public static extern void IRQ58();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ59")]
		public static extern void IRQ59();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ60")]
		public static extern void IRQ60();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ61")]
		public static extern void IRQ61();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ62")]
		public static extern void IRQ62();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ63")]
		public static extern void IRQ63();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ64")]
		public static extern void IRQ64();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ65")]
		public static extern void IRQ65();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ66")]
		public static extern void IRQ66();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ67")]
		public static extern void IRQ67();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ68")]
		public static extern void IRQ68();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ69")]
		public static extern void IRQ69();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ70")]
		public static extern void IRQ70();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ71")]
		public static extern void IRQ71();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ72")]
		public static extern void IRQ72();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ73")]
		public static extern void IRQ73();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ74")]
		public static extern void IRQ74();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ75")]
		public static extern void IRQ75();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ76")]
		public static extern void IRQ76();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ77")]
		public static extern void IRQ77();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ78")]
		public static extern void IRQ78();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ79")]
		public static extern void IRQ79();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ80")]
		public static extern void IRQ80();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ81")]
		public static extern void IRQ81();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ82")]
		public static extern void IRQ82();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ83")]
		public static extern void IRQ83();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ84")]
		public static extern void IRQ84();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ85")]
		public static extern void IRQ85();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ86")]
		public static extern void IRQ86();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ87")]
		public static extern void IRQ87();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ88")]
		public static extern void IRQ88();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ89")]
		public static extern void IRQ89();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ90")]
		public static extern void IRQ90();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ91")]
		public static extern void IRQ91();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ92")]
		public static extern void IRQ92();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ93")]
		public static extern void IRQ93();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ94")]
		public static extern void IRQ94();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ95")]
		public static extern void IRQ95();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ96")]
		public static extern void IRQ96();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ97")]
		public static extern void IRQ97();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ98")]
		public static extern void IRQ98();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ99")]
		public static extern void IRQ99();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ100")]
		public static extern void IRQ100();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ101")]
		public static extern void IRQ101();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ102")]
		public static extern void IRQ102();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ103")]
		public static extern void IRQ103();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ14")]
		public static extern void IRQ104();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ105")]
		public static extern void IRQ105();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ106")]
		public static extern void IRQ106();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ107")]
		public static extern void IRQ107();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ108")]
		public static extern void IRQ108();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ109")]
		public static extern void IRQ109();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ110")]
		public static extern void IRQ110();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ111")]
		public static extern void IRQ111();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ112")]
		public static extern void IRQ112();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ113")]
		public static extern void IRQ113();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ114")]
		public static extern void IRQ114();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ115")]
		public static extern void IRQ115();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ116")]
		public static extern void IRQ116();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ117")]
		public static extern void IRQ117();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ118")]
		public static extern void IRQ118();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ119")]
		public static extern void IRQ119();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ120")]
		public static extern void IRQ120();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ121")]
		public static extern void IRQ121();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ122")]
		public static extern void IRQ122();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ123")]
		public static extern void IRQ123();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ124")]
		public static extern void IRQ124();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ125")]
		public static extern void IRQ125();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ126")]
		public static extern void IRQ126();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ127")]
		public static extern void IRQ127();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ128")]
		public static extern void IRQ128();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ129")]
		public static extern void IRQ129();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ130")]
		public static extern void IRQ130();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ131")]
		public static extern void IRQ131();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ132")]
		public static extern void IRQ132();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ133")]
		public static extern void IRQ133();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ134")]
		public static extern void IRQ134();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ135")]
		public static extern void IRQ135();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ136")]
		public static extern void IRQ136();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ137")]
		public static extern void IRQ137();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ138")]
		public static extern void IRQ138();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ139")]
		public static extern void IRQ139();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ140")]
		public static extern void IRQ140();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ141")]
		public static extern void IRQ141();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ142")]
		public static extern void IRQ142();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ143")]
		public static extern void IRQ143();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ144")]
		public static extern void IRQ144();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ145")]
		public static extern void IRQ145();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ146")]
		public static extern void IRQ146();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ147")]
		public static extern void IRQ147();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ148")]
		public static extern void IRQ148();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ149")]
		public static extern void IRQ149();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ150")]
		public static extern void IRQ150();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ151")]
		public static extern void IRQ151();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ152")]
		public static extern void IRQ152();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ153")]
		public static extern void IRQ153();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ154")]
		public static extern void IRQ154();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ155")]
		public static extern void IRQ155();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ156")]
		public static extern void IRQ156();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ157")]
		public static extern void IRQ157();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ158")]
		public static extern void IRQ158();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ159")]
		public static extern void IRQ159();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ160")]
		public static extern void IRQ160();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ161")]
		public static extern void IRQ161();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ162")]
		public static extern void IRQ162();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ163")]
		public static extern void IRQ163();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ164")]
		public static extern void IRQ164();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ165")]
		public static extern void IRQ165();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ166")]
		public static extern void IRQ166();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ167")]
		public static extern void IRQ167();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ168")]
		public static extern void IRQ168();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ169")]
		public static extern void IRQ169();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ170")]
		public static extern void IRQ170();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ171")]
		public static extern void IRQ171();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ172")]
		public static extern void IRQ172();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ173")]
		public static extern void IRQ173();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ174")]
		public static extern void IRQ174();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ175")]
		public static extern void IRQ175();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ176")]
		public static extern void IRQ176();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ177")]
		public static extern void IRQ177();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ178")]
		public static extern void IRQ178();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ179")]
		public static extern void IRQ179();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ180")]
		public static extern void IRQ180();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ181")]
		public static extern void IRQ181();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ182")]
		public static extern void IRQ182();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ183")]
		public static extern void IRQ183();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ184")]
		public static extern void IRQ184();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ185")]
		public static extern void IRQ185();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ186")]
		public static extern void IRQ186();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ187")]
		public static extern void IRQ187();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ188")]
		public static extern void IRQ188();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ189")]
		public static extern void IRQ189();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ190")]
		public static extern void IRQ190();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ191")]
		public static extern void IRQ191();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ192")]
		public static extern void IRQ192();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ193")]
		public static extern void IRQ193();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ194")]
		public static extern void IRQ194();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ195")]
		public static extern void IRQ195();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ196")]
		public static extern void IRQ196();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ197")]
		public static extern void IRQ197();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ198")]
		public static extern void IRQ198();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ199")]
		public static extern void IRQ199();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ200")]
		public static extern void IRQ200();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ201")]
		public static extern void IRQ201();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ202")]
		public static extern void IRQ202();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ203")]
		public static extern void IRQ203();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ204")]
		public static extern void IRQ204();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ205")]
		public static extern void IRQ205();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ206")]
		public static extern void IRQ206();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ207")]
		public static extern void IRQ207();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ208")]
		public static extern void IRQ208();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ209")]
		public static extern void IRQ209();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ210")]
		public static extern void IRQ210();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ211")]
		public static extern void IRQ211();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ212")]
		public static extern void IRQ212();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ213")]
		public static extern void IRQ213();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ214")]
		public static extern void IRQ214();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ215")]
		public static extern void IRQ215();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ216")]
		public static extern void IRQ216();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ217")]
		public static extern void IRQ217();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ218")]
		public static extern void IRQ218();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ219")]
		public static extern void IRQ219();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ220")]
		public static extern void IRQ220();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ221")]
		public static extern void IRQ221();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ222")]
		public static extern void IRQ222();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ223")]
		public static extern void IRQ223();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ224")]
		public static extern void IRQ224();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ225")]
		public static extern void IRQ225();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ226")]
		public static extern void IRQ226();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ227")]
		public static extern void IRQ227();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ228")]
		public static extern void IRQ228();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ229")]
		public static extern void IRQ229();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ230")]
		public static extern void IRQ230();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ231")]
		public static extern void IRQ231();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ232")]
		public static extern void IRQ232();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ233")]
		public static extern void IRQ233();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ234")]
		public static extern void IRQ234();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ235")]
		public static extern void IRQ235();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ236")]
		public static extern void IRQ236();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ237")]
		public static extern void IRQ237();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ238")]
		public static extern void IRQ238();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ239")]
		public static extern void IRQ239();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ240")]
		public static extern void IRQ240();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ241")]
		public static extern void IRQ241();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ242")]
		public static extern void IRQ242();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ243")]
		public static extern void IRQ243();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ244")]
		public static extern void IRQ244();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ245")]
		public static extern void IRQ245();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ246")]
		public static extern void IRQ246();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ247")]
		public static extern void IRQ247();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ248")]
		public static extern void IRQ248();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ249")]
		public static extern void IRQ249();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ250")]
		public static extern void IRQ250();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ251")]
		public static extern void IRQ251();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ252")]
		public static extern void IRQ252();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ253")]
		public static extern void IRQ253();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ254")]
		public static extern void IRQ254();

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DllImport("Mosa.Platform.x86.Intrinsic::IRQ255")]
		public static extern void IRQ255();

		#endregion IRQs Intrinsic
	}
}
