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
	///	E.g. an instantiation of Foo&lt;T&gt; with "int" for T will replace all occurrences of T
	///	inside Foo&lt;T&gt; with "int" in each member and each method's instruction stream.
	/// </summary>
	public class GenericTypePatcher : IGenericTypePatcher
	{
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
		/// Patches the type of the generic.
		/// </summary>
		/// <param name="typeToPatch">The type to patch.</param>
		/// <param name="sigtypes">The sigtypes.</param>
		/// <returns></returns>
		CilGenericType IGenericTypePatcher.PatchGenericType(CilGenericType typeToPatch, params SigType[] sigtypes)
		{
			GenericInstSigType newSigType = new GenericInstSigType (null, typeToPatch.GenericArguments);
			CilGenericType patchedType = typeToPatch; // new CilGenericType(typeToPatch.Module, typeToPatch.Token, typeToPatch.BaseGenericType, newSigType);

			this.PatchFields(patchedType);
			this.PatchMethods(patchedType);
			this.PatchTypeModule(patchedType);

			return patchedType;
		}

		/// <summary>
		/// Patches the fields.
		/// </summary>
		/// <param name="patchedType">Type of the patched.</param>
		private void PatchFields(CilGenericType patchedType)
		{
			var genericArguments = patchedType.GenericArguments;
			//TODO: Patch members by replacing their signatures with the actual set generic type parameter
			for (int i = 0; i < patchedType.Fields.Count; ++i)
			{
				var field = patchedType.Fields[i];
				var signatureType = field.SignatureType as GenericInstSigType;

				patchedType.Fields[i] = field;
			}
		}

		/// <summary>
		/// Patches the field.
		/// </summary>
		/// <param name="fieldToPatch">The field to patch.</param>
		/// <returns></returns>
		private RuntimeField PatchField(RuntimeField fieldToPatch)
		{
			var reader = new SignatureReader(null);
			var signature = new FieldSignature(reader, null);
			return new CilRuntimeField(fieldToPatch.Module, fieldToPatch.Name, signature, fieldToPatch.Token, 0, 
				fieldToPatch.RVA, fieldToPatch.DeclaringType, fieldToPatch.Attributes);
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
				if (instructionStream == null)
					continue;
			}
		}

		/// <summary>
		/// Patches the type module.
		/// </summary>
		/// <param name="patchedType">Type of the patched.</param>
		private void PatchTypeModule(CilGenericType patchedType)
		{
			//Remove the following line when GenericTypePatcher is fully implemented
			//(this.typeSystem.InternalTypeModule as InternalTypeModule).AddType(patchedType);
		}
	}
}
