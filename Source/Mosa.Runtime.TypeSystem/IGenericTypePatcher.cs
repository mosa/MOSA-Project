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
	/// 
	/// </summary>
	public interface IGenericTypePatcher
	{
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
	}
}
