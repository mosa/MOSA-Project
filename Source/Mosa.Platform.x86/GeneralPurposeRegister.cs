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

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Represents integral general purpose x86 registers.
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
			/// The x86 EAX register instruction encoding.
			/// </summary>
			EAX = 0,

			/// <summary>
			/// The x86 ECX register instruction encoding.
			/// </summary>
			ECX = 1,

			/// <summary>
			/// The x86 EDX register instruction encoding.
			/// </summary>
			EDX = 2,

			/// <summary>
			/// The x86 EBX register instruction encoding.
			/// </summary>
			EBX = 3,

			/// <summary>
			/// The x86 ESP register instruction encoding.
			/// </summary>
			ESP = 4,

			/// <summary>
			/// The x86 EBP register instruction encoding.
			/// </summary>
			EBP = 5,

			/// <summary>
			/// The x86 ESI register instruction encoding.
			/// </summary>
			ESI = 6,

			/// <summary>
			/// The x86 EDI register instruction encoding.
			/// </summary>
			EDI = 7
		}

		#endregion // Types

		#region Static data members

		/// <summary>
		/// Represents the EAX register.
		/// </summary>
		public static readonly GeneralPurposeRegister EAX = new GeneralPurposeRegister(0, GPR.EAX);

		/// <summary>
		/// Represents the ECX register.
		/// </summary>
		public static readonly GeneralPurposeRegister ECX = new GeneralPurposeRegister(1, GPR.ECX);

		/// <summary>
		/// Represents the EDX register.
		/// </summary>
		public static readonly GeneralPurposeRegister EDX = new GeneralPurposeRegister(2, GPR.EDX);

		/// <summary>
		/// Represents the EBX register.
		/// </summary>
		public static readonly GeneralPurposeRegister EBX = new GeneralPurposeRegister(3, GPR.EBX);

		/// <summary>
		/// Represents the ESP register.
		/// </summary>
		public static readonly GeneralPurposeRegister ESP = new GeneralPurposeRegister(4, GPR.ESP);

		/// <summary>
		/// Represents the EBP register.
		/// </summary>
		public static readonly GeneralPurposeRegister EBP = new GeneralPurposeRegister(5, GPR.EBP);

		/// <summary>
		/// Represents the ESI register.
		/// </summary>
		public static readonly GeneralPurposeRegister ESI = new GeneralPurposeRegister(6, GPR.ESI);

		/// <summary>
		/// Represents the EDI register.
		/// </summary>
		public static readonly GeneralPurposeRegister EDI = new GeneralPurposeRegister(7, GPR.EDI);

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
