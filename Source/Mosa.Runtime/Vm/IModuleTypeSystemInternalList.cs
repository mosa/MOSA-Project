/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.Vm
{
	/// <summary>
	/// 
	/// </summary>
	internal interface IModuleTypeSystemInternalList
	{

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		RuntimeMethod[] Methods { get; }

		/// <summary>
		/// Holds all parameter information elements.
		/// </summary>
		RuntimeParameter[] Parameters { get; }

		/// <summary>
		/// Holds all loaded _stackFrameIndex definitions.
		/// </summary>
		RuntimeField[] Fields { get; }

	}
}
