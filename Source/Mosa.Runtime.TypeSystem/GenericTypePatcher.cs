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
	public class GenericTypePatcher
	{
		/// <summary>
		/// Patches the type of the generic.
		/// </summary>
		/// <param name="typeToPatch">The type to patch.</param>
		/// <param name="sigtypes">The sigtypes.</param>
		/// <returns></returns>
		public CilGenericType PatchGenericType(CilGenericType typeToPatch, params SigType[] sigtypes)
		{
			GenericInstSigType newSigType = new GenericInstSigType (null, typeToPatch.GenericArguments);
			CilGenericType patchedType = new CilGenericType(typeToPatch.Module, typeToPatch.Token, typeToPatch.BaseGenericType, newSigType);

			this.PatchFields(patchedType);
			this.PatchMethods(patchedType);

			return patchedType;
		}

		/// <summary>
		/// Patches the fields.
		/// </summary>
		/// <param name="patchedType">Type of the patched.</param>
		private void PatchFields(CilGenericType patchedType)
		{
			//TODO: Patch members
		}

		/// <summary>
		/// Patches the methods.
		/// </summary>
		/// <param name="patchedType">Type of the patched.</param>
		private void PatchMethods(CilGenericType patchedType)
		{
			//TODO: Need to access the method's instruction stream, patch it,
			//TODO: and store it in the internal type module
			foreach (RuntimeMethod method in patchedType.Methods)
			{
				var instructionStream = method.InstructionStream;
			}
		}
	}
}
