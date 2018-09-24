// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Basics
{
	public sealed class Loop
	{
		public BasicBlock Header { get; set; }
		public readonly List<BasicBlock> Backedges = new List<BasicBlock>();
		public readonly List<BasicBlock> nodes = new List<BasicBlock>();

		public Loop(BasicBlock header, BasicBlock backedge)
		{
			Header = header;
			Backedges.Add(backedge);
		}
	}
}
