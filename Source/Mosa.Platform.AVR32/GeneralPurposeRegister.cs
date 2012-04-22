/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// Represents integral general purpose AVR32 registers.
	/// </summary>
	public sealed class GeneralPurposeRegister : Register
	{
		#region Types

		/// <summary>
		/// Identifies AVR32 general purpose registers using their instruction encoding.
		/// </summary>
		private enum GPR
		{
			/// <summary>
			/// The AVR32 R0 register instruction encoding.
			/// </summary>
			R0 = 0,

			/// <summary>
			/// The AVR32 R1 register instruction encoding.
			/// </summary>
			R1 = 1,

			/// <summary>
			/// The AVR32 R2 register instruction encoding.
			/// </summary>
			R2 = 2,

			/// <summary>
			/// The AVR32 R3 register instruction encoding.
			/// </summary>
			R3 = 3,

			/// <summary>
			/// The AVR32 R4 register instruction encoding.
			/// </summary>
			R4 = 4,

			/// <summary>
			/// The AVR32 R5 register instruction encoding.
			/// </summary>
			R5 = 5,

			/// <summary>
			/// The AVR32 R6 register instruction encoding.
			/// </summary>
			R6 = 6,

			/// <summary>
			/// The AVR32 R7 register instruction encoding.
			/// </summary>
			R7 = 7,

			/// <summary>
			/// The AVR32 R8 register instruction encoding.
			/// </summary>
			R8 = 8,

			/// <summary>
			/// The AVR32 R9 register instruction encoding.
			/// </summary>
			R9 = 9,

			/// <summary>
			/// The AVR32 R10 register instruction encoding.
			/// </summary>
			R10 = 10,

			/// <summary>
			/// The AVR32 R11 register instruction encoding.
			/// </summary>
			R11 = 11,

			/// <summary>
			/// The AVR32 R12 register instruction encoding.
			/// </summary>
			R12 = 12,

			/// <summary>
			/// The AVR32 SP/R13 register instruction encoding.
			/// </summary>
			SP = 13,

			/// <summary>
			/// The AVR32 LR/R14 register instruction encoding.
			/// </summary>
			LR = 14
}

		#endregion // Types

		#region Static data members

		/// <summary>
		/// Represents the R0 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R0 = new GeneralPurposeRegister(0, GPR.R0);

		/// <summary>
		/// Represents the R1 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R1 = new GeneralPurposeRegister(1, GPR.R1);

		/// <summary>
		/// Represents the R2 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R2 = new GeneralPurposeRegister(2, GPR.R2);

		/// <summary>
		/// Represents the R3 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R3 = new GeneralPurposeRegister(3, GPR.R3);

		/// <summary>
		/// Represents the R4 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R4 = new GeneralPurposeRegister(4, GPR.R4);

		/// <summary>
		/// Represents the R5 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R5 = new GeneralPurposeRegister(5, GPR.R5);

		/// <summary>
		/// Represents the R6 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R6 = new GeneralPurposeRegister(6, GPR.R6);

		/// <summary>
		/// Represents the R7 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R7 = new GeneralPurposeRegister(7, GPR.R7);

		/// <summary>
		/// Represents the R8 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R8 = new GeneralPurposeRegister(8, GPR.R8);

		/// <summary>
		/// Represents the R9 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R9 = new GeneralPurposeRegister(9, GPR.R9);

		/// <summary>
		/// Represents the R10 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R10 = new GeneralPurposeRegister(10, GPR.R10);

		/// <summary>
		/// Represents the R11 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R11 = new GeneralPurposeRegister(11, GPR.R11);

		/// <summary>
		/// Represents the R12 register.
		/// </summary>
		public static readonly GeneralPurposeRegister R12 = new GeneralPurposeRegister(12, GPR.R12);

		/// <summary>
		/// Represents the SP/R13 register.
		/// </summary>
		public static readonly GeneralPurposeRegister SP = new GeneralPurposeRegister(13, GPR.SP);

		/// <summary>
		/// Represents the LR/R14 register.
		/// </summary>
		public static readonly GeneralPurposeRegister LR = new GeneralPurposeRegister(14, GPR.LR);

		#endregion // Static data members

		#region Data members

		/// <summary>
		/// Stores the general purpose register identified by this object instance.
		/// </summary>
		private readonly GPR _gpr;

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
			_gpr = gpr;
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
			get { return (int)_gpr; }
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
			return _gpr.ToString();
		}

		#endregion // Methods
	}
}
