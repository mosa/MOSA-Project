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
	/// Represents an x86 SSE2 register.
	/// </summary>
	/// <remarks>
	/// SSE2 is used by floating point instructions for their operands. An
	/// SSE2 register allows storage of double precision floating point values (64 bit)
	/// as required by the CIL standard.
	/// </remarks>
	public sealed class SSE2Register : Register
	{
		#region Static data members

		/// <summary>
		/// Represents SSE2 register XMM0.
		/// </summary>
		public static readonly SSE2Register XMM0 = new SSE2Register(16, 0);

		/// <summary>
		/// Represents SSE2 register XMM1.
		/// </summary>
		public static readonly SSE2Register XMM1 = new SSE2Register(17, 1);

		/// <summary>
		/// Represents SSE2 register XMM2.
		/// </summary>
		public static readonly SSE2Register XMM2 = new SSE2Register(18, 2);

		/// <summary>
		/// Represents SSE2 register XMM3.
		/// </summary>
		public static readonly SSE2Register XMM3 = new SSE2Register(19, 3);

		/// <summary>
		/// Represents SSE2 register XMM4.
		/// </summary>
		public static readonly SSE2Register XMM4 = new SSE2Register(20, 4);

		/// <summary>
		/// Represents SSE2 register XMM5.
		/// </summary>
		public static readonly SSE2Register XMM5 = new SSE2Register(21, 5);

		/// <summary>
		/// Represents SSE2 register XMM6.
		/// </summary>
		public static readonly SSE2Register XMM6 = new SSE2Register(22, 6);

		/// <summary>
		/// Represents SSE2 register XMM7.
		/// </summary>
		public static readonly SSE2Register XMM7 = new SSE2Register(23, 7);

		#endregion // Static data members

		#region Data members

		/// <summary>
		/// Stores the register index of this instance.
		/// </summary>
		private readonly int _registerCode;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new SSE2Register.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="registerCode">The SSE2 register index of this instance.</param>
		private SSE2Register(int index, int registerCode) :
			base(index)
		{
			_registerCode = registerCode;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Indicates that SSE2 registers are floating point register.
		/// </summary>
		public override bool IsFloatingPoint
		{
			get { return true; }
		}

		/// <summary>
		/// Returns the register index of this register.
		/// </summary>
		public override int RegisterCode
		{
			get { return _registerCode; }
		}

		/// <summary>
		/// Returns the width of the register.
		/// </summary>
		public override int Width
		{
			get { return 64; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Checks if the signature type is valid for this register type.
		/// </summary>
		/// <param name="type">The signature type to check.</param>
		/// <returns>True, if the register can store this signature type.</returns>
		public override bool IsValidSigType(SigType type)
		{
			return (type.Type == CilElementType.R4 || type.Type == CilElementType.R8 || type.Type == CilElementType.I8 || type.Type == CilElementType.U8);
		}

		/// <summary>
		/// Retrieves the SSE2 register name.
		/// </summary>
		/// <returns>The SSE2 register name.</returns>
		public override string ToString()
		{
			return String.Format("XMM#{0}", _registerCode);
		}

		#endregion // Methods
	}
}
