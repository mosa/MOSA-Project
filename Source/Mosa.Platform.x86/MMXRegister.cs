/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Represents an MMX register.
	/// </summary>
	public sealed class MMXRegister : Register
	{
		#region Static data members

		/// <summary>
		/// Represents the MMX register MM0.
		/// </summary>
		public static readonly MMXRegister MM0 = new MMXRegister(8, 0);

		/// <summary>
		/// Represents the MMX register MM1.
		/// </summary>
		public static readonly MMXRegister MM1 = new MMXRegister(9, 1);

		/// <summary>
		/// Represents the MMX register MM2.
		/// </summary>
		public static readonly MMXRegister MM2 = new MMXRegister(10, 2);

		/// <summary>
		/// Represents the MMX register MM3.
		/// </summary>
		public static readonly MMXRegister MM3 = new MMXRegister(11, 3);

		/// <summary>
		/// Represents the MMX register MM4.
		/// </summary>
		public static readonly MMXRegister MM4 = new MMXRegister(12, 4);

		/// <summary>
		/// Represents the MMX register MM5.
		/// </summary>
		public static readonly MMXRegister MM5 = new MMXRegister(13, 5);

		/// <summary>
		/// Represents the MMX register MM6.
		/// </summary>
		public static readonly MMXRegister MM6 = new MMXRegister(14, 6);

		/// <summary>
		/// Represents the MMX register MM7.
		/// </summary>
		public static readonly MMXRegister MM7 = new MMXRegister(15, 7);

		#endregion // Static data members

		#region Data members

		/// <summary>
		/// The register index.
		/// </summary>
		private int _registerCode;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the MMX register.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="registerCode">The MMX register index.</param>
		private MMXRegister(int index, int registerCode) :
			base(index)
		{
			_registerCode = registerCode;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// MMX registers do not support fp operation.
		/// </summary>
		public override bool IsFloatingPoint
		{
			get { return false; }
		}

		/// <summary>
		/// Retrieves the register index.
		/// </summary>
		public override int RegisterCode
		{
			get { return _registerCode; }
		}

		/// <summary>
		/// Returns the width of MMX registers.
		/// </summary>
		public override int Width
		{
			get { return 64; }
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
			return (type.Type == CilElementType.I8 || type.Type == CilElementType.U8);
		}

		/// <summary>
		/// Returns the string representation of the register.
		/// </summary>
		/// <returns>The string representation of the register.</returns>
		public override string ToString()
		{
			return String.Format("MM{0}", _registerCode);
		}

		#endregion // Methods
	}
}
