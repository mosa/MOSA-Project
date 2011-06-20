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
		/// Patches the type of the generic.
		/// </summary>
		/// <param name="typeToPatch">The type to patch.</param>
		/// <param name="sigtypes">The sigtypes.</param>
		/// <returns></returns>
		CilGenericType PatchGenericType(CilGenericType typeToPatch, params SigType[] sigtypes);
	}
}
