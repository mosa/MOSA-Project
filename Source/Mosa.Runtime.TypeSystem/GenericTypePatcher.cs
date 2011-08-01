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
using System.Linq;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem.Cil;
using Mosa.Runtime.TypeSystem.Generic;

namespace Mosa.Runtime.TypeSystem
{
	/// <summary>
	/// Patches a generic type with the actual set of generic type parameters used in an instantiation.
	/// E.g. an instantiation of Foo&lt;T&gt; with "int" for T will replace all occurrences of T
	/// inside Foo&lt;T&gt; with "int" in each member and each method's instruction stream.
	/// </summary>
	public class GenericTypePatcher : IGenericTypePatcher
	{
		/// <summary>
		/// 
		/// </summary>
		private static uint typeTokenCounter = 1u;
		/// <summary>
		/// 
		/// </summary>
		private static uint signatureTokenCounter = 1u;
		/// <summary>
		/// 
		/// </summary>
		private ITypeSystem typeSystem = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericTypePatcher"/> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		public GenericTypePatcher(ITypeSystem typeSystem)
		{
			this.typeSystem = typeSystem;
		}

		/// <summary>
		/// Patches the type.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		public RuntimeType PatchType(ITypeModule typeModule, CilGenericType enclosingType, CilGenericType openType)
		{
			var genericParameters = enclosingType.GenericParameters;

			var typeToken = new Token(0xFE000000 | typeTokenCounter++);
			var signatureToken = new Token(0xFD000000 | signatureTokenCounter++);
			var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
			var genericArguments = CloseGenericArguments(enclosingType, openType);

			var signature = new GenericInstSigType(sigtype, genericArguments);

			var type = new CilGenericType(openType.Module, typeToken, openType.BaseGenericType, signature);
			(this.typeSystem.InternalTypeModule as InternalTypeModule).AddType(type);

			return type;
		}

		/// <summary>
		/// Patches the field.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="enclosingType">Type of the closed.</param>
		/// <param name="openField">The open field.</param>
		/// <returns></returns>
		public RuntimeField PatchField(ITypeModule typeModule, CilGenericType enclosingType, RuntimeField openField)
		{
			var openType = openField.DeclaringType as CilGenericType;
			var genericParameters = enclosingType.GenericParameters;

			var typeToken = new Token(0xFE000000 | typeTokenCounter++);
			var signatureToken = new Token(0xFD000000 | signatureTokenCounter++);
			var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
			var genericArguments = CloseGenericArguments(enclosingType, openType);

			var signature = new GenericInstSigType(sigtype, genericArguments);

			var patchedType = new CilGenericType(openField.Module, typeToken, openType.BaseGenericType, signature);
			(this.typeSystem.InternalTypeModule as InternalTypeModule).AddType(patchedType);

			foreach (var field in patchedType.Fields)
			{
				if (field.Name == openField.Name)
					return field;
			}

			throw new MissingFieldException();
		}

		/// <summary>
		/// Patches the type of the signature.
		/// </summary>
		/// <param name="typemodule"></param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public SigType PatchSignatureType(ITypeModule typemodule, RuntimeType enclosingType, Token token)
		{
			if (token.Table == TableType.TypeSpec)
			{
				var typespecRow = typemodule.MetadataModule.Metadata.ReadTypeSpecRow(token);
				var signature = new TypeSpecSignature(typemodule.MetadataModule.Metadata, typespecRow.SignatureBlobIdx);

				if (enclosingType is CilGenericType)
				{
					var enclosingGenericType = enclosingType as CilGenericType;
					if (signature.Type is VarSigType)
						return enclosingGenericType.GenericArguments[(signature.Type as VarSigType).Index];
					else if (signature.Type is GenericInstSigType)
					{
						var openGenericSigType = (signature.Type as GenericInstSigType);
						return new GenericInstSigType(openGenericSigType.BaseType, this.CloseGenericArguments(enclosingGenericType, openGenericSigType));
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
			return this.CloseGenericArguments(enclosingType.GenericArguments, openType.GenericArguments);
		}

		/// <summary>
		/// Closes the generic arguments.
		/// </summary>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		private SigType[] CloseGenericArguments(CilGenericType enclosingType, GenericInstSigType openType)
		{
			return this.CloseGenericArguments(enclosingType.GenericArguments, openType.GenericArguments);
		}

		/// <summary>
		/// Closes the generic arguments.
		/// </summary>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		public SigType[] CloseGenericArguments(SigType[] enclosingType, SigType[] openType)
		{
			var result = new SigType[openType.Length];

			for (var i = 0; i < openType.Length; ++i)
			{
				if (openType[i] is VarSigType)
					result[i] = enclosingType[(openType[i] as VarSigType).Index];
			}

			return result;
		}

		/// <summary>
		/// Patches the method.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openMethod">The open method.</param>
		/// <returns></returns>
		public RuntimeMethod PatchMethod(ITypeModule typeModule, CilGenericType enclosingType, RuntimeMethod openMethod)
		{
			var openType = openMethod.DeclaringType as CilGenericType;
			var genericParameters = enclosingType.GenericParameters;

			var typeToken = new Token(0xFE000000 | typeTokenCounter++);
			var signatureToken = new Token(0xFD000000 | signatureTokenCounter++);
			var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
			var genericArguments = CloseGenericArguments(enclosingType, openType);

			var signature = new GenericInstSigType(sigtype, genericArguments);

			var patchedType = new CilGenericType(openMethod.Module, typeToken, openType.BaseGenericType, signature);
			(this.typeSystem.InternalTypeModule as InternalTypeModule).AddType(patchedType);

			foreach (var method in patchedType.Methods)
			{
				if (method.Name == method.Name)
					return method;
			}

			throw new MissingMethodException();
		}
	}
}
