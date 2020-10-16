// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class Move
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
