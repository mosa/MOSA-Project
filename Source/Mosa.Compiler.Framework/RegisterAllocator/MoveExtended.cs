/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class MoveExtended<T>
	{
		public readonly T Value;

		public readonly Operand Source;

		public readonly Operand Destination;

		public MoveExtended(Operand source, Operand destination, T value)
		{
			Source = source;
			Destination = destination;
			Value = value;
		}
	}
}