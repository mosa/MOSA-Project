/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata.Signatures
{
	public class BuiltInSigType : SigType
	{
		internal static readonly BuiltInSigType Void = new BuiltInSigType(CilElementType.Void);

		internal static readonly BuiltInSigType Boolean = new BuiltInSigType(CilElementType.Boolean);

		internal static readonly BuiltInSigType Char = new BuiltInSigType(CilElementType.Char);

		internal static readonly BuiltInSigType SByte = new BuiltInSigType(CilElementType.I1);

		internal static readonly BuiltInSigType Byte = new BuiltInSigType(CilElementType.U1);

		internal static readonly BuiltInSigType Int16 = new BuiltInSigType(CilElementType.I2);

		internal static readonly BuiltInSigType UInt16 = new BuiltInSigType(CilElementType.U2);

		internal static readonly BuiltInSigType Int32 = new BuiltInSigType(CilElementType.I4);

		internal static readonly BuiltInSigType UInt32 = new BuiltInSigType(CilElementType.U4);

		internal static readonly BuiltInSigType Int64 = new BuiltInSigType(CilElementType.I8);

		internal static readonly BuiltInSigType UInt64 = new BuiltInSigType(CilElementType.U8);

		internal static readonly BuiltInSigType Single = new BuiltInSigType(CilElementType.R4);

		internal static readonly BuiltInSigType Double = new BuiltInSigType(CilElementType.R8);

		internal static readonly BuiltInSigType String = new BuiltInSigType(CilElementType.String);

		internal static readonly BuiltInSigType Object = new BuiltInSigType(CilElementType.Object);

		internal static readonly BuiltInSigType IntPtr = new BuiltInSigType(CilElementType.I);

		internal static readonly BuiltInSigType UIntPtr = new BuiltInSigType(CilElementType.U);

		internal static readonly BuiltInSigType TypedByRef = new BuiltInSigType(CilElementType.TypedByRef);

		//public static readonly BuiltInSigType Ptr = new BuiltInSigType(CilElementType.Ptr);

		internal BuiltInSigType(CilElementType type) :
			base(type)
		{
		}

	}
}