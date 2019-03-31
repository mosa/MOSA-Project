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

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Lidt")]
		public extern static void Lidt(uint address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Cli")]
		public extern static void Cli();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Lgdt")]
		public extern static void Lgdt(uint address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Jmp")]
		public extern static void Jmp(uint address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Call")]
		public extern static void Call(uint address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Sti")]
		public extern static void Sti();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:In8")]
		public extern static byte In8(ushort address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:In16")]
		public extern static ushort In16(ushort address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:In32")]
		public extern static uint In32(ushort address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Out8")]
		public extern static void Out8(ushort address, byte value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Out16")]
		public extern static void Out16(ushort address, ushort value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Out32")]
		public extern static void Out32(ushort address, uint value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Nop")]
		public extern static void Nop();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Hlt")]
		public extern static void Hlt();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Invlpg")]
		public extern static void Invlpg(uint address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:CpuIdEax")]
		public extern static uint CpuIdEax(uint function);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:CpuIdEbx")]
		public extern static uint CpuIdEbx(uint function);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:CpuIdEcx")]
		public extern static uint CpuIdEcx(uint function);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:CpuIdEdx")]
		public extern static uint CpuIdEdx(uint function);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:BochsDebug")]
		public extern static void BochsDebug();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:SetCR0")]
		public extern static void SetCR0(uint status);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:SetCR2")]
		public extern static void SetCR2(uint status);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:SetCR3")]
		public extern static void SetCR3(uint status);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:SetCR4")]
		public extern static void SetCR4(uint status);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetCR0")]
		public extern static uint GetCR0();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetCR2")]
		public extern static uint GetCR2();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetCR3")]
		public extern static uint GetCR3();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetCR4")]
		public extern static uint GetCR4();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Get8")]
		public extern static byte Get8(uint address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Get16")]
		public extern static ushort Get16(uint address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Get32")]
		public extern static uint Get32(uint address);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Set8")]
		public extern static void Set8(uint address, byte value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Set16")]
		public extern static void Set16(uint address, ushort value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Set32")]
		public extern static void Set32(uint address, uint value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetEBP")]
		public extern static IntPtr GetEBP();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetFS")]
		public extern static uint GetFS();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetCS")]
		public extern static uint GetCS();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:SetFS")]
		public extern static void SetFS(uint value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Div")]
		public extern static uint Div(ulong n, uint d);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:CmpXChgLoad32")]
		public extern static int CmpXChgLoad32(ref int location, int value, int comparand);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:CmpXChgLoad32")]
		public extern static int CmpXChgLoad32(int location, int value, int comparand);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:XAddLoad32")]
		public extern static int XAddLoad32(ref int location, int value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:XAddLoad32")]
		public extern static int XAddLoad32(int location, int value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:XChgLoad32")]
		public extern static int XChgLoad32(ref int location, int value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:XChgLoad32")]
		public extern static int XChgLoad32(int location, int value);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Pause")]
		public extern static void Pause();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetExceptionRegister")]
		public extern static uint GetExceptionRegister();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetIDTJumpLocation")]
		public extern static uint GetIDTJumpLocation(uint irq);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetMultibootEAX")]
		public extern static uint GetMultibootEAX();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:GetMultibootEBX")]
		public extern static uint GetMultibootEBX();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:FrameJump")]
		public extern static void FrameJump(IntPtr eip, IntPtr esp, IntPtr ebp, int exceptionRegister);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:FrameCall")]
		public extern static void FrameCall(uint eip);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:FrameCallRetU4")]
		public extern static uint FrameCallRetU4(uint eip);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:FrameCallRetU8")]
		public extern static ulong FrameCallRetU8(uint eip);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:FrameCallRetR8")]
		public extern static ulong FrameCallRetR8(uint eip);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:InterruptReturn")]
		public extern static void InterruptReturn(uint esp);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:AllocateStackSpace")]
		public extern static uint AllocateStackSpace(uint size);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Int")]
		public extern static void Int(byte interrupt);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Remainder")]
		public extern static float Remainder(float dividend, float divisor);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Remainder")]
		public extern static double Remainder(double dividend, double divisor);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Memcpy256")]
		public extern static void Memcpy256(void* destination, void* source);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Memclr256")]
		public extern static void Memclr256(void* destination);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Sqrtsd")]
		public extern static double Sqrtsd(double destination);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Sqrtss")]
		public extern static float Sqrtss(float destination);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Roundsd2Negative")]
		public extern static double Roundsd2Negative(double destination);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Roundsd2Positive")]
		public extern static double Roundsd2Positive(double destination);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Roundss2Negative")]
		public extern static float Roundss2Negative(float destination);

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:Roundss2Positive")]
		public extern static float Roundss2Positive(float destination);

		#endregion Intrinsic

		#region IRQs Intrinsic

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ0")]
		public extern static void IRQ0();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ1")]
		public extern static void IRQ1();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ2")]
		public extern static void IRQ2();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ3")]
		public extern static void IRQ3();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ4")]
		public extern static void IRQ4();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ5")]
		public extern static void IRQ5();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ6")]
		public extern static void IRQ6();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ7")]
		public extern static void IRQ7();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ8")]
		public extern static void IRQ8();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ9")]
		public extern static void IRQ9();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ10")]
		public extern static void IRQ10();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ11")]
		public extern static void IRQ11();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ12")]
		public extern static void IRQ12();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ13")]
		public extern static void IRQ13();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ14")]
		public extern static void IRQ14();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ15")]
		public extern static void IRQ15();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ16")]
		public extern static void IRQ16();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ17")]
		public extern static void IRQ17();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ18")]
		public extern static void IRQ18();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ19")]
		public extern static void IRQ19();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ20")]
		public extern static void IRQ20();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ21")]
		public extern static void IRQ21();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ22")]
		public extern static void IRQ22();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ23")]
		public extern static void IRQ23();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ24")]
		public extern static void IRQ24();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ25")]
		public extern static void IRQ25();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ26")]
		public extern static void IRQ26();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ27")]
		public extern static void IRQ27();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ28")]
		public extern static void IRQ28();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ29")]
		public extern static void IRQ29();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ30")]
		public extern static void IRQ30();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ31")]
		public extern static void IRQ31();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ32")]
		public extern static void IRQ32();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ33")]
		public extern static void IRQ33();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ34")]
		public extern static void IRQ34();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ35")]
		public extern static void IRQ35();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ36")]
		public extern static void IRQ36();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ37")]
		public extern static void IRQ37();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ38")]
		public extern static void IRQ38();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ39")]
		public extern static void IRQ39();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ40")]
		public extern static void IRQ40();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ41")]
		public extern static void IRQ41();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ42")]
		public extern static void IRQ42();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ43")]
		public extern static void IRQ43();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ44")]
		public extern static void IRQ44();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ45")]
		public extern static void IRQ45();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ46")]
		public extern static void IRQ46();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ47")]
		public extern static void IRQ47();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ48")]
		public extern static void IRQ48();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ49")]
		public extern static void IRQ49();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ50")]
		public extern static void IRQ50();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ51")]
		public extern static void IRQ51();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ52")]
		public extern static void IRQ52();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ53")]
		public extern static void IRQ53();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ54")]
		public extern static void IRQ54();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ55")]
		public extern static void IRQ55();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ56")]
		public extern static void IRQ56();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ57")]
		public extern static void IRQ57();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ58")]
		public extern static void IRQ58();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ59")]
		public extern static void IRQ59();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ60")]
		public extern static void IRQ60();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ61")]
		public extern static void IRQ61();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ62")]
		public extern static void IRQ62();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ63")]
		public extern static void IRQ63();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ64")]
		public extern static void IRQ64();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ65")]
		public extern static void IRQ65();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ66")]
		public extern static void IRQ66();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ67")]
		public extern static void IRQ67();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ68")]
		public extern static void IRQ68();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ69")]
		public extern static void IRQ69();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ70")]
		public extern static void IRQ70();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ71")]
		public extern static void IRQ71();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ72")]
		public extern static void IRQ72();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ73")]
		public extern static void IRQ73();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ74")]
		public extern static void IRQ74();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ75")]
		public extern static void IRQ75();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ76")]
		public extern static void IRQ76();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ77")]
		public extern static void IRQ77();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ78")]
		public extern static void IRQ78();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ79")]
		public extern static void IRQ79();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ80")]
		public extern static void IRQ80();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ81")]
		public extern static void IRQ81();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ82")]
		public extern static void IRQ82();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ83")]
		public extern static void IRQ83();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ84")]
		public extern static void IRQ84();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ85")]
		public extern static void IRQ85();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ86")]
		public extern static void IRQ86();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ87")]
		public extern static void IRQ87();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ88")]
		public extern static void IRQ88();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ89")]
		public extern static void IRQ89();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ90")]
		public extern static void IRQ90();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ91")]
		public extern static void IRQ91();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ92")]
		public extern static void IRQ92();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ93")]
		public extern static void IRQ93();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ94")]
		public extern static void IRQ94();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ95")]
		public extern static void IRQ95();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ96")]
		public extern static void IRQ96();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ97")]
		public extern static void IRQ97();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ98")]
		public extern static void IRQ98();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ99")]
		public extern static void IRQ99();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ100")]
		public extern static void IRQ100();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ101")]
		public extern static void IRQ101();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ102")]
		public extern static void IRQ102();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ103")]
		public extern static void IRQ103();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ14")]
		public extern static void IRQ104();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ105")]
		public extern static void IRQ105();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ106")]
		public extern static void IRQ106();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ107")]
		public extern static void IRQ107();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ108")]
		public extern static void IRQ108();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ109")]
		public extern static void IRQ109();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ110")]
		public extern static void IRQ110();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ111")]
		public extern static void IRQ111();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ112")]
		public extern static void IRQ112();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ113")]
		public extern static void IRQ113();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ114")]
		public extern static void IRQ114();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ115")]
		public extern static void IRQ115();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ116")]
		public extern static void IRQ116();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ117")]
		public extern static void IRQ117();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ118")]
		public extern static void IRQ118();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ119")]
		public extern static void IRQ119();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ120")]
		public extern static void IRQ120();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ121")]
		public extern static void IRQ121();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ122")]
		public extern static void IRQ122();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ123")]
		public extern static void IRQ123();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ124")]
		public extern static void IRQ124();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ125")]
		public extern static void IRQ125();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ126")]
		public extern static void IRQ126();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ127")]
		public extern static void IRQ127();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ128")]
		public extern static void IRQ128();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ129")]
		public extern static void IRQ129();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ130")]
		public extern static void IRQ130();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ131")]
		public extern static void IRQ131();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ132")]
		public extern static void IRQ132();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ133")]
		public extern static void IRQ133();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ134")]
		public extern static void IRQ134();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ135")]
		public extern static void IRQ135();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ136")]
		public extern static void IRQ136();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ137")]
		public extern static void IRQ137();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ138")]
		public extern static void IRQ138();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ139")]
		public extern static void IRQ139();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ140")]
		public extern static void IRQ140();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ141")]
		public extern static void IRQ141();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ142")]
		public extern static void IRQ142();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ143")]
		public extern static void IRQ143();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ144")]
		public extern static void IRQ144();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ145")]
		public extern static void IRQ145();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ146")]
		public extern static void IRQ146();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ147")]
		public extern static void IRQ147();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ148")]
		public extern static void IRQ148();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ149")]
		public extern static void IRQ149();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ150")]
		public extern static void IRQ150();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ151")]
		public extern static void IRQ151();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ152")]
		public extern static void IRQ152();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ153")]
		public extern static void IRQ153();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ154")]
		public extern static void IRQ154();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ155")]
		public extern static void IRQ155();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ156")]
		public extern static void IRQ156();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ157")]
		public extern static void IRQ157();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ158")]
		public extern static void IRQ158();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ159")]
		public extern static void IRQ159();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ160")]
		public extern static void IRQ160();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ161")]
		public extern static void IRQ161();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ162")]
		public extern static void IRQ162();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ163")]
		public extern static void IRQ163();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ164")]
		public extern static void IRQ164();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ165")]
		public extern static void IRQ165();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ166")]
		public extern static void IRQ166();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ167")]
		public extern static void IRQ167();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ168")]
		public extern static void IRQ168();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ169")]
		public extern static void IRQ169();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ170")]
		public extern static void IRQ170();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ171")]
		public extern static void IRQ171();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ172")]
		public extern static void IRQ172();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ173")]
		public extern static void IRQ173();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ174")]
		public extern static void IRQ174();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ175")]
		public extern static void IRQ175();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ176")]
		public extern static void IRQ176();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ177")]
		public extern static void IRQ177();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ178")]
		public extern static void IRQ178();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ179")]
		public extern static void IRQ179();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ180")]
		public extern static void IRQ180();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ181")]
		public extern static void IRQ181();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ182")]
		public extern static void IRQ182();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ183")]
		public extern static void IRQ183();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ184")]
		public extern static void IRQ184();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ185")]
		public extern static void IRQ185();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ186")]
		public extern static void IRQ186();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ187")]
		public extern static void IRQ187();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ188")]
		public extern static void IRQ188();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ189")]
		public extern static void IRQ189();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ190")]
		public extern static void IRQ190();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ191")]
		public extern static void IRQ191();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ192")]
		public extern static void IRQ192();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ193")]
		public extern static void IRQ193();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ194")]
		public extern static void IRQ194();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ195")]
		public extern static void IRQ195();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ196")]
		public extern static void IRQ196();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ197")]
		public extern static void IRQ197();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ198")]
		public extern static void IRQ198();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ199")]
		public extern static void IRQ199();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ200")]
		public extern static void IRQ200();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ201")]
		public extern static void IRQ201();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ202")]
		public extern static void IRQ202();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ203")]
		public extern static void IRQ203();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ204")]
		public extern static void IRQ204();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ205")]
		public extern static void IRQ205();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ206")]
		public extern static void IRQ206();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ207")]
		public extern static void IRQ207();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ208")]
		public extern static void IRQ208();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ209")]
		public extern static void IRQ209();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ210")]
		public extern static void IRQ210();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ211")]
		public extern static void IRQ211();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ212")]
		public extern static void IRQ212();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ213")]
		public extern static void IRQ213();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ214")]
		public extern static void IRQ214();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ215")]
		public extern static void IRQ215();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ216")]
		public extern static void IRQ216();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ217")]
		public extern static void IRQ217();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ218")]
		public extern static void IRQ218();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ219")]
		public extern static void IRQ219();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ220")]
		public extern static void IRQ220();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ221")]
		public extern static void IRQ221();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ222")]
		public extern static void IRQ222();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ223")]
		public extern static void IRQ223();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ224")]
		public extern static void IRQ224();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ225")]
		public extern static void IRQ225();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ226")]
		public extern static void IRQ226();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ227")]
		public extern static void IRQ227();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ228")]
		public extern static void IRQ228();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ229")]
		public extern static void IRQ229();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ230")]
		public extern static void IRQ230();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ231")]
		public extern static void IRQ231();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ232")]
		public extern static void IRQ232();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ233")]
		public extern static void IRQ233();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ234")]
		public extern static void IRQ234();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ235")]
		public extern static void IRQ235();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ236")]
		public extern static void IRQ236();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ237")]
		public extern static void IRQ237();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ238")]
		public extern static void IRQ238();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ239")]
		public extern static void IRQ239();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ240")]
		public extern static void IRQ240();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ241")]
		public extern static void IRQ241();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ242")]
		public extern static void IRQ242();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ243")]
		public extern static void IRQ243();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ244")]
		public extern static void IRQ244();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ245")]
		public extern static void IRQ245();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ246")]
		public extern static void IRQ246();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ247")]
		public extern static void IRQ247();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ248")]
		public extern static void IRQ248();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ249")]
		public extern static void IRQ249();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ250")]
		public extern static void IRQ250();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ251")]
		public extern static void IRQ251();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ252")]
		public extern static void IRQ252();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ253")]
		public extern static void IRQ253();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ254")]
		public extern static void IRQ254();

		[DllImportAttribute("Mosa.Platform.x86.Intrinsic:IRQ255")]
		public extern static void IRQ255();

		#endregion IRQs Intrinsic
	}
}
