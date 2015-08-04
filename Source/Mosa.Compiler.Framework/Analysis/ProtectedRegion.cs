// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	public class ProtectedRegion
	{
		public MosaExceptionHandler Handler { get; private set; }

		private List<BasicBlock> included = new List<BasicBlock>();
		private List<BasicBlock> excluded = new List<BasicBlock>();
		private List<BasicBlock> final = new List<BasicBlock>();

		public IList<BasicBlock> IncludedBlocks { get { return final.AsReadOnly(); } }

		public ProtectedRegion(BasicBlocks basicBlocks, MosaExceptionHandler exceptionHandler)
		{
			this.Handler = exceptionHandler;

			foreach (var block in basicBlocks)
			{
				if (block.Label >= exceptionHandler.TryStart && block.Label < exceptionHandler.TryEnd)
					included.Add(block);
				else
					excluded.Add(block);
			}
		}

		public void Finalize(BasicBlocks basicBlocks)
		{
			foreach (var block in included)
			{
				if (!basicBlocks.Contains(block))
					continue;

				Trace(block);
			}
		}

		private void Trace(BasicBlock block)
		{
			if (excluded.Contains(block))
				return;

			if (final.Contains(block))
				return;

			final.Add(block);

			foreach (var next in block.NextBlocks)
				Trace(next);

			foreach (var prev in block.PreviousBlocks)
				Trace(prev);
		}

		public static IList<ProtectedRegion> CreateProtectedRegions(BasicBlocks basicBlocks, IList<MosaExceptionHandler> exceptionHandlers)
		{
			var protectedRegions = new List<ProtectedRegion>(exceptionHandlers.Count);

			foreach (var handler in exceptionHandlers)
			{
				var protectedRegion = new ProtectedRegion(basicBlocks, handler);
				protectedRegions.Add(protectedRegion);
			}

			return protectedRegions;
		}

		public static void FinalizeAll(BasicBlocks basicBlocks, IList<ProtectedRegion> protectedRegions)
		{
			foreach (var region in protectedRegions)
			{
				region.Finalize(basicBlocks);
			}
		}
	}
}
