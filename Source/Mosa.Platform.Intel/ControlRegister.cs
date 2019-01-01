// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.Intel
{
	/// <summary>
	/// Base class for x86 control registers.
	/// </summary>
	public static class ControlRegister
	{
		#region Static data members

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

		#endregion Static data members
	}
}
