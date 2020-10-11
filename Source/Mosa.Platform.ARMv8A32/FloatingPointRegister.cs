// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32
{
	/// <summary>
	/// Represents an ARM Floating Point  register.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public static class FloatingPointRegister
	{
		#region Static data members

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

		#endregion Static data members
	}
}
