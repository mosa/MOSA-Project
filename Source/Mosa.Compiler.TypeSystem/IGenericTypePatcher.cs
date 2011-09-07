/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */


using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem.Generic;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// 
	/// </summary>
	public interface IGenericTypePatcher
	{
		/// <summary>
		/// Patches the type.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		RuntimeType PatchType(ITypeModule typeModule, CilGenericType enclosingType, CilGenericType openType);
		/// <summary>
		/// Patches the field.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="closedType">Type of the closed.</param>
		/// <param name="openField">The open field.</param>
		/// <returns></returns>
		RuntimeField PatchField(ITypeModule typeModule, CilGenericType closedType, RuntimeField openField);
		/// <summary>
		/// Patches the method.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openMethod">The open method.</param>
		/// <returns></returns>
		RuntimeMethod PatchMethod(ITypeModule typeModule, CilGenericType enclosingType, RuntimeMethod openMethod);
		/// <summary>
		/// Patches the type of the signature.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		SigType PatchSignatureType(ITypeModule typemodule, RuntimeType enclosingType, Token token);
		/// <summary>
		/// Closes the generic arguments.
		/// </summary>
		/// <param name="enclosingType">Type of the enclosing.</param>
		/// <param name="openType">Type of the open.</param>
		/// <returns></returns>
		SigType[] CloseGenericArguments(SigType[] enclosingType, SigType[] openType);
	}
}
