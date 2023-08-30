// Copyright (c) MOSA Project. Licensed under the New BSD License.
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32;

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
	public static readonly PhysicalRegister FP = new PhysicalRegister(11, 11, "FP", true, false);

	/// <summary>
	/// Represents the R12 register.
	/// </summary>
	public static readonly PhysicalRegister R12 = new PhysicalRegister(12, 12, "R12", true, false);

	/// <summary>
	/// Represents the SP/R13 register.
	/// </summary>
	public static readonly PhysicalRegister SP = new PhysicalRegister(13, 13, "SP", true, false);

	/// <summary>
	/// Represents the LR/R14 register.
	/// </summary>
	public static readonly PhysicalRegister LR = new PhysicalRegister(14, 14, "LR", true, false);

	/// <summary>
	/// Represents the PC/R15 register.
	/// </summary>
	public static readonly PhysicalRegister PC = new PhysicalRegister(15, 15, "PC", true, false);

	#endregion General Purpose Registers

	#region Floating Point

	/// <summary>
	/// Represents SSE2 register d0.
	/// </summary>
	public static readonly PhysicalRegister d0 = new PhysicalRegister(16, 0, "d0", false, true);

	/// <summary>
	/// Represents SSE2 register d1.
	/// </summary>
	public static readonly PhysicalRegister d1 = new PhysicalRegister(17, 1, "d1", false, true);

	/// <summary>
	/// Represents SSE2 register d2.
	/// </summary>
	public static readonly PhysicalRegister d2 = new PhysicalRegister(18, 2, "d2", false, true);

	/// <summary>
	/// Represents SSE2 register d3.
	/// </summary>
	public static readonly PhysicalRegister d3 = new PhysicalRegister(19, 3, "d3", false, true);

	/// <summary>
	/// Represents SSE2 register d4.
	/// </summary>
	public static readonly PhysicalRegister d4 = new PhysicalRegister(20, 4, "d4", false, true);

	/// <summary>
	/// Represents SSE2 register d5.
	/// </summary>
	public static readonly PhysicalRegister d5 = new PhysicalRegister(21, 5, "d5", false, true);

	/// <summary>
	/// Represents SSE2 register d6.
	/// </summary>
	public static readonly PhysicalRegister d6 = new PhysicalRegister(22, 6, "d6", false, true);

	/// <summary>
	/// Represents SSE2 register d7.
	/// </summary>
	public static readonly PhysicalRegister d7 = new PhysicalRegister(23, 7, "d7", false, true);

	/// <summary>
	/// Represents SSE2 register d7.
	/// </summary>
	public static readonly PhysicalRegister d8 = new PhysicalRegister(24, 8, "d8", false, true);

	/// <summary>
	/// Represents SSE2 register d7.
	/// </summary>
	public static readonly PhysicalRegister d9 = new PhysicalRegister(25, 9, "d9", false, true);

	/// <summary>
	/// Represents SSE2 register d7.
	/// </summary>
	public static readonly PhysicalRegister d10 = new PhysicalRegister(26, 10, "d10", false, true);

	/// <summary>
	/// Represents SSE2 register d7.
	/// </summary>
	public static readonly PhysicalRegister d11 = new PhysicalRegister(27, 11, "d11", false, true);

	/// <summary>
	/// Represents SSE2 register d7.
	/// </summary>
	public static readonly PhysicalRegister d12 = new PhysicalRegister(28, 12, "d12", false, true);

	/// <summary>
	/// Represents SSE2 register d7.
	/// </summary>
	public static readonly PhysicalRegister d13 = new PhysicalRegister(29, 13, "d13", false, true);

	/// <summary>
	/// Represents SSE2 register d7.
	/// </summary>
	public static readonly PhysicalRegister d14 = new PhysicalRegister(30, 14, "d14", false, true);

	/// <summary>
	/// Represents SSE2 register d7.
	/// </summary>
	public static readonly PhysicalRegister d15 = new PhysicalRegister(31, 15, "d15", false, true);

	#endregion Floating Point
}
