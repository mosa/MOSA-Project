/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class Move
	{
		public Operand Source { get; set; }

		public Operand Destination { get; set; }

		public Move(Operand source, Operand destination)
		{
			Debug.Assert(source != null);
			Debug.Assert(destination != null);

			Source = source;
			Destination = destination;
		}
	}
}