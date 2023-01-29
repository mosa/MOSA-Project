// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.x86;

/// <summary>
/// Provides stub methods for selected x86 native assembly instructions.
/// </summary>
public static unsafe class Native
{
	#region Intrinsic

	public static void Lidt(uint address)
	{
	}

	public static void Cli()
	{
	}

	public static void Lgdt(uint address)
	{
	}

	public static void SetSegments(ushort ds, ushort es, ushort fs, ushort gs, ushort ss)
	{
	}

	public static void FarJump()
	{
	}

	public static void Jmp(uint address)
	{
	}

	public static void Call(uint address)
	{
	}

	public static void Sti()
	{
	}

	public static byte In8(ushort address)
	{
		return 0;
	}

	public static ushort In16(ushort address)
	{
		return 0;
	}

	public static uint In32(ushort address)
	{
		return 0;
	}

	public static void Out8(ushort address, byte value)
	{
	}

	public static void Out16(ushort address, ushort value)
	{
	}

	public static void Out32(ushort address, uint value)
	{
	}

	public static void Nop()
	{
	}

	public static void Hlt()
	{
	}

	public static void Invlpg(uint address)
	{
	}

	public static uint CpuIdEAX(uint function)
	{
		return 0;
	}

	public static uint CpuIdEBX(uint function)
	{
		return 0;
	}

	public static uint CpuIdECX(uint function)
	{
		return 0;
	}

	public static uint CpuIdEDX(uint function)
	{
		return 0;
	}

	public static void BochsDebug()
	{
	}

	public static void SetCR0(uint status)
	{
	}

	public static void SetCR2(uint status)
	{
	}

	public static void SetCR3(uint status)
	{
	}

	public static void SetCR4(uint status)
	{
	}

	public static uint GetCR0()
	{
		return 0;
	}

	public static uint GetCR2()
	{
		return 0;
	}

	public static uint GetCR3()
	{
		return 0;
	}

	public static uint GetCR4()
	{
		return 0;
	}

	public static byte Get8(uint address)
	{
		return 0; // TODO
	}

	public static ushort Get16(uint address)
	{
		return 0; // TODO
	}

	public static uint Get32(uint address)
	{
		return 0; // TODO
	}

	public static void Set8(uint address, byte value)
	{
	}

	public static void Set16(uint address, ushort value)
	{
	}

	public static void Set32(uint address, uint value)
	{
	}

	public static IntPtr GetEBP()
	{
		return IntPtr.Zero;
	}

	public static uint GetFS()
	{
		return 0;
	}

	public static uint GetCS()
	{
		return 0;
	}

	public static void SetFS(uint value)
	{
	}

	public static uint Div(ulong n, uint d)
	{
		return 0;
	}

	public static int CmpXChgLoad32(ref int location, int value, int comparand)
	{
		return 0;
	}

	public static int CmpXChgLoad32(int location, int value, int comparand)
	{
		return 0;
	}

	public static int XAddLoad32(ref int location, int value)
	{
		return 0;
	}

	public static int XAddLoad32(int location, int value)
	{
		return 0;
	}

	public static int XChgLoad32(ref int location, int value)
	{
		return 0;
	}

	public static int XChgLoad32(int location, int value)
	{
		return 0;
	}

	public static void Pause()
	{
	}

	public static uint GetIDTJumpLocation(uint irq)
	{
		return 0;
	}

	public static uint GetMultibootEAX()
	{
		return 0;
	}

	public static uint GetMultibootEBX()
	{
		return 0;
	}

	public static void FrameJump(IntPtr eip, IntPtr esp, IntPtr ebp, int exceptionRegister)
	{
	}

	public static void FrameCall(uint eip)
	{
	}

	public static uint FrameCallRetU4(uint eip)
	{
		return 0;
	}

	public static ulong FrameCallRetU8(uint eip)
	{
		return 0;
	}

	public static ulong FrameCallRetR8(uint eip)
	{
		return 0;
	}

	public static void InterruptReturn(uint esp)
	{
	}

