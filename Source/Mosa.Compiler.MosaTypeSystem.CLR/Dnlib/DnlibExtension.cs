// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using Mosa.Compiler.MosaTypeSystem.CLR.Utils;

namespace Mosa.Compiler.MosaTypeSystem.CLR.Dnlib;

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

		if (signature is ModifierSig { Modifier: TypeSpec modifier } && HasOpenGenericParameter(modifier.TypeSig))
			return true;

		switch (signature)
		{
			case NonLeafSig:
				{
					return HasOpenGenericParameter(signature.Next);
				}

			case TypeDefOrRefSig sig:
				{
					if (sig.TypeDefOrRef is TypeSpec type && HasOpenGenericParameter(type.TypeSig))
						return true;

					return sig.TypeDefOrRef.ResolveTypeDef().HasGenericParameters;
				}

			case GenericInstSig sig:
				{
					foreach (var genericArg in sig.GenericArguments)
					{
						if (HasOpenGenericParameter(genericArg))
							return true;
					}

					if (sig.GenericType.TypeDefOrRef is TypeSpec genericType && HasOpenGenericParameter(genericType.TypeSig))
						return true;

					break;
				}
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
		return signature is ModifierSig or PinnedSig;
	}

	public static TypeSig GetTypeSig(this MosaType type)
	{
		return type.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>()?.Signature ?? throw new InvalidOperationException("Type signature is null!");
	}

	public static MethodSig GetMethodSig(this MosaMethod method)
	{
		return method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>()?.Signature ?? throw new InvalidOperationException("Method signature is null!");
	}

	public static FieldSig GetFieldSig(this MosaField field)
	{
		return field.GetUnderlyingObject<UnitDesc<FieldDef, FieldSig>>()?.Signature ?? throw new InvalidOperationException("Field signature is null!");
	}

	public static PropertySig GetPropertySig(this MosaProperty property)
	{
		return property.GetUnderlyingObject<UnitDesc<PropertyDef, PropertySig>>()?.Signature ?? throw new InvalidOperationException("Property signature is null!");
	}

	public static IList<TypeSig> GetGenericArguments(this IReadOnlyList<MosaType> types)
	{
		var result = new List<TypeSig>();

		foreach (var type in types)
		{
			var typeSig = type.GetTypeSig();
			if (typeSig == null)
				throw new InvalidOperationException("Type signature of type is null!");

			result.Add(typeSig);
		}

		return result;
	}

	public static IList<TypeSig> GetGenericArguments(this IList<MosaType> types)
	{
		var result = new List<TypeSig>();

		foreach (var type in types)
		{
			var typeSig = type.GetTypeSig();
			if (typeSig == null)
				throw new InvalidOperationException("Type signature of type is null!");

			result.Add(typeSig);
		}

		return result;
	}
}
