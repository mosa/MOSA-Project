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
		private readonly ITypeSystem typeSystem;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericTypePatcher"/> class.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		public GenericTypePatcher(ITypeSystem typeSystem)
		{
			this.typeSystem = typeSystem;
		}

		/// <summary>
		/// Patches the field.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="enclosingType">Type of the closed.</param>
		/// <param name="openType">Type of the open.</param>
		/// <param name="openField">The open field.</param>
		/// <returns></returns>
		RuntimeField IGenericTypePatcher.PatchField(ITypeModule typeModule, CilGenericType enclosingType, RuntimeField openField)
		{
			var openType = openField.DeclaringType as CilGenericType;
			var genericParameters = enclosingType.GenericParameters;

			var typeToken = new Token(0xFE000000 | typeTokenCounter++);
			var signatureToken = new Token(0xFD000000 | signatureTokenCounter++);
			var sigtype = new TypeSigType(signatureToken, CilElementType.Var);
			var genericArguments = CloseGenericArguments(enclosingType, openType);

			var signature = new GenericInstSigType(sigtype, genericArguments);

			var patchedType = new CilGenericType(typeModule, typeToken, openType.BaseGenericType, signature);

			foreach (var field in patchedType.Fields)
			{
				if (field.Name == openField.Name)
					return field;
			}

			throw new MissingFieldException();
		}

		/// <summary>
		/// Closes the generic arguments.
		/// </summary>
		/// <param name="enclosingType">Type of the closed.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		private SigType[] CloseGenericArguments(CilGenericType enclosingType, CilGenericType openType)
		{
			var result = new SigType[openType.GenericArguments.Length];

			for (var i = 0; i < openType.GenericArguments.Length; ++i)
			{
				result[i] = enclosingType.GenericArguments[(openType.GenericArguments[i] as VarSigType).Index];
			}

			return result;
		}
	}
}
