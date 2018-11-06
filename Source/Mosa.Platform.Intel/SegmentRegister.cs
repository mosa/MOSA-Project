// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.Intel
{
	/// <summary>
	/// Base class for x86 segment registers.
	/// </summary>
	public static class SegmentRegister
	{
		#region Static data members

		/// <summary>
		/// Represents the ES register.
		/// </summary>
		public static readonly PhysicalRegister ES = new PhysicalRegister(18, 0, "ES", false, false);

		/// <summary>
		/// Represents the CS register.
		/// </summary>
		public static readonly PhysicalRegister CS = new PhysicalRegister(16, 1, "CS", false, false);

		/// <summary>
		/// Represents the SS register.
		/// </summary>
		public static readonly PhysicalRegister SS = new PhysicalRegister(21, 2, "SS", false, false);

		/// <summary>
		/// Represents the DS register.
		/// </summary>
		public static readonly PhysicalRegister DS = new PhysicalRegister(17, 3, "DS", false, false);

		/// <summary>
		/// Represents the FS register.
		/// </summary>
		public static readonly PhysicalRegister FS = new PhysicalRegister(19, 4, "FS", false, false);

		/// <summary>
		/// Represents the GS register.
		/// </summary>
		public static readonly PhysicalRegister GS = new PhysicalRegister(20, 5, "GS", false, false);

		#endregion Static data members
	}
}
