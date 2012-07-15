/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	public class PlugSystem 
	{
		#region Data members

		protected Dictionary<RuntimeMethod, RuntimeMethod> plugMethods = new Dictionary<RuntimeMethod, RuntimeMethod>();

		#endregion // Data members

		/// <summary>
		/// Gets the plug.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		public RuntimeMethod GetPlugMethod(RuntimeMethod method)
		{
			RuntimeMethod plug = null;

			plugMethods.TryGetValue(method, out plug);

			return plug;
		}

		public void CreatePlug(RuntimeMethod plug, RuntimeMethod methodToPlug)
		{
			plugMethods.Add(methodToPlug, plug);
		}


	}
}

