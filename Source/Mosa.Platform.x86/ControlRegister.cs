/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Base class for x86 control registers.
	/// </summary>
	public sealed class ControlRegister : Register
	{
		#region Types

		/// <summary>
		/// Identifies x86 control registers using their instruction encoding.
		/// </summary>
		public enum ControlRegisterType
		{
			/// <summary>
			/// The x86 CR0 register instruction encoding.
			/// </summary>
			CR0 = 0,

			/// <summary>
			/// The x86 CR2 register instruction encoding.
			/// </summary>
			CR2 = 2,

			/// <summary>
			/// The x86 CR3 register instruction encoding.
			/// </summary>
			CR3 = 3,

			/// <summary>
			/// The x86 CR4 register instruction encoding.
			/// </summary>
			CR4 = 4,
		}

		#endregion Types

		#region Static data members

		/// <summary>
		/// Represents the CR0 register.
		/// </summary>
		public static readonly ControlRegister CR0 = new ControlRegister(ControlRegisterType.CR0);

		/// <summary>
		/// Represents the CR2 register.
		/// </summary>
		public static readonly ControlRegister CR2 = new ControlRegister(ControlRegisterType.CR2);

		/// <summary>
		/// Represents the CR3 register.
		/// </summary>
		public static readonly ControlRegister CR3 = new ControlRegister(ControlRegisterType.CR3);

		/// <summary>
		/// Represents the CR4 register.
		/// </summary>
		public static readonly ControlRegister CR4 = new ControlRegister(ControlRegisterType.CR4);

		#endregion Static data members

		/// <summary>
		/// Stores the general purpose register identified by this object instance.
		/// </summary>
		private readonly ControlRegisterType control;

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="SegmentRegister"/>.
		/// </summary>
		/// <param name="control">The control.</param>
		private ControlRegister(ControlRegisterType control) :
			base((int)control)
		{
			this.control = control;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the control.
		/// </summary>
		/// <value>The control.</value>
		public ControlRegisterType Control { get { return control; } }

		/// <summary>
		/// Returns the index of this register.
		/// </summary>
		public override int RegisterCode
		{
			get { return base.Index; }
		}

		/// <summary>
		/// Control registers do not support integer operations.
		/// </summary>
		public override bool IsInteger
		{
			get { return false; }
		}

		/// <summary>
		///
		/// <summary>
		/// Control registers do not support floating point operations.
		/// </summary>
		public override bool IsFloatingPoint
		{
			get { return false; }
		}

		/// <summary>
		/// Returns the width of general purpose registers in bits.
		/// </summary>
		public override int Width
		{
			get { return 32; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Returns the name of the control register.
		/// </summary>
		/// <returns>The name of the control register.</returns>
		public override string ToString()
		{
			return control.ToString();
		}

		#endregion Methods
	}
}