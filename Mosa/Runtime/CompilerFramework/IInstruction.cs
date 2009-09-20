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

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Interface to an instruction
	/// </summary>
	public interface IInstruction
	{
		/// <summary>
		/// Gets the flow control.
		/// </summary>
		/// <value>The flow control.</value>
		FlowControl FlowControl { get; }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		void Visit(IVisitor vistor, Context context);
	}
}
