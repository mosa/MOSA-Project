/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem.Generic;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// Patches a generic type with the actual set of generic type parameters used in an instantiation.
	/// E.g. an instantiation of Foo&lt;T&gt; with "int" for T will replace all occurrences of T
	/// inside Foo&lt;T&gt; with "int" in each member and each method's instruction stream.
	/// </summary>
	public sealed class GenericTypePatcher : IGenericTypePatcher
	{
		/// <summary>
		/// 
		/// </summary>
		private uint typeTokenCounter = 0;
		/// <summary>
		/// 
		/// </summary>
		private uint signatureTokenCounter = 0;
		/// <summary>
		/// 
		/// </summary>
		private ITypeSystem typeSystem;
		/// <summary>
		/// 
		/// </summary>
		private static Dictionary<string, Dictionary<long, CilGenericType>> typeDictionary = new Dictionary<string, Dictionary<long, CilGenericType>>();

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericTypePatcher"/> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		public GenericTypePatcher(ITypeSystem typeSystem)
		{
			this.typeSystem = typeSystem;
		}

		/// <summary>
		/// Adds the type.
		/// </summary>
		/// <param name="patchedType">The type.</param>
		/// <param name="signature">The signature.</param>
		private void AddType(CilGenericType patchedType, SigType[] signature)
		{
			(typeSystem.InternalTypeModule as InternalTypeModule).AddType(patchedType);

			long signatureHash = ComputeSignatureHash(signature);

			if (!typeDictionary.ContainsKey(patchedType.BaseGenericType.FullName))
			{
				typeDictionary[patchedType.BaseGenericType.FullName] = new Dictionary<long, CilGenericType>();
			}

			typeDictionary[patchedType.BaseGenericType.FullName][signatureHash] = patchedType;
		}

		private CilGenericType GetType(CilGenericType type, SigType[] signature)
		{
			// TODO: Look up in typeSpecs first!

			// FIXME: Do not use a hash based lookup without handling of collision
			long signatureHash = ComputeSignatureHash(signature);

			if (typeDictionary.ContainsKey(type.FullName) && typeDictionary[type.FullName].ContainsKey(signatureHash))
			{
				return typeDictionary[type.FullName][signatureHash];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Patches the type.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		RuntimeType IGenericTypePatcher.PatchType(ITypeModule typeModule, CilGenericType enclosingType, CilGenericType openType)
		{
			var genericArguments = CloseGenericArguments(enclosingType, openType);
			var patchedType = GetType(openType, genericArguments);

			if (patchedType == null)
			{
				var typeToken = new Token(0xFE000000 | ++typeTokenCounter);
				var signatureToken = new Token(0xFD000000 | ++signatureTokenCounter);
				var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
				var signature = new GenericInstSigType(sigtype, genericArguments);

				patchedType = new CilGenericType(enclosingType.InstantiationModule, typeToken, openType.BaseGenericType, signature);
				AddType(patchedType, genericArguments);
			}

			return patchedType;
		}

		/// <summary>
		/// Patches the method.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openMethod">The open method.</param>
		/// <returns></returns>
		RuntimeMethod IGenericTypePatcher.PatchMethod(ITypeModule typeModule, CilGenericType enclosingType, RuntimeMethod openMethod)
		{
			var openType = openMethod.DeclaringType as CilGenericType;
			var genericArguments = CloseGenericArguments(enclosingType, openType);

			var patchedType = GetType(openType, genericArguments);
			if (patchedType == null)
			{
				var typeToken = new Token(0xFE000000 | ++typeTokenCounter);
				var signatureToken = new Token(0xFD000000 | ++signatureTokenCounter);
				var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
				var signature = new GenericInstSigType(sigtype, genericArguments);
				
				patchedType = new CilGenericType(enclosingType.InstantiationModule, typeToken, openType.BaseGenericType, signature);
				AddType(patchedType, genericArguments);
			}

			var methodIndex = GetMethodIndex(openMethod);
			return patchedType.Methods[methodIndex];
		}

		/// <summary>
		/// Patches the field.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="enclosingType">Type of the closed.</param>
		/// <param name="openField">The open field.</param>
		/// <returns></returns>
		RuntimeField IGenericTypePatcher.PatchField(ITypeModule typeModule, CilGenericType enclosingType, RuntimeField openField)
		{
			var openType = openField.DeclaringType as CilGenericType;
			var genericArguments = CloseGenericArguments(enclosingType, openType);
			var patchedType = GetType(openType, genericArguments);

			if (patchedType == null)
			{
				var typeToken = new Token(0xFE000000 | ++typeTokenCounter);
				var signatureToken = new Token(0xFD000000 | ++signatureTokenCounter);
				var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
				var signature = new GenericInstSigType(sigtype, genericArguments);

				// FIXME: There has got to be a better way to do this...
				try
				{
					patchedType = new CilGenericType(enclosingType.Module, typeToken, openType.BaseGenericType, signature);
				}
				catch (Exception)
				{
					foreach (var module in typeModule.TypeSystem.TypeModules)
					{
						try
						{
							patchedType = new CilGenericType(module, typeToken, openType.BaseGenericType, signature);
							break;
						}
						catch (Exception)
						{
							;
						}
					}
				}

				AddType(patchedType, genericArguments);
			}


			foreach (var field in patchedType.Fields)
			{
				if (field.Name == openField.Name)
				{
					return field;
				}
			}

			throw new MissingFieldException();
		}

		private RuntimeField GetFieldByName(string name, RuntimeType type)
		{
			foreach (var field in type.Fields)
			{
				if (field.Name == name)
				{
					return field;
				}
			}
			return null;
		}

		/// <summary>
		/// Computes the signature hash.
		/// </summary>
		/// <param name="signature">The signature.</param>
		/// <returns></returns>
		private long ComputeSignatureHash(SigType[] signature)
		{
			// FIXME: There might be collisions!
			var result = 0;

			foreach (SigType sig in signature)
			{
				if (sig is ClassSigType)
				{
					result += (sig as ClassSigType).Token.ToInt32();
				}
				else
				{
					result += sig.GetHashCode();
				}
			}
			return result;
		}

		/// <summary>
		/// Patches the type of the signature.
		/// </summary>
		/// <param name="typemodule"></param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		SigType IGenericTypePatcher.PatchSignatureType(ITypeModule typemodule, RuntimeType enclosingType, Token token)
		{
			if (typemodule.MetadataModule == null)
				return new ClassSigType(token);

			if (token.Table == TableType.TypeSpec)
			{
				var typespecRow = typemodule.MetadataModule.Metadata.ReadTypeSpecRow(token);
				var signature = new TypeSpecSignature(typemodule.MetadataModule.Metadata, typespecRow.SignatureBlobIdx);

				if (enclosingType is CilGenericType)
				{
					var enclosingGenericType = enclosingType as CilGenericType;
					if (signature.Type is VarSigType)
					{
						return enclosingGenericType.GenericArguments[(signature.Type as VarSigType).Index];
					}
					else if (signature.Type is GenericInstSigType)
					{
						var openGenericSigType = (signature.Type as GenericInstSigType);
						if (openGenericSigType.ContainsGenericParameters)
						{
							return new GenericInstSigType(openGenericSigType.BaseType, this.CloseGenericArguments(enclosingGenericType, openGenericSigType));
						}
					}
				}

				return signature.Type;
			}
			return new ClassSigType(token);
		}

		/// <summary>
		/// Closes the generic arguments.
		/// </summary>
		/// <param name="enclosingType">Type of the closed.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		private SigType[] CloseGenericArguments(CilGenericType enclosingType, CilGenericType openType)
		{
			return CloseGenericArguments(enclosingType.GenericArguments, openType.GenericArguments);
		}

		/// <summary>
		/// Closes the generic arguments.
		/// </summary>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		private SigType[] CloseGenericArguments(CilGenericType enclosingType, GenericInstSigType openType)
		{
			return CloseGenericArguments(enclosingType.GenericArguments, openType.GenericArguments);
		}

		/// <summary>
		/// Closes the generic arguments.
		/// </summary>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		private SigType[] CloseGenericArguments(SigType[] enclosingType, SigType[] openType)
		{
			var result = new SigType[openType.Length];

			for (var i = 0; i < openType.Length; ++i)
			{
				if (openType[i] is VarSigType)
				{
					result[i] = enclosingType[(openType[i] as VarSigType).Index];
				}
			}

			return result;
		}

		/// <summary>
		/// Closes the generic arguments.
		/// </summary>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		SigType[] IGenericTypePatcher.CloseGenericArguments(SigType[] enclosingType, SigType[] openType)
		{
			return CloseGenericArguments(enclosingType, openType);
		}

		private int GetMethodIndex(RuntimeMethod method)
		{
			var openType = method.DeclaringType as CilGenericType;

			for (int i = 0; i < openType.Methods.Count; ++i)
			{
				if (method.Name == openType.Methods[i].Name && method.Signature.Matches(openType.Methods[i].Signature))
				{
					return i;
				}
			}
			throw new MissingMethodException();
		}
	}
}
