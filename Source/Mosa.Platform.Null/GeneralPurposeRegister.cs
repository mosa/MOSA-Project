/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.Null
{
	/// <summary>
	/// Represents integral general purpose null registers.
	/// </summary>
	public sealed class GeneralPurposeRegister : Register
	{
		#region Types

		/// <summary>
		/// Identifies x86 general purpose registers using their instruction encoding.
		/// </summary>
		private enum GPR
		{
			/// <summary>
			/// 
			/// </summary>
			R0 = 0,

			/// <summary>
			/// 
			/// </summary>
			R1 = 1,

			/// <summary>
			/// 
			/// </summary>
			R2 = 2,

			/// <summary>
			/// 
			/// </summary>
			R3 = 3,

			/// <summary>
			/// 
			/// </summary>
			R4 = 4,

			/// <summary>
			/// 
			/// </summary>
			R5 = 5,

			/// <summary>
			/// 
			/// </summary>
			R6 = 6,

		}

		#endregion // Types

		#region Static data members

		/// <summary>
		/// Represents the R0 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R0 = new GeneralPurposeRegister(0, GPR.R0);

		/// <summary>
		/// Represents the R0 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R1 = new GeneralPurposeRegister(1, GPR.R1);

		/// <summary>
		/// Represents the R0 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R2 = new GeneralPurposeRegister(2, GPR.R2);

		/// <summary>
		/// Represents the R0 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R3 = new GeneralPurposeRegister(3, GPR.R3);

		/// <summary>
		/// Represents the R0 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R4 = new GeneralPurposeRegister(4, GPR.R4);

		/// <summary>
		/// Represents the R0 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R5 = new GeneralPurposeRegister(5, GPR.R5);

		/// <summary>
		/// Represents the R0 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R6 = new GeneralPurposeRegister(6, GPR.R6);


		#endregion // Static data members

		#region Data members

		/// <summary>
		/// Stores the general purpose register identified by this object instance.
		/// </summary>
		private readonly GPR gpr;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="GeneralPurposeRegister"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="gpr">The general purpose register index.</param>
		private GeneralPurposeRegister(int index, GPR gpr) :
			base(index)
		{
			this.gpr = gpr;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// General purpose register do not support floating point operations.
		/// </summary>
		public override bool IsFloatingPoint
		{
			get { return false; }
		}

		/// <summary>
		/// Returns the index of this register.
		/// </summary>
		public override int RegisterCode
		{
			get { return (int)gpr; }
		}

		/// <summary>
		/// Returns the width of general purpose registers in bits.
		/// </summary>
		public override int Width
		{
			get { return 32; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Determines if the signature type fits into the register.
		/// </summary>
		/// <param name="type">The signature type to check.</param>
		/// <returns>True if the signature type fits.</returns>
		public override bool IsValidSigType(SigType type)
		{
			return (type.Type == CilElementType.I ||
					type.Type == CilElementType.I1 ||
					type.Type == CilElementType.I2 ||
					type.Type == CilElementType.I4 ||
					type.Type == CilElementType.U1 ||
					type.Type == CilElementType.U2 ||
					type.Type == CilElementType.U4 ||
					type.Type == CilElementType.Ptr ||
					type.Type == CilElementType.ByRef ||
					type.Type == CilElementType.FunctionPtr ||
					type.Type == CilElementType.Object);
		}

		/// <summary>
		/// Returns the name of the general purpose register.
		/// </summary>
		/// <returns>The name of the general purpose register.</returns>
		public override string ToString()
		{
			return gpr.ToString();
		}

		#endregion // Methods
	}
}
