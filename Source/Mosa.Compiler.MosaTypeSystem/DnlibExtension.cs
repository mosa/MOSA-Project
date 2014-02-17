/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using dnlib.DotNet;

namespace Mosa.Compiler.MosaTypeSystem
{
	static class DnlibExtension
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
				TypeSpec spec = signature.TryGetTypeSpec();
				if (spec != null)
					signature = spec.TypeSig;
			}

			return signature;
		}

		public static MethodDef ResolveMethod(this IMethodDefOrRef method)
		{
			MethodDef result = method as MethodDef;
			if (result != null)
				return result;

			return ((MemberRef)method).ResolveMethodThrow();
		}
	}
}
