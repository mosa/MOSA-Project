/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System.Collections.Generic;

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
				ptrType.DeclaringType = type;
				ptrType.Namespace = type.Namespace;
				ptrType.Name = type.Name;

				ptrType.HasOpenGenericParams = type.HasOpenGenericParams;
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
				ptrType.DeclaringType = type;
				ptrType.Namespace = type.Namespace;
				ptrType.Name = type.Name;

				ptrType.HasOpenGenericParams = type.HasOpenGenericParams;
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
				arrayType.DeclaringType = type.DeclaringType;
				arrayType.Namespace = type.Namespace;
				arrayType.Name = type.Name;

				arrayType.HasOpenGenericParams = type.HasOpenGenericParams;
				arrayType.BaseType = array;
				arrayType.ElementType = type;
				arrayType.TypeCode = MosaTypeCode.SZArray;
				arrayType.ArrayInfo = MosaArrayInfo.Vector;

				AddArrayMethods(type.TypeSystem, result, arrayType, MosaArrayInfo.Vector);

				return result;
			}
		}

		public static MosaType ToArray(this MosaType type, MosaArrayInfo info)
		{
			MosaType array = type.TypeSystem.GetTypeByName(type.TypeSystem.CorLib, "System", "Array");
			MosaType result = type.TypeSystem.Controller.CreateType(array);
			using (var arrayType = type.TypeSystem.Controller.MutateType(result))
			{
				// See Partition II 14.2 Arrays

				arrayType.Module = type.Module;
				arrayType.DeclaringType = type.DeclaringType;
				arrayType.Namespace = type.Namespace;
				arrayType.Name = type.Name;

				arrayType.HasOpenGenericParams = type.HasOpenGenericParams;
				arrayType.BaseType = array;
				arrayType.ElementType = type;
				arrayType.TypeCode = MosaTypeCode.Array;
				arrayType.ArrayInfo = info;

				AddArrayMethods(type.TypeSystem, result, arrayType, info);

				return result;
			}
		}

		static void AddArrayMethods(TypeSystem typeSystem, MosaType arrayType, MosaType.Mutator type, MosaArrayInfo info)
		{
			// Remove all methods & fields --> Since BaseType = System.Array, they're automatically inherited.
			type.Methods.Clear();
			type.Fields.Clear();

			// Add three array accessors as defined in standard (Get, Set, Address)
			// Also, constructor.

			uint rank = info.Rank;

			MosaMethod methodGet = typeSystem.Controller.CreateMethod();
			using (var method = typeSystem.Controller.MutateMethod(methodGet))
			{
				method.DeclaringType = arrayType;
				method.Name = "Get";
				method.IsInternalCall = true;
				method.IsRTSpecialName = method.IsSpecialName = true;
				method.IsFinal = true;
				method.HasThis = true;

				List<MosaParameter> parameters = new List<MosaParameter>();
				for (uint i = 0; i < rank; i++)
					parameters.Add(new MosaParameter("index" + i, typeSystem.BuiltIn.I4));
				method.Signature = new MosaMethodSignature(arrayType.ElementType, parameters);
			}
			type.Methods.Add(methodGet);

			MosaMethod methodSet = typeSystem.Controller.CreateMethod();
			using (var method = typeSystem.Controller.MutateMethod(methodSet))
			{
				method.DeclaringType = arrayType;
				method.Name = "Set";
				method.IsInternalCall = true;
				method.IsRTSpecialName = method.IsSpecialName = true;
				method.IsFinal = true;
				method.HasThis = true;

				List<MosaParameter> parameters = new List<MosaParameter>();
				for (uint i = 0; i < rank; i++)
					parameters.Add(new MosaParameter("index" + i, typeSystem.BuiltIn.I4));
				parameters.Add(new MosaParameter("value", arrayType.ElementType));
				method.Signature = new MosaMethodSignature(typeSystem.BuiltIn.Void, parameters);
			}
			type.Methods.Add(methodSet);

			MosaMethod methodAdrOf = typeSystem.Controller.CreateMethod();
			using (var method = typeSystem.Controller.MutateMethod(methodAdrOf))
			{
				method.DeclaringType = arrayType;
				method.Name = "AddressOr";
				method.IsInternalCall = true;
				method.IsRTSpecialName = method.IsSpecialName = true;
				method.IsFinal = true;
				method.HasThis = true;

				List<MosaParameter> parameters = new List<MosaParameter>();
				for (uint i = 0; i < rank; i++)
					parameters.Add(new MosaParameter("index" + i, typeSystem.BuiltIn.I4));
				method.Signature = new MosaMethodSignature(arrayType.ElementType.ToManagedPointer(), parameters);
			}
			type.Methods.Add(methodAdrOf);

			MosaMethod methodCtor = typeSystem.Controller.CreateMethod();
			using (var method = typeSystem.Controller.MutateMethod(methodCtor))
			{
				method.DeclaringType = arrayType;
				method.Name = ".ctor";
				method.IsInternalCall = true;
				method.IsRTSpecialName = method.IsSpecialName = true;
				method.IsFinal = true;
				method.HasThis = true;

				List<MosaParameter> parameters = new List<MosaParameter>();
				for (uint i = 0; i < rank; i++)
					parameters.Add(new MosaParameter("length" + i, typeSystem.BuiltIn.I4));
				method.Signature = new MosaMethodSignature(typeSystem.BuiltIn.Void, parameters);
			}
			type.Methods.Add(methodCtor);
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

		public static int? GetPrimitiveSize(this MosaType type, int nativeSize)
		{
			if (type.IsEnum)
				return type.GetEnumUnderlyingType().GetPrimitiveSize(nativeSize);

			if (type.IsPointer || type.IsN)
				return nativeSize;
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

		public static MosaType GetEnumUnderlyingType(this MosaType type)
		{
			if (!type.IsEnum)
				return type;

			foreach (var field in type.Fields)
			{
				if (!field.IsStatic)
					return field.FieldType;
			}
			return null;
		}

		public static MosaType RemoveModifiers(this MosaType type)
		{
			while (type.Modifier != null)
				type = type.ElementType;
			return type;
		}
	}
}
