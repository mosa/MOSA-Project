/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Provides a visitor interface for instructions.
	/// </summary>
	public interface IInstructionVisitor2
    {
		/// <summary>
		/// Visitation method for instructions not caught by more specific visitation methods.
		/// </summary>
		/// <param name="ctx">The context.</param>
        void Visit(Context ctx);
    }
}
