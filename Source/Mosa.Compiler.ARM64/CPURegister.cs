// Copyright (c) MOSA Project. Licensed under the New BSD License.
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64;

public static class CPURegister
{
	#region General Purpose Registers

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
	/// Represents the R4 register.
	/// </summary>
	public static readonly PhysicalRegister R4 = new PhysicalRegister(4, 4, "R4", true, false);

	/// <summary>
	/// Represents the R5 register.
	/// </summary>
	public static readonly PhysicalRegister R5 = new PhysicalRegister(5, 5, "R5", true, false);

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
	/// Represents the SP/R13 register.
	/// </summary>
	public static readonly PhysicalRegister R13 = new PhysicalRegister(13, 13, "R13", true, false);

	/// <summary>
	/// Represents the LR/R14 register.
	/// </summary>
	public static readonly PhysicalRegister R14 = new PhysicalRegister(14, 14, "R14", true, false);

	/// <summary>
	/// Represents the PC/R15 register.
	/// </summary>
	public static readonly PhysicalRegister R15 = new PhysicalRegister(15, 15, "R15", true, false);

	/// <summary>
	/// Represents the R16 register.
	/// </summary>
	public static readonly PhysicalRegister R16 = new PhysicalRegister(16, 16, "R16", true, false);

	/// <summary>
	/// Represents the R17 register.
	/// </summary>
	public static readonly PhysicalRegister R17 = new PhysicalRegister(17, 17, "R17", true, false);

	/// <summary>
	/// Represents the R18 register.
	/// </summary>
	public static readonly PhysicalRegister R18 = new PhysicalRegister(18, 18, "R18", true, false);

	/// <summary>
	/// Represents the R19 register.
	/// </summary>
	public static readonly PhysicalRegister R19 = new PhysicalRegister(19, 19, "R19", true, false);

	/// <summary>
	/// Represents the R20 register.
	/// </summary>
	public static readonly PhysicalRegister R20 = new PhysicalRegister(20, 20, "R20", true, false);

	/// <summary>
	/// Represents the R21 register.
	/// </summary>
	public static readonly PhysicalRegister R21 = new PhysicalRegister(21, 21, "R21", true, false);

	/// <summary>
	/// Represents the R22 register.
	/// </summary>
	public static readonly PhysicalRegister R22 = new PhysicalRegister(22, 22, "R22", true, false);

	/// <summary>
	/// Represents the R23 register.
	/// </summary>
	public static readonly PhysicalRegister R23 = new PhysicalRegister(23, 23, "R23", true, false);

	/// <summary>
	/// Represents the R24 register.
	/// </summary>
	public static readonly PhysicalRegister R24 = new PhysicalRegister(24, 24, "R24", true, false);

	/// <summary>
	/// Represents the R25 register.
	/// </summary>
	public static readonly PhysicalRegister R25 = new PhysicalRegister(25, 25, "R25", true, false);

	/// <summary>
	/// Represents the R26 register.
	/// </summary>
	public static readonly PhysicalRegister R26 = new PhysicalRegister(26, 26, "R26", true, false);

	/// <summary>
	/// Represents the R27 register.
	/// </summary>
	public static readonly PhysicalRegister R27 = new PhysicalRegister(27, 27, "R27", true, false);

	/// <summary>
	/// Represents the R28 register.
	/// </summary>
	public static readonly PhysicalRegister R28 = new PhysicalRegister(28, 28, "R28", true, false);

	/// <summary>
	/// Represents the R29/FP register.
	/// </summary>
	public static readonly PhysicalRegister FP = new PhysicalRegister(29, 29, "FP", true, false);

	public static PhysicalRegister R29 => FP;

	/// <summary>
	/// Represents the R30/LR register.
	/// </summary>
	public static readonly PhysicalRegister LR = new PhysicalRegister(30, 30, "LR", true, false);

	public static PhysicalRegister R30 => LR;

	/// <summary>
	/// Represents the R31 register.
	/// </summary>
	public static readonly PhysicalRegister R31 = new PhysicalRegister(31, 31, "R31", true, false);

	public static PhysicalRegister SP => R31;

	public static PhysicalRegister ZERO => R31;

	#endregion General Purpose Registers

	#region Floating Point

	/// <summary>
	/// Represents floating point register d0.
	/// </summary>
	public static readonly PhysicalRegister d0 = new PhysicalRegister(32, 0, "d0", false, true);

	/// <summary>
	/// Represents floating point register d1.
	/// </summary>
	public static readonly PhysicalRegister d1 = new PhysicalRegister(33, 1, "d1", false, true);

	/// <summary>
	/// Represents floating point register d2.
	/// </summary>
	public static readonly PhysicalRegister d2 = new PhysicalRegister(34, 2, "d2", false, true);

	/// <summary>
	/// Represents floating point register d3.
	/// </summary>
	public static readonly PhysicalRegister d3 = new PhysicalRegister(35, 3, "d3", false, true);

