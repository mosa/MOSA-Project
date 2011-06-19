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
	///	Patches a generic type with the actual set of generic type parameters used in an instantiation.
	///	E.g. an instantiation of Foo<T> with "int" for T will replace all occurences of T
	///	inside Foo<T> with "int" in each member and each method's instruction stream.
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
			//TODO: Patch members by replacing their signatures with the actual set generic type parameter
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
