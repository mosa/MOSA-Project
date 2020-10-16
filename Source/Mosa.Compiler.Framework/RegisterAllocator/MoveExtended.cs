// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class MoveExtended<T>
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