	public static uint AllocateStackSpace(uint size)
	{
		return 0;
	}

	public static void Int(byte interrupt)
	{
	}

	public static float Remainder(float dividend, float divisor)
	{
		return 0;
	}

	public static double Remainder(double dividend, double divisor)
	{
		return 0;
	}

	public static void Memcpy256(void* destination, void* source)
	{
	}

	public static void Memclr256(void* destination)
	{
	}

	public static double Sqrtsd(double destination)
	{
		return 0;
	}

	public static float Sqrtss(float destination)
	{
		return 0;
	}

	public static double Roundsd2Negative(double destination)
	{
		return 0;
	}

	public static double Roundsd2Positive(double destination)
	{
		return 0;
	}

	public static float Roundss2Negative(float destination)
	{
		return 0;
	}

	public static float Roundss2Positive(float destination)
	{
		return 0;
	}

	#endregion Intrinsic

	#region IRQs Intrinsic

	public static void IRQ0()
	{
	}

	public static void IRQ1()
	{
	}

	public static void IRQ2()
	{
	}

	public static void IRQ3()
	{
	}

	public static void IRQ4()
	{
	}

	public static void IRQ5()
	{
	}

	public static void IRQ6()
	{
	}

	public static void IRQ7()
	{
	}

	public static void IRQ8()
	{
	}

	public static void IRQ9()
	{
	}

	public static void IRQ10()
	{
	}

	public static void IRQ11()
	{
	}

	public static void IRQ12()
	{
	}

	public static void IRQ13()
	{
	}

	public static void IRQ14()
	{
	}

	public static void IRQ15()
	{
	}

	public static void IRQ16()
	{
	}

	public static void IRQ17()
	{
	}

	public static void IRQ18()
	{
	}

	public static void IRQ19()
	{
	}

	public static void IRQ20()
	{
	}

	public static void IRQ21()
	{
	}

	public static void IRQ22()
	{
	}

	public static void IRQ23()
	{
	}

	public static void IRQ24()
	{
	}

	public static void IRQ25()
	{
	}

	public static void IRQ26()
	{
	}

	public static void IRQ27()
	{
	}

	public static void IRQ28()
	{
	}

	public static void IRQ29()
	{
	}

	public static void IRQ30()
	{
	}

	public static void IRQ31()
	{
	}

	public static void IRQ32()
	{
	}

	public static void IRQ33()
	{
	}

	public static void IRQ34()
	{
	}

	public static void IRQ35()
	{
	}

	public static void IRQ36()
	{
	}

	public static void IRQ37()
	{
	}

	public static void IRQ38()
	{
	}

	public static void IRQ39()
	{
	}

	public static void IRQ40()
	{
	}

	public static void IRQ41()
	{
	}

	public static void IRQ42()
	{
	}

	public static void IRQ43()
	{
	}

	public static void IRQ44()
	{
	}

	public static void IRQ45()
	{
	}

	public static void IRQ46()
	{
	}

	public static void IRQ47()
	{
	}

	public static void IRQ48()
	{
	}

	public static void IRQ49()
	{
	}

	public static void IRQ50()
	{
	}

	public static void IRQ51()
	{
	}

	public static void IRQ52()
	{
	}

	public static void IRQ53()
	{
	}

	public static void IRQ54()
	{
	}

	public static void IRQ55()
	{
	}

	public static void IRQ56()
	{
	}

	public static void IRQ57()
	{
	}

	public static void IRQ58()
	{
	}

	public static void IRQ59()
	{
	}

	public static void IRQ60()
	{
	}

	public static void IRQ61()
	{
	}

	public static void IRQ62()
	{
	}

	public static void IRQ63()
	{
	}

	public static void IRQ64()
	{
	}

	public static void IRQ65()
	{
	}

	public static void IRQ66()
	{
	}

	public static void IRQ67()
	{
	}

	public static void IRQ68()
	{
	}

	public static void IRQ69()
	{
	}

	public static void IRQ70()
	{
	}

