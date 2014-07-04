/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
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
		public readonly Operand Source;

		public readonly Operand Destination;

		public Move(Operand source, Operand destination)
		{
			Debug.Assert(source != null);
			Debug.Assert(destination != null);

			Source = source;
			Destination = destination;
		}
	}
}