// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Inline Method Data
	/// </summary>
	public sealed class InlineMethodData
	{
		public BasicBlocks BasicBlocks { get; }
		public List<MosaMethod> Callee { get; }

		public InlineMethodData(BasicBlocks basicBlocks, List<MosaMethod> callee)
		{
			BasicBlocks = basicBlocks;
			Callee = callee;
		}
	}
}
