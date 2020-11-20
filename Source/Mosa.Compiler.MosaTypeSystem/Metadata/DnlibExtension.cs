// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem.Metadata
{
	internal static class DnlibExtension
	{
		public static ITypeDefOrRef GetElementType(this TypeSig signature)
		{
			return signature.GetNonNestedTypeRefScope();
		}

		public static TypeSig GetElementSig(this TypeSig signature)
		{
			while (signature.Next != null)
			{
				signature = signature.Next;
				var spec = signature.TryGetTypeSpec();
				if (spec != null)
					signature = spec.TypeSig;
			}

			return signature;
		}

		public static MethodDef ResolveMethod(this IMethodDefOrRef method)
		{
			if (method is MethodDef result)

				return result;

			return ((MemberRef)method).ResolveMethodThrow();
		}

		public static bool HasOpenGenericParameter(this TypeSig signature)
		{
			if (signature.IsGenericParameter)
				return true;

			if (signature is ModifierSig)
			{
				var modifier = ((ModifierSig)signature).Modifier as TypeSpec;

				if (modifier != null && HasOpenGenericParameter(modifier.TypeSig))
					return true;
			}

			if (signature is NonLeafSig)
			{
				return HasOpenGenericParameter(signature.Next);
			}
			else if (signature is TypeDefOrRefSig)
			{
				var type = ((TypeDefOrRefSig)signature).TypeDefOrRef as TypeSpec;

				if (type != null && HasOpenGenericParameter(type.TypeSig))
					return true;
				else
					return ((TypeDefOrRefSig)signature).TypeDefOrRef.ResolveTypeDef().HasGenericParameters;
			}
			else if (signature is GenericInstSig)
			{
				var genericInst = (GenericInstSig)signature;

				foreach (var genericArg in genericInst.GenericArguments)
				{
					if (HasOpenGenericParameter(genericArg))
						return true;
				}
				var genericType = genericInst.GenericType.TypeDefOrRef as TypeSpec;

				if (genericType != null && HasOpenGenericParameter(genericType.TypeSig))
					return true;
			}

			return false;
		}

		public static bool HasOpenGenericParameter(this MethodSig signature)
		{
			if (signature.GenParamCount > 0)
				return true;

			foreach (var param in signature.Params)
			{
				if (HasOpenGenericParameter(param))
					return true;
			}
			return HasOpenGenericParameter(signature.RetType);
		}

		public static bool HasModifierOrPinned(this TypeSig signature)
		{
			return signature is ModifierSig || signature is PinnedSig;
		}

		public static TypeSig GetTypeSig(this MosaType type)
		{
			return type.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Signature;
		}

		public static MethodSig GetMethodSig(this MosaMethod method)
		{
			return method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>().Signature;
		}

		public static FieldSig GetFieldSig(this MosaField field)
		{
			return field.GetUnderlyingObject<UnitDesc<FieldDef, FieldSig>>().Signature;
		}

		public static PropertySig GetPropertySig(this MosaProperty property)
		{
			return property.GetUnderlyingObject<UnitDesc<PropertyDef, PropertySig>>().Signature;
		}

		public static IList<TypeSig> GetGenericArguments(this IReadOnlyList<MosaType> types)
		{
			var result = new List<TypeSig>();

			foreach (var type in types)
			{
				result.Add(type.GetTypeSig());
			}

			return result;
		}

		public static IList<TypeSig> GetGenericArguments(this IList<MosaType> types)
		{
			var result = new List<TypeSig>();

			foreach (var type in types)
			{
				result.Add(type.GetTypeSig());
			}

			return result;
		}
	}
}
