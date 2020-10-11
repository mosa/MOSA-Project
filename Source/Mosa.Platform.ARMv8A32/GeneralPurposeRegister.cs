// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32
{
	/// <summary>
	/// Represents integral general purpose ARMv8A32 registers.
	/// </summary>
	public static class GeneralPurposeRegister
	{
		#region Static data members

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

		#endregion Static data members
	}
}
