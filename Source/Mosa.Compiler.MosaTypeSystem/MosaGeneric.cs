using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public static class MosaGeneric
	{
		public static MosaType ResolveGenericType(MosaType type, MosaType genericBaseType, List<MosaType> genericTypes)
		{
			var clone = new MosaType(type.Assembly);

			clone.GenericBaseType = genericBaseType;
			clone.GenericParameterTypes = genericTypes;
			clone.Name = type.Name;
			clone.FullName = type.FullName;
			clone.Namespace = type.Namespace;
			clone.BaseType = type.BaseType;
			clone.EnclosingType = type.EnclosingType;
			clone.IsUnsignedByte = type.IsUnsignedByte;
			clone.IsSignedByte = type.IsSignedByte;
			clone.IsUnsignedShort = type.IsUnsignedShort;
			clone.IsSignedShort = type.IsSignedShort;
			clone.IsUnsignedInt = type.IsUnsignedInt;
			clone.IsSignedInt = type.IsSignedInt;
			clone.IsUnsignedLong = type.IsUnsignedLong;
			clone.IsSignedLong = type.IsSignedLong;
			clone.IsChar = type.IsChar;
			clone.IsBoolean = type.IsBoolean;
			clone.IsPointer = type.IsPointer;
			clone.IsObject = type.IsObject;
			clone.IsDouble = type.IsDouble;
			clone.IsSingle = type.IsSingle;
			clone.IsInteger = type.IsInteger;
			clone.IsSigned = type.IsSigned;
			clone.IsUnsigned = type.IsUnsigned;
			clone.IsVarFlag = type.IsVarFlag;
			clone.IsMVarFlag = type.IsMVarFlag;
			clone.IsManagedPointerType = type.IsManagedPointerType;
			clone.IsUnmanagedPointerType = type.IsUnmanagedPointerType;
			clone.IsArrayType = type.IsArrayType;
			clone.IsBuiltInType = type.IsBuiltInType;
			clone.Size = type.Size;
			clone.PackingSize = type.PackingSize;
			clone.VarOrMVarIndex = type.VarOrMVarIndex;
			clone.ElementType = type.ElementType;

			var genericTypeNames = new StringBuilder();

			foreach (var genericType in genericTypes)
			{
				genericTypeNames.Append(genericType.FullName);
				genericTypeNames.Append(", ");
			}

			genericTypeNames.Length = genericTypeNames.Length - 2;
			clone.FullName = clone.FullName + '<' + genericTypeNames.ToString() + '>';

			foreach (var m in type.Methods)
			{
				var cloneMethod = MosaGeneric.ResolveGenericMethod(m, clone);
				clone.Methods.Add(cloneMethod);
			}

			foreach (var f in type.Fields)
			{
				var cloneField = MosaGeneric.ResolveGenericField(f, clone);
				clone.Fields.Add(cloneField);
			}

			foreach (var m in type.Interfaces)
			{
				clone.Interfaces.Add(m);
			}

			clone.SetFlags();

			return clone;
		}

		private static MosaMethod ResolveGenericMethod(MosaMethod method, MosaType declaringType)
		{
			var clone = new MosaMethod();

			clone.DeclaringType = declaringType;
			clone.Name = method.Name;
			clone.MethodName = method.MethodName;
			clone.IsAbstract = method.IsAbstract;
			clone.IsGeneric = method.IsGeneric;
			clone.IsStatic = method.IsStatic;
			clone.HasThis = method.HasThis;
			clone.HasExplicitThis = method.HasExplicitThis;
			clone.ReturnType = method.ReturnType;
			clone.IsInternal = method.IsInternal;
			clone.IsNoInlining = method.IsNoInlining;
			clone.IsSpecialName = method.IsSpecialName;
			clone.IsRTSpecialName = method.IsRTSpecialName;
			clone.IsVirtual = method.IsVirtual;
			clone.IsPInvokeImpl = method.IsPInvokeImpl;
			clone.IsNewSlot = method.IsNewSlot;
			clone.IsFinal = method.IsFinal;
			clone.Rva = method.Rva;

			foreach (var p in method.Parameters)
			{
				var cloneParameter = ResolveGenericParameter(p, declaringType);
				clone.Parameters.Add(cloneParameter);
			}

			foreach (var a in method.CustomAttributes)
			{
				clone.CustomAttributes.Add(a);
			}

			if (method.ReturnType.IsVarFlag || method.ReturnType.IsMVarFlag)
			{
				clone.ReturnType = declaringType.GenericParameterTypes[method.ReturnType.VarOrMVarIndex];
			}

			clone.SetName();

			return clone;
		}

		private static MosaField ResolveGenericField(MosaField field, MosaType declaringType)
		{
			var clone = new MosaField();

			clone.DeclaringType = declaringType;
			clone.FieldType = field.FieldType;
			clone.Name = field.Name;
			clone.FullName = field.FullName;
			clone.CustomAttributes = field.CustomAttributes;
			clone.IsLiteralField = field.IsLiteralField;
			clone.IsStaticField = field.IsStaticField;
			clone.RVA = clone.RVA;

			if (field.FieldType.IsVarFlag || field.FieldType.IsMVarFlag)
			{
				clone.FieldType = declaringType.GenericParameterTypes[field.FieldType.VarOrMVarIndex];
			}

			return clone;
		}

		private static MosaParameter ResolveGenericParameter(MosaParameter parameter, MosaType declaringType)
		{
			if (!(parameter.Type.IsVarFlag || parameter.Type.IsMVarFlag))
				return parameter;

			var clone = new MosaParameter();

			clone.Type = declaringType.GenericParameterTypes[parameter.Type.VarOrMVarIndex];
			clone.Position = parameter.Position;
			clone.Name = parameter.Name;
			clone.IsIn = parameter.IsIn;
			clone.IsOut = parameter.IsOut;

			return clone;
		}
	}
}