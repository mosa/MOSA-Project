// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.Intel
{
	/// <summary>
	/// Represents an x64 SSE2 register.
	/// </summary>
	/// <remarks>
	/// SSE2 is used by floating point instructions for their operands. An
	/// SSE2 register allows storage of double precision floating point values (64 bit)
	/// as required by the CIL standard.
	/// </remarks>
	public static class SSE2Register
	{
		#region Static data members

		/// <summary>
		/// Represents SSE2 register XMM0.
		/// </summary>
		public static readonly PhysicalRegister XMM0 = new PhysicalRegister(8, 0, "XMM#0", false, true);

		/// <summary>
		/// Represents SSE2 register XMM1.
		/// </summary>
		public static readonly PhysicalRegister XMM1 = new PhysicalRegister(9, 1, "XMM#1", false, true);

		/// <summary>
		/// Represents SSE2 register XMM2.
		/// </summary>
		public static readonly PhysicalRegister XMM2 = new PhysicalRegister(10, 2, "XMM#2", false, true);

		/// <summary>
		/// Represents SSE2 register XMM3.
		/// </summary>
		public static readonly PhysicalRegister XMM3 = new PhysicalRegister(11, 3, "XMM#3", false, true);

		/// <summary>
		/// Represents SSE2 register XMM4.
		/// </summary>
		public static readonly PhysicalRegister XMM4 = new PhysicalRegister(12, 4, "XMM#4", false, true);

		/// <summary>
		/// Represents SSE2 register XMM5.
		/// </summary>
		public static readonly PhysicalRegister XMM5 = new PhysicalRegister(13, 5, "XMM#5", false, true);

		/// <summary>
		/// Represents SSE2 register XMM6.
		/// </summary>
		public static readonly PhysicalRegister XMM6 = new PhysicalRegister(14, 6, "XMM#6", false, true);

		/// <summary>
		/// Represents SSE2 register XMM7.
		/// </summary>
		public static readonly PhysicalRegister XMM7 = new PhysicalRegister(15, 7, "XMM#7", false, true);

		#endregion Static data members
	}
}