	public static void IRQ71()
	{
	}

	public static void IRQ72()
	{
	}

	public static void IRQ73()
	{
	}

	public static void IRQ74()
	{
	}

	public static void IRQ75()
	{
	}

	public static void IRQ76()
	{
	}

	public static void IRQ77()
	{
	}

	public static void IRQ78()
	{
	}

	public static void IRQ79()
	{
	}

	public static void IRQ80()
	{
	}

	public static void IRQ81()
	{
	}

	public static void IRQ82()
	{
	}

	public static void IRQ83()
	{
	}

	public static void IRQ84()
	{
	}

	public static void IRQ85()
	{
	}

	public static void IRQ86()
	{
	}

	public static void IRQ87()
	{
	}

	public static void IRQ88()
	{
	}

	public static void IRQ89()
	{
	}

	public static void IRQ90()
	{
	}

	public static void IRQ91()
	{
	}

	public static void IRQ92()
	{
	}

	public static void IRQ93()
	{
	}

	public static void IRQ94()
	{
	}

	public static void IRQ95()
	{
	}

	public static void IRQ96()
	{
	}

	public static void IRQ97()
	{
	}

	public static void IRQ98()
	{
	}

	public static void IRQ99()
	{
	}

	public static void IRQ100()
	{
	}

	public static void IRQ101()
	{
	}

	public static void IRQ102()
	{
	}

	public static void IRQ103()
	{
	}

	public static void IRQ104()
	{
	}

	public static void IRQ105()
	{
	}

	public static void IRQ106()
	{
	}

	public static void IRQ107()
	{
	}

	public static void IRQ108()
	{
	}

	public static void IRQ109()
	{
	}

	public static void IRQ110()
	{
	}

	public static void IRQ111()
	{
	}

	public static void IRQ112()
	{
	}

	public static void IRQ113()
	{
	}

	public static void IRQ114()
	{
	}

	public static void IRQ115()
	{
	}

	public static void IRQ116()
	{
	}

	public static void IRQ117()
	{
	}

	public static void IRQ118()
	{
	}

	public static void IRQ119()
	{
	}

	public static void IRQ120()
	{
	}

	public static void IRQ121()
	{
	}

	public static void IRQ122()
	{
	}

	public static void IRQ123()
	{
	}

	public static void IRQ124()
	{
	}

	public static void IRQ125()
	{
	}

	public static void IRQ126()
	{
	}

	public static void IRQ127()
	{
	}

	public static void IRQ128()
	{
	}

	public static void IRQ129()
	{
	}

	public static void IRQ130()
	{
	}

	public static void IRQ131()
	{
	}

	public static void IRQ132()
	{
	}

	public static void IRQ133()
	{
	}

	public static void IRQ134()
	{
	}

	public static void IRQ135()
	{
	}

	public static void IRQ136()
	{
	}

	public static void IRQ137()
	{
	}

	public static void IRQ138()
	{
	}

	public static void IRQ139()
	{
	}

	public static void IRQ140()
	{
	}

	public static void IRQ141()
	{
	}

	public static void IRQ142()
	{
	}

	public static void IRQ143()
	{
	}

	public static void IRQ144()
	{
	}

	public static void IRQ145()
	{
	}

	public static void IRQ146()
	{
	}

	public static void IRQ147()
	{
	}

	public static void IRQ148()
	{
	}

	public static void IRQ149()
	{
	}

	public static void IRQ150()
	{
	}

	public static void IRQ151()
	{
	}

	public static void IRQ152()
	{
	}

	public static void IRQ153()
	{
	}

	public static void IRQ154()
	{
	}

	public static void IRQ155()
	{
	}

	public static void IRQ156()
	{
	}

	public static void IRQ157()
	{
	}

	public static void IRQ158()
	{
	}

	public static void IRQ159()
	{
	}

	public static void IRQ160()
	{
	}

	public static void IRQ161()
	{
	}

	public static void IRQ162()
	{
	}

	public static void IRQ163()
	{
	}

