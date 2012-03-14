/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// 
	/// </summary>
	public interface IPlugSystem
	{
		/// <summary>
		/// Gets the plug.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		RuntimeMethod GetPlugMethod(RuntimeMethod method);
	}
}
