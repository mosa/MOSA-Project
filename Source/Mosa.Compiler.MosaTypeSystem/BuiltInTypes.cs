/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class BuiltInTypes
	{
		public MosaType Void { get; private set; }

		public MosaType Boolean { get; private set; }

		public MosaType Char { get; private set; }

		public MosaType I1 { get; private set; }

		public MosaType U1 { get; private set; }

		public MosaType I2 { get; private set; }

		public MosaType U2 { get; private set; }

		public MosaType I4 { get; private set; }

		public MosaType U4 { get; private set; }

		public MosaType I8 { get; private set; }

		public MosaType U8 { get; private set; }

		public MosaType R4 { get; private set; }

		public MosaType R8 { get; private set; }

		public MosaType String { get; private set; }

		public MosaType Object { get; private set; }

		public MosaType I { get; private set; }

		public MosaType U { get; private set; }

		public MosaType TypedRef { get; private set; }

		public MosaType Pointer { get; private set; }

		public BuiltInTypes(TypeSystem typeSystem, MosaModule corlib)
		{
			Void = corlib.Types[Tuple.Create("System", "System.Void")];
			Boolean = corlib.Types[Tuple.Create("System", "System.Boolean")];
			Char = corlib.Types[Tuple.Create("System", "System.Char")];
			I1 = corlib.Types[Tuple.Create("System", "System.SByte")];
			U1 = corlib.Types[Tuple.Create("System", "System.Byte")];
			I2 = corlib.Types[Tuple.Create("System", "System.Int16")];
			U2 = corlib.Types[Tuple.Create("System", "System.UInt16")];
			I4 = corlib.Types[Tuple.Create("System", "System.Int32")];
			U4 = corlib.Types[Tuple.Create("System", "System.UInt32")];
			I8 = corlib.Types[Tuple.Create("System", "System.Int64")];
			U8 = corlib.Types[Tuple.Create("System", "System.UInt64")];
			R4 = corlib.Types[Tuple.Create("System", "System.Single")];
			R8 = corlib.Types[Tuple.Create("System", "System.Double")];
			String = corlib.Types[Tuple.Create("System", "System.String")];
			Object = corlib.Types[Tuple.Create("System", "System.Object")];
			I = corlib.Types[Tuple.Create("System", "System.IntPtr")];
			U = corlib.Types[Tuple.Create("System", "System.UIntPtr")];
			TypedRef = corlib.Types[Tuple.Create("System", "System.TypedReference")];
			Pointer = typeSystem.GetUnmanagedPointerType(Void);
		}
	}
}