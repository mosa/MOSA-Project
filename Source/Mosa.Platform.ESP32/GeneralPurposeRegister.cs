// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ESP32
{
	/// <summary>
	/// Represents integral general purpose ESP32 registers.
	/// </summary>
	public static class GeneralPurposeRegister
	{
		#region Static data members

		/// <summary>
		/// Represents the R0 register.
		/// </summary>
		public static readonly PhysicalRegister A0 = new PhysicalRegister(0, 0, "R0", true, false);

		/// <summary>
		/// Represents the R1/Stack Pointer register.
		/// </summary>
		public static readonly PhysicalRegister SP = new PhysicalRegister(1, 1, "SP", true, false);

		/// <summary>
		/// Represents the R2 register.
		/// </summary>
		public static readonly PhysicalRegister A2 = new PhysicalRegister(2, 2, "A2", true, false);

		/// <summary>
		/// Represents the R3 register.
		/// </summary>
		public static readonly PhysicalRegister A3 = new PhysicalRegister(3, 3, "A3", true, false);

		/// <summary>
		/// Represents the R4 register.
		/// </summary>
		public static readonly PhysicalRegister A4 = new PhysicalRegister(4, 4, "A4", true, false);

		/// <summary>
		/// Represents the R5 register.
		/// </summary>
		public static readonly PhysicalRegister A5 = new PhysicalRegister(5, 5, "A5", true, false);

		/// <summary>
		/// Represents the R6 register.
		/// </summary>
		public static readonly PhysicalRegister A6 = new PhysicalRegister(6, 6, "A6", true, false);

		/// <summary>
		/// Represents the R7 register.
		/// </summary>
		public static readonly PhysicalRegister A7 = new PhysicalRegister(7, 7, "A7", true, false);

		/// <summary>
		/// Represents the R8 register.
		/// </summary>
		public static readonly PhysicalRegister A8 = new PhysicalRegister(8, 8, "A8", true, false);

		/// <summary>
		/// Represents the R9 register.
		/// </summary>
		public static readonly PhysicalRegister A9 = new PhysicalRegister(9, 9, "A9", true, false);

		/// <summary>
		/// Represents the R10 register.
		/// </summary>
		public static readonly PhysicalRegister A10 = new PhysicalRegister(10, 10, "A10", true, false);

		/// <summary>
		/// Represents the R11 register.
		/// </summary>
		public static readonly PhysicalRegister A11 = new PhysicalRegister(11, 11, "A11", true, false);

		/// <summary>
		/// Represents the R12 register.
		/// </summary>
		public static readonly PhysicalRegister A12 = new PhysicalRegister(12, 12, "A12", true, false);

		/// <summary>
		/// Represents the R13 register.
		/// </summary>
		public static readonly PhysicalRegister A13 = new PhysicalRegister(13, 13, "A13", true, false);

		/// <summary>
		/// Represents the R14 register.
		/// </summary>
		public static readonly PhysicalRegister A14 = new PhysicalRegister(14, 14, "A14", true, false);

		/// <summary>
		/// Represents the R15 register.
		/// </summary>
		public static readonly PhysicalRegister A15 = new PhysicalRegister(15, 15, "A15", true, false);

		/// <summary>
		/// Represents the PC register.
		/// </summary>
		public static readonly PhysicalRegister PC = new PhysicalRegister(16, 16, "PC", true, false);

		/// <summary>
		/// Represents the Shift-amount register.
		/// </summary>
		public static readonly PhysicalRegister SAR = new PhysicalRegister(17, 17, "SAR", true, false);

		#endregion Static data members
	}
}
