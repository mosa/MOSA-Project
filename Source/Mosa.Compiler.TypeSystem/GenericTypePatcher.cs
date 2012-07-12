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


		private struct GenericEntry
		{
			public CilGenericType openType;
			public SigType[] genericArguments;
			public CilGenericType patchedType;

			public GenericEntry(CilGenericType openType, SigType[] genericArguments, CilGenericType patchedType)
			{
				this.openType = openType;
				this.genericArguments = genericArguments;
				this.patchedType = patchedType;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private List<GenericEntry> patchedTypes = new List<GenericEntry>();

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericTypePatcher"/> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		public GenericTypePatcher(ITypeSystem typeSystem)
		{
			this.typeSystem = typeSystem;
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <param name="openType">Type of the open.</param>
		/// <param name="genericArguments">The generic arguments.</param>
		/// <returns></returns>
		private CilGenericType GetPatchedType(CilGenericType openType, SigType[] genericArguments)
		{
			foreach (var genericEntry in patchedTypes)
			{
				if (genericEntry.openType == openType && CompareSignatures(genericEntry.genericArguments, genericArguments))
					return genericEntry.patchedType;
			}

			return null;
		}

		/// <summary>
		/// Compares the signatures.
		/// </summary>
		/// <param name="signatureA">The signature A.</param>
		/// <param name="signatureB">The signature B.</param>
		/// <returns></returns>
		private bool CompareSignatures(SigType[] signatureA, SigType[] signatureB)
		{
			if (signatureA.Length != signatureB.Length)
				return false;

			for (int i = 0; i < signatureA.Length; i++)
			{
				if (!signatureA[i].Equals(signatureB[i]))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Adds the type.
		/// </summary>
		/// <param name="patchedType">The type.</param>
		/// <param name="signature">The signature.</param>
		private void AddPatchedType(CilGenericType openType, SigType[] signature, CilGenericType patchedType)
		{
			(typeSystem.InternalTypeModule as InternalTypeModule).AddType(patchedType);
			
			GenericEntry genericEntry = new GenericEntry(openType, signature, patchedType);

			patchedTypes.Add(genericEntry);
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

			var patchedType = GetPatchedType(openType, genericArguments);
			if (patchedType == null)
			{
				var typeToken = new Token(0xFE000000 | ++typeTokenCounter);
				var signatureToken = new Token(0xFD000000 | ++signatureTokenCounter);
				var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
				var signature = new GenericInstSigType(sigtype, genericArguments);

				patchedType = new CilGenericType(enclosingType.InstantiationModule, typeToken, openType.BaseGenericType, signature);
				AddPatchedType(openType, genericArguments, patchedType);
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

			var patchedType = GetPatchedType(openType, genericArguments);
			if (patchedType == null)
			{
				var typeToken = new Token(0xFE000000 | ++typeTokenCounter);
				var signatureToken = new Token(0xFD000000 | ++signatureTokenCounter);
				var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
				var signature = new GenericInstSigType(sigtype, genericArguments);

				patchedType = new CilGenericType(enclosingType.InstantiationModule, typeToken, openType.BaseGenericType, signature);
				AddPatchedType(openType, genericArguments, patchedType);
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

			var patchedType = GetPatchedType(openType, genericArguments);
			if (patchedType == null)
			{
				var typeToken = new Token(0xFE000000 | ++typeTokenCounter);
				var signatureToken = new Token(0xFD000000 | ++signatureTokenCounter);
				var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
				var signature = new GenericInstSigType(sigtype, genericArguments);

				patchedType = new CilGenericType(enclosingType.InstantiationModule, typeToken, openType.BaseGenericType, signature);
				
				AddPatchedType(openType, genericArguments, patchedType);
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

		/// <summary>
		/// Gets the index of the method.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
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
