// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.Intel
{
	/// <summary>
	/// Represents integral general purpose x86 registers.
	/// </summary>
	public static class GeneralPurposeRegister
	{
		#region Static data members

		/// <summary>
		/// Represents the EAX register.
		/// </summary>
		public static readonly PhysicalRegister EAX = new PhysicalRegister(0, 0, "EAX", true, false);

		/// <summary>
		/// Represents the ECX register.
		/// </summary>
		public static readonly PhysicalRegister ECX = new PhysicalRegister(1, 1, "ECX", true, false);

		/// <summary>
		/// Represents the EDX register.
		/// </summary>
		public static readonly PhysicalRegister EDX = new PhysicalRegister(2, 2, "EDX", true, false);

		/// <summary>
		/// Represents the EBX register.
		/// </summary>
		public static readonly PhysicalRegister EBX = new PhysicalRegister(3, 3, "EBX", true, false);

		/// <summary>
		/// Represents the ESP register.
		/// </summary>
		public static readonly PhysicalRegister ESP = new PhysicalRegister(4, 4, "ESP", true, false);

		/// <summary>
		/// Represents the EBP register.
		/// </summary>
		public static readonly PhysicalRegister EBP = new PhysicalRegister(5, 5, "EBP", true, false);

		/// <summary>
		/// Represents the ESI register.
		/// </summary>
		public static readonly PhysicalRegister ESI = new PhysicalRegister(6, 6, "ESI", true, false);

		/// <summary>
		/// Represents the EDI register.
		/// </summary>
		public static readonly PhysicalRegister EDI = new PhysicalRegister(7, 7, "EDI", true, false);

		#endregion Static data members
	}
}
