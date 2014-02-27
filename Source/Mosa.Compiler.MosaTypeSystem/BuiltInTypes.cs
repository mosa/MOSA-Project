/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
			Void = typeSystem.GetTypeByName(corlib, "System", "Void");
			Boolean = typeSystem.GetTypeByName(corlib, "System", "Boolean");
			Char = typeSystem.GetTypeByName(corlib, "System", "Char");
			I1 = typeSystem.GetTypeByName(corlib, "System", "SByte");
			U1 = typeSystem.GetTypeByName(corlib, "System", "Byte");
			I2 = typeSystem.GetTypeByName(corlib, "System", "Int16");
			U2 = typeSystem.GetTypeByName(corlib, "System", "UInt16");
			I4 = typeSystem.GetTypeByName(corlib, "System", "Int32");
			U4 = typeSystem.GetTypeByName(corlib, "System", "UInt32");
			I8 = typeSystem.GetTypeByName(corlib, "System", "Int64");
			U8 = typeSystem.GetTypeByName(corlib, "System", "UInt64");
			R4 = typeSystem.GetTypeByName(corlib, "System", "Single");
			R8 = typeSystem.GetTypeByName(corlib, "System", "Double");
			String = typeSystem.GetTypeByName(corlib, "System", "String");
			Object = typeSystem.GetTypeByName(corlib, "System", "Object");
			I = typeSystem.GetTypeByName(corlib, "System", "IntPtr");
			U = typeSystem.GetTypeByName(corlib, "System", "UIntPtr");
			TypedRef = typeSystem.GetTypeByName(corlib, "System", "TypedReference");
			Pointer = Void.ToUnmanagedPointer();
		}
	}
}