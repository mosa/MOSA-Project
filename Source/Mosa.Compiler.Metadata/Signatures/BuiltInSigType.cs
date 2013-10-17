/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Metadata.Signatures
{
	public class BuiltInSigType : SigType
	{
		public static readonly BuiltInSigType Void = new BuiltInSigType(@"System.Void", CilElementType.Void);

		public static readonly BuiltInSigType Boolean = new BuiltInSigType(@"System.Boolean", CilElementType.Boolean);

		public static readonly BuiltInSigType Char = new BuiltInSigType(@"System.Char", CilElementType.Char);

		public static readonly BuiltInSigType SByte = new BuiltInSigType(@"System.SByte", CilElementType.I1);

		public static readonly BuiltInSigType Byte = new BuiltInSigType(@"System.Byte", CilElementType.U1);

		public static readonly BuiltInSigType Int16 = new BuiltInSigType(@"System.Int16", CilElementType.I2);

		public static readonly BuiltInSigType UInt16 = new BuiltInSigType(@"System.UInt16", CilElementType.U2);

		public static readonly BuiltInSigType Int32 = new BuiltInSigType(@"System.Int32", CilElementType.I4);

		public static readonly BuiltInSigType UInt32 = new BuiltInSigType(@"System.UInt32", CilElementType.U4);

		public static readonly BuiltInSigType Int64 = new BuiltInSigType(@"System.Int64", CilElementType.I8);

		public static readonly BuiltInSigType UInt64 = new BuiltInSigType(@"System.UInt64", CilElementType.U8);

		public static readonly BuiltInSigType Single = new BuiltInSigType(@"System.Single", CilElementType.R4);

		public static readonly BuiltInSigType Double = new BuiltInSigType(@"System.Double", CilElementType.R8);

		public static readonly BuiltInSigType String = new BuiltInSigType(@"System.String", CilElementType.String);

		public static readonly BuiltInSigType Object = new BuiltInSigType(@"System.Object", CilElementType.Object);

		public static readonly BuiltInSigType IntPtr = new BuiltInSigType(@"System.IntPtr", CilElementType.I);

		public static readonly BuiltInSigType UIntPtr = new BuiltInSigType(@"System.UIntPtr", CilElementType.U);

		public static readonly BuiltInSigType TypedByRef = new BuiltInSigType(@"System.TypedByRef", CilElementType.TypedByRef);

		public static readonly BuiltInSigType Ptr = new BuiltInSigType(@"System.Ptr", CilElementType.Ptr);

		private readonly string typeName;

		public BuiltInSigType(string typeName, CilElementType type) :
			base(type)
		{
			this.typeName = typeName;
		}

		public string TypeName { get { return this.typeName; } }
	}
}