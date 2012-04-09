/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Base class for x86 segment registers.
	/// </summary>
	public sealed class SegmentRegister : Register
	{
		#region Types

		/// <summary>
		/// Identifies x86 segment registers using their instruction encoding.
		/// </summary>
		public enum SegmentType
		{
			/// <summary>
			/// The x86 ES register instruction encoding.
			/// </summary>
			ES = 0,

			/// <summary>
			/// The x86 CS register instruction encoding.
			/// </summary>
			CS = 1,

			/// <summary>
			/// The x86 SS register instruction encoding.
			/// </summary>
			SS = 2,

			/// <summary>
			/// The x86 DS register instruction encoding.
			/// </summary>
			DS = 3,

			/// <summary>
			/// The x86 FS register instruction encoding.
			/// </summary>
			FS = 4,

			/// <summary>
			/// The x86 GS register instruction encoding.
			/// </summary>
			GS = 5
		}

		#endregion // Types

		#region Static data members

		/// <summary>
		/// Represents the CS register.
		/// </summary>
		public static readonly SegmentRegister CS = new SegmentRegister(SegmentType.CS, 24);

		/// <summary>
		/// Represents the DS register.
		/// </summary>
		public static readonly SegmentRegister DS = new SegmentRegister(SegmentType.DS, 25);

		/// <summary>
		/// Represents the ES register.
		/// </summary>
		public static readonly SegmentRegister ES = new SegmentRegister(SegmentType.ES, 26);

		/// <summary>
		/// Represents the FS register.
		/// </summary>
		public static readonly SegmentRegister FS = new SegmentRegister(SegmentType.FS, 27);

		/// <summary>
		/// Represents the GS register.
		/// </summary>
		public static readonly SegmentRegister GS = new SegmentRegister(SegmentType.GS, 28);

		/// <summary>
		/// Represents the SS register.
		/// </summary>
		public static readonly SegmentRegister SS = new SegmentRegister(SegmentType.SS, 29);

		#endregion // Static data members

		/// <summary>
		/// Stores the general purpose register identified by this object instance.
		/// </summary>
		private readonly SegmentType segment;

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="SegmentRegister"/>.
		/// </summary>
		/// <param name="segment">The segment.</param>
		private SegmentRegister(SegmentType segment, int index) :
			base(index)
		{
			this.segment = segment;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the segment.
		/// </summary>
		/// <value>The segment.</value>
		public SegmentType Segment { get { return segment; } }

		/// <summary>
		/// Returns the index of this register.
		/// </summary>
		public override int RegisterCode
		{
			get { return (int)segment; }
		}

		/// <summary>
		/// General purpose register do not support floating point operations.
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
			get { return 16; }
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
		/// Returns the name of the segment register.
		/// </summary>
		/// <returns>The name of the segment register.</returns>
		public override string ToString()
		{
			return segment.ToString();
		}

		#endregion // Methods
	}
}