	public static void IRQ164()
	{
	}

	public static void IRQ165()
	{
	}

	public static void IRQ166()
	{
	}

	public static void IRQ167()
	{
	}

	public static void IRQ168()
	{
	}

	public static void IRQ169()
	{
	}

	public static void IRQ170()
	{
	}

	public static void IRQ171()
	{
	}

	public static void IRQ172()
	{
	}

	public static void IRQ173()
	{
	}

	public static void IRQ174()
	{
	}

	public static void IRQ175()
	{
	}

	public static void IRQ176()
	{
	}

	public static void IRQ177()
	{
	}

	public static void IRQ178()
	{
	}

	public static void IRQ179()
	{
	}

	public static void IRQ180()
	{
	}

	public static void IRQ181()
	{
	}

	public static void IRQ182()
	{
	}

	public static void IRQ183()
	{
	}

	public static void IRQ184()
	{
	}

	public static void IRQ185()
	{
	}

	public static void IRQ186()
	{
	}

	public static void IRQ187()
	{
	}

	public static void IRQ188()
	{
	}

	public static void IRQ189()
	{
	}

	public static void IRQ190()
	{
	}

	public static void IRQ191()
	{
	}

	public static void IRQ192()
	{
	}

	public static void IRQ193()
	{
	}

	public static void IRQ194()
	{
	}

	public static void IRQ195()
	{
	}

	public static void IRQ196()
	{
	}

	public static void IRQ197()
	{
	}

	public static void IRQ198()
	{
	}

	public static void IRQ199()
	{
	}

	public static void IRQ200()
	{
	}

	public static void IRQ201()
	{
	}

	public static void IRQ202()
	{
	}

	public static void IRQ203()
	{
	}

	public static void IRQ204()
	{
	}

	public static void IRQ205()
	{
	}

	public static void IRQ206()
	{
	}

	public static void IRQ207()
	{
	}

	public static void IRQ208()
	{
	}

	public static void IRQ209()
	{
	}

	public static void IRQ210()
	{
	}

	public static void IRQ211()
	{
	}

	public static void IRQ212()
	{
	}

	public static void IRQ213()
	{
	}

	public static void IRQ214()
	{
	}

	public static void IRQ215()
	{
	}

	public static void IRQ216()
	{
	}

	public static void IRQ217()
	{
	}

	public static void IRQ218()
	{
	}

	public static void IRQ219()
	{
	}

	public static void IRQ220()
	{
	}

	public static void IRQ221()
	{
	}

	public static void IRQ222()
	{
	}

	public static void IRQ223()
	{
	}

	public static void IRQ224()
	{
	}

	public static void IRQ225()
	{
	}

	public static void IRQ226()
	{
	}

	public static void IRQ227()
	{
	}

	public static void IRQ228()
	{
	}

	public static void IRQ229()
	{
	}

	public static void IRQ230()
	{
	}

	public static void IRQ231()
	{
	}

	public static void IRQ232()
	{
	}

	public static void IRQ233()
	{
	}

	public static void IRQ234()
	{
	}

	public static void IRQ235()
	{
	}

	public static void IRQ236()
	{
	}

	public static void IRQ237()
	{
	}

	public static void IRQ238()
	{
	}

	public static void IRQ239()
	{
	}

	public static void IRQ240()
	{
	}

	public static void IRQ241()
	{
	}

	public static void IRQ242()
	{
	}

	public static void IRQ243()
	{
	}

	public static void IRQ244()
	{
	}

	public static void IRQ245()
	{
	}

	public static void IRQ246()
	{
	}

	public static void IRQ247()
	{
	}

	public static void IRQ248()
	{
	}

	public static void IRQ249()
	{
	}

	public static void IRQ250()
	{
	}

	public static void IRQ251()
	{
	}

	public static void IRQ252()
	{
	}

	public static void IRQ253()
	{
	}

	public static void IRQ254()
	{
	}

	public static void IRQ255()
	{
	}

	#endregion IRQs Intrinsic
}
