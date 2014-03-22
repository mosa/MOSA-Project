/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	public class PlugSystem
	{
		#region Data members

		protected Dictionary<MosaMethod, MosaMethod> plugMethods = new Dictionary<MosaMethod, MosaMethod>();

		#endregion Data members

		/// <summary>
		/// Gets the plug.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		public MosaMethod GetPlugMethod(MosaMethod method)
		{
			MosaMethod plug = null;

			plugMethods.TryGetValue(method, out plug);

			return plug;
		}

		public void CreatePlug(MosaMethod plug, MosaMethod methodToPlug)
		{
			plugMethods.Add(methodToPlug, plug);
		}
	}
}