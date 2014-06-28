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
	public class MoveExtended<T>
	{
		public readonly Move Move;

		public readonly T Value;

		public MoveExtended(Move move, T value)
		{
			Debug.Assert(move != null);

			Move = move;
			Value = value;
		}
	}
}