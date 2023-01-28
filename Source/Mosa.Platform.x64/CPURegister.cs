// Copyright (c) MOSA Project. Licensed under the New BSD License.
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64;

public static class CPURegister
{
	#region Translations

	public static PhysicalRegister R4 => RBP;

	public static PhysicalRegister R5 => RSP;

	public static PhysicalRegister RAX => R0;

	public static PhysicalRegister RCX => R1;

	public static PhysicalRegister RDX => R2;

	public static PhysicalRegister RBX => R3;

	public static PhysicalRegister RSI => R6;

	public static PhysicalRegister RDI => R7;

	#endregion Translations

	#region Physical Registers

	/// <summary>
	/// Represents the R0 register.
	/// </summary>
	public static readonly PhysicalRegister R0 = new PhysicalRegister(0, 0, "R0", true, false);

	/// <summary>
	/// Represents the R1 register.
	/// </summary>
	public static readonly PhysicalRegister R1 = new PhysicalRegister(1, 1, "R1", true, false);

	/// <summary>
	/// Represents the R2 register.
	/// </summary>
	public static readonly PhysicalRegister R2 = new PhysicalRegister(2, 2, "R2", true, false);

	/// <summary>
	/// Represents the R3 register.
	/// </summary>
	public static readonly PhysicalRegister R3 = new PhysicalRegister(3, 3, "R3", true, false);

	/// <summary>
	/// Represents the RSP register.
	/// </summary>
	public static readonly PhysicalRegister RSP = new PhysicalRegister(4, 4, "RSP", true, false);

	/// <summary>
	/// Represents the RBP register.
	/// </summary>
	public static readonly PhysicalRegister RBP = new PhysicalRegister(5, 5, "RBP", true, false);

	/// <summary>
	/// Represents the R6 register.
	/// </summary>
	public static readonly PhysicalRegister R6 = new PhysicalRegister(6, 6, "R6", true, false);

	/// <summary>
	/// Represents the R7 register.
	/// </summary>
	public static readonly PhysicalRegister R7 = new PhysicalRegister(7, 7, "R7", true, false);

	/// <summary>
	/// Represents the R8 register.
	/// </summary>
	public static readonly PhysicalRegister R8 = new PhysicalRegister(8, 8, "R8", true, false);

	/// <summary>
	/// Represents the R9 register.
	/// </summary>
	public static readonly PhysicalRegister R9 = new PhysicalRegister(9, 9, "R9", true, false);

	/// <summary>
	/// Represents the R10 register.
	/// </summary>
	public static readonly PhysicalRegister R10 = new PhysicalRegister(10, 10, "R10", true, false);

	/// <summary>
	/// Represents the R11 register.
	/// </summary>
	public static readonly PhysicalRegister R11 = new PhysicalRegister(11, 11, "R11", true, false);

	/// <summary>
	/// Represents the R12 register.
	/// </summary>
	public static readonly PhysicalRegister R12 = new PhysicalRegister(12, 12, "R12", true, false);

	/// <summary>
	/// Represents the R13 register.
	/// </summary>
	public static readonly PhysicalRegister R13 = new PhysicalRegister(13, 13, "R13", true, false);

	/// <summary>
	/// Represents the R14 register.
	/// </summary>
	public static readonly PhysicalRegister R14 = new PhysicalRegister(14, 14, "R14", true, false);

	/// <summary>
	/// Represents the R15 register.
	/// </summary>
	public static readonly PhysicalRegister R15 = new PhysicalRegister(15, 15, "R15", true, false);

	#endregion Physical Registers

	#region SSE2 Registers

	/// <summary>
	/// Represents SSE2 register XMM0.
	/// </summary>
	public static readonly PhysicalRegister XMM0 = new PhysicalRegister(16, 0, "XMM#0", false, true);

	/// <summary>
	/// Represents SSE2 register XMM1.
	/// </summary>
	public static readonly PhysicalRegister XMM1 = new PhysicalRegister(17, 1, "XMM#1", false, true);

	/// <summary>
	/// Represents SSE2 register XMM2.
	/// </summary>
	public static readonly PhysicalRegister XMM2 = new PhysicalRegister(18, 2, "XMM#2", false, true);

	/// <summary>
	/// Represents SSE2 register XMM3.
	/// </summary>
	public static readonly PhysicalRegister XMM3 = new PhysicalRegister(19, 3, "XMM#3", false, true);

	/// <summary>
	/// Represents SSE2 register XMM4.
	/// </summary>
	public static readonly PhysicalRegister XMM4 = new PhysicalRegister(20, 4, "XMM#4", false, true);

	/// <summary>
	/// Represents SSE2 register XMM5.
	/// </summary>
	public static readonly PhysicalRegister XMM5 = new PhysicalRegister(21, 5, "XMM#5", false, true);

	/// <summary>
	/// Represents SSE2 register XMM6.
	/// </summary>
	public static readonly PhysicalRegister XMM6 = new PhysicalRegister(22, 6, "XMM#6", false, true);

	/// <summary>
	/// Represents SSE2 register XMM7.
	/// </summary>
	public static readonly PhysicalRegister XMM7 = new PhysicalRegister(23, 7, "XMM#7", false, true);

	/// <summary>
	/// Represents SSE2 register XMM7.
	/// </summary>
	public static readonly PhysicalRegister XMM8 = new PhysicalRegister(24, 8, "XMM#8", false, true);

	/// <summary>
	/// Represents SSE2 register XMM7.
	/// </summary>
	public static readonly PhysicalRegister XMM9 = new PhysicalRegister(25, 9, "XMM#9", false, true);

	/// <summary>
	/// Represents SSE2 register XMM7.
	/// </summary>
	public static readonly PhysicalRegister XMM10 = new PhysicalRegister(26, 10, "XMM#10", false, true);

	/// <summary>
	/// Represents SSE2 register XMM7.
	/// </summary>
	public static readonly PhysicalRegister XMM11 = new PhysicalRegister(27, 11, "XMM#11", false, true);

	/// <summary>
	/// Represents SSE2 register XMM7.
	/// </summary>
	public static readonly PhysicalRegister XMM12 = new PhysicalRegister(28, 12, "XMM#12", false, true);

	/// <summary>
	/// Represents SSE2 register XMM7.
	/// </summary>
	public static readonly PhysicalRegister XMM13 = new PhysicalRegister(29, 13, "XMM#13", false, true);

	/// <summary>
	/// Represents SSE2 register XMM7.
	/// </summary>
	public static readonly PhysicalRegister XMM14 = new PhysicalRegister(30, 14, "XMM#14", false, true);

	/// <summary>
	/// Represents SSE2 register XMM7.
	/// </summary>
	public static readonly PhysicalRegister XMM15 = new PhysicalRegister(31, 15, "XMM#15", false, true);

	#endregion SSE2 Registers

	#region Control Registers

	/// <summary>
	/// Represents the CR0 register.
	/// </summary>
	public static readonly PhysicalRegister CR0 = new PhysicalRegister(-1, 0, "CR0", false, false);

	/// <summary>
	/// Represents the CR2 register.
	/// </summary>
	public static readonly PhysicalRegister CR2 = new PhysicalRegister(-1, 2, "CR2", false, false);

	/// <summary>
	/// Represents the CR3 register.
	/// </summary>
	public static readonly PhysicalRegister CR3 = new PhysicalRegister(-1, 3, "CR3", false, false);

	/// <summary>
	/// Represents the CR4 register.
	/// </summary>
	public static readonly PhysicalRegister CR4 = new PhysicalRegister(-1, 4, "CR4", false, false);

	#endregion Control Registers

	#region Segmentation Registers

	/// <summary>
	/// Represents the ES register.
	/// </summary>
	public static readonly PhysicalRegister ES = new PhysicalRegister(-1, 0, "ES", false, false);

	/// <summary>
	/// Represents the CS register.
	/// </summary>
	public static readonly PhysicalRegister CS = new PhysicalRegister(-1, 1, "CS", false, false);

	/// <summary>
	/// Represents the SS register.
	/// </summary>
	public static readonly PhysicalRegister SS = new PhysicalRegister(-1, 2, "SS", false, false);

	/// <summary>
	/// Represents the DS register.
	/// </summary>
	public static readonly PhysicalRegister DS = new PhysicalRegister(-1, 3, "DS", false, false);

	/// <summary>
	/// Represents the FS register.
	/// </summary>
	public static readonly PhysicalRegister FS = new PhysicalRegister(-1, 4, "FS", false, false);

	/// <summary>
	/// Represents the GS register.
	/// </summary>
	public static readonly PhysicalRegister GS = new PhysicalRegister(-1, 5, "GS", false, false);

	#endregion Segmentation Registers
}