	/// <summary>
	/// Represents floating point register d4.
	/// </summary>
	public static readonly PhysicalRegister d4 = new PhysicalRegister(36, 4, "d4", false, true);

	/// <summary>
	/// Represents floating point register d5.
	/// </summary>
	public static readonly PhysicalRegister d5 = new PhysicalRegister(37, 5, "d5", false, true);

	/// <summary>
	/// Represents floating point register d6.
	/// </summary>
	public static readonly PhysicalRegister d6 = new PhysicalRegister(38, 6, "d6", false, true);

	/// <summary>
	/// Represents floating point register d7.
	/// </summary>
	public static readonly PhysicalRegister d7 = new PhysicalRegister(39, 7, "d7", false, true);

	/// <summary>
	/// Represents floating point register d7.
	/// </summary>
	public static readonly PhysicalRegister d8 = new PhysicalRegister(40, 8, "d8", false, true);

	/// <summary>
	/// Represents floating point register d7.
	/// </summary>
	public static readonly PhysicalRegister d9 = new PhysicalRegister(41, 9, "d9", false, true);

	/// <summary>
	/// Represents floating point register d7.
	/// </summary>
	public static readonly PhysicalRegister d10 = new PhysicalRegister(42, 10, "d10", false, true);

	/// <summary>
	/// Represents floating point register d7.
	/// </summary>
	public static readonly PhysicalRegister d11 = new PhysicalRegister(43, 11, "d11", false, true);

	/// <summary>
	/// Represents floating point register d7.
	/// </summary>
	public static readonly PhysicalRegister d12 = new PhysicalRegister(44, 12, "d12", false, true);

	/// <summary>
	/// Represents floating point register d7.
	/// </summary>
	public static readonly PhysicalRegister d13 = new PhysicalRegister(45, 13, "d13", false, true);

	/// <summary>
	/// Represents floating point register d7.
	/// </summary>
	public static readonly PhysicalRegister d14 = new PhysicalRegister(46, 14, "d14", false, true);

	/// <summary>
	/// Represents floating point register d7.
	/// </summary>
	public static readonly PhysicalRegister d15 = new PhysicalRegister(47, 15, "d15", false, true);

	/// <summary>
	/// Represents floating point register d16.
	/// </summary>
	public static readonly PhysicalRegister d16 = new PhysicalRegister(48, 16, "d16", false, true);

	/// <summary>
	/// Represents floating point register d17.
	/// </summary>
	public static readonly PhysicalRegister d17 = new PhysicalRegister(49, 17, "d17", false, true);

	/// <summary>
	/// Represents floating point register d18.
	/// </summary>
	public static readonly PhysicalRegister d18 = new PhysicalRegister(50, 18, "d18", false, true);

	/// <summary>
	/// Represents floating point register d19.
	/// </summary>
	public static readonly PhysicalRegister d19 = new PhysicalRegister(51, 19, "d19", false, true);

	/// <summary>
	/// Represents floating point register d20.
	/// </summary>
	public static readonly PhysicalRegister d20 = new PhysicalRegister(52, 20, "d20", false, true);

	/// <summary>
	/// Represents floating point register d21.
	/// </summary>
	public static readonly PhysicalRegister d21 = new PhysicalRegister(53, 21, "d21", false, true);

	/// <summary>
	/// Represents floating point register d22.
	/// </summary>
	public static readonly PhysicalRegister d22 = new PhysicalRegister(54, 22, "d22", false, true);

	/// <summary>
	/// Represents floating point register d23.
	/// </summary>
	public static readonly PhysicalRegister d23 = new PhysicalRegister(55, 23, "d23", false, true);

	/// <summary>
	/// Represents floating point register d24.
	/// </summary>
	public static readonly PhysicalRegister d24 = new PhysicalRegister(56, 24, "d24", false, true);

	/// <summary>
	/// Represents floating point register d25.
	/// </summary>
	public static readonly PhysicalRegister d25 = new PhysicalRegister(57, 25, "d25", false, true);

	/// <summary>
	/// Represents floating point register d26.
	/// </summary>
	public static readonly PhysicalRegister d26 = new PhysicalRegister(58, 26, "d26", false, true);

	/// <summary>
	/// Represents floating point register d27.
	/// </summary>
	public static readonly PhysicalRegister d27 = new PhysicalRegister(59, 27, "d27", false, true);

	/// <summary>
	/// Represents floating point register d28.
	/// </summary>
	public static readonly PhysicalRegister d28 = new PhysicalRegister(60, 28, "d28", false, true);

	/// <summary>
	/// Represents floating point register d29.
	/// </summary>
	public static readonly PhysicalRegister d29 = new PhysicalRegister(61, 29, "d29", false, true);

	/// <summary>
	/// Represents floating point register d30.
	/// </summary>
	public static readonly PhysicalRegister d30 = new PhysicalRegister(62, 30, "d30", false, true);

	/// <summary>
	/// Represents floating point register d31.
	/// </summary>
	public static readonly PhysicalRegister d31 = new PhysicalRegister(63, 15, "d31", false, true);

	#endregion Floating Point
}
