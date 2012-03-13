/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
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
	/// This interface represents a stage of compilation of an assembly.
	/// </summary>
	public interface IPlugStage
	{
		/// <summary>
		/// Gets the plug.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		RuntimeMethod GetPlug(RuntimeMethod method);
	}
}
