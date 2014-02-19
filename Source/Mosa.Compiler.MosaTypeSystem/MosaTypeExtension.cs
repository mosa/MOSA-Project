/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

namespace Mosa.Compiler.MosaTypeSystem
{
	public static class MosaTypeExtension
	{
		public static MosaType ToManagedPointer(this MosaType type)
		{
			MosaType result = type.TypeSystem.Controller.CreateType();
			using (var ptrType = type.TypeSystem.Controller.MutateType(result))
			{
				ptrType.Module = type.Module;
				ptrType.Namespace = type.Namespace;
				ptrType.Name = type.Name;

				ptrType.ElementType = type;
				ptrType.TypeCode = MosaTypeCode.ManagedPointer;

				return result;
			}
		}

		public static MosaType ToUnmanagedPointer(this MosaType type)
		{
			MosaType result = type.TypeSystem.Controller.CreateType();
			using (var ptrType = type.TypeSystem.Controller.MutateType(result))
			{
				ptrType.Module = type.Module;
				ptrType.Namespace = type.Namespace;
				ptrType.Name = type.Name;

				ptrType.ElementType = type;
				ptrType.TypeCode = MosaTypeCode.UnmanagedPointer;

				return result;
			}
		}

		public static MosaType ToSZArray(this MosaType type)
		{
			MosaType array = type.TypeSystem.GetTypeByName(type.TypeSystem.CorLib, "System", "Array");
			MosaType result = type.TypeSystem.Controller.CreateType(array);
			using (var arrayType = type.TypeSystem.Controller.MutateType(result))
			{
				// See Partition II 14.2 Arrays

				arrayType.Module = type.Module;
				arrayType.Namespace = type.Namespace;
				arrayType.Name = type.Name;

				arrayType.Fields.Clear();
				arrayType.Methods.Clear();

				arrayType.BaseType = array;
				arrayType.ElementType = type;
				arrayType.TypeCode = MosaTypeCode.SZArray;
				arrayType.ArrayInfo = MosaArrayInfo.Vector;

				// Add three array accessors as defined in standard (Get, Set, Address)

				// TODO: Add them

				return result;
			}
		}

		public static MosaType ToArray(this MosaType type, MosaArrayInfo info)
		{
			MosaType result = type.ToSZArray();
			using (var arrayType = type.TypeSystem.Controller.MutateType(result))
			{
				arrayType.TypeCode = MosaTypeCode.Array;
				arrayType.ArrayInfo = info;
				return result;
			}
		}

		public static MosaType ToFnPtr(this TypeSystem typeSystem, MosaMethodSignature signature)
		{
			MosaType result = typeSystem.Controller.CreateType();
			using (var ptrType = typeSystem.Controller.MutateType(result))
			{
				ptrType.Module = typeSystem.LinkerModule;
				ptrType.Namespace = "";
				ptrType.Name = "";

				ptrType.TypeCode = MosaTypeCode.FunctionPointer;
				ptrType.FunctionPtrSig = signature;

				return result;
			}
		}

		public static int? GetPrimitiveSize(this MosaType type, int ptrSize)
		{
			if (type.IsPointer || type.IsN)
				return ptrSize;
			else if (type.IsUI1 || type.IsBoolean)
				return 1;
			else if (type.IsUI2 || type.IsChar)
				return 2;
			else if (type.IsUI4 || type.IsR4)
				return 4;
			else if (type.IsUI8 || type.IsR8)
				return 8;
			else
				return null;
		}
	}
}
