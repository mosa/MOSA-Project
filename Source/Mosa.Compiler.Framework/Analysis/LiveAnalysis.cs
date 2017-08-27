// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.RegisterAllocator;
using Mosa.Compiler.Trace;
using System.Collections;
using System.Collections.Generic;
using System;
using Mosa.Compiler.Common;
using System.Diagnostics;

// INCOMPLETE

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	/// This stage determine were object references are located in code.
	/// </summary>
	public class LiveAnalysis
	{
		private class LiveRanges
		{
		}

		private BaseLiveAnalysisEnvironment Environment;
		private BasicBlocks BasicBlocks { get { return Environment.BasicBlocks; } }
		private int IndexCount { get { return Environment.IndexCount; } }

		private readonly ITraceFactory TraceFactory;

		private List<ExtendedBlock> ExtendedBlocks;

		//private TraceLog trace;

		private TraceLog CreateTraceLog(string name)
		{
			return TraceFactory.CreateTraceLog(name);
		}

		public LiveAnalysis(BaseLiveAnalysisEnvironment environment, ITraceFactory traceFactory)
		{
			Environment = environment;
			TraceFactory = traceFactory;

			Run();
		}

		protected void Run()
		{
			//trace = CreateTraceLog("Analysis");

			CreateExtendedBlocks();

			ComputeLocalLiveSets();

			ComputeLocalLiveSets();
		}

		private void CreateExtendedBlocks()
		{
			ExtendedBlocks = new List<ExtendedBlock>(BasicBlocks.Count);

			foreach (var block in BasicBlocks)
			{
				ExtendedBlocks.Add(new ExtendedBlock(block, IndexCount, 0));
			}
		}

		public void NumberInstructions()
		{
			const int increment = SlotIndex.Increment;
			int index = increment;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; ; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					node.Offset = index;
					index += increment;

					if (node.IsBlockEndInstruction)
						break;
				}

				Debug.Assert(block.Last.Offset != 0);
			}

			foreach (var block in BasicBlocks)
			{
				var start = new SlotIndex(block.First);
				var end = new SlotIndex(block.Last);
				ExtendedBlocks[block.Sequence].Interval = new Interval(start, end);
			}
		}

		private void ComputeLocalLiveSets()
		{
			var liveSetTrace = CreateTraceLog("ComputeLocalLiveSets");

			foreach (var block in ExtendedBlocks)
			{
				if (liveSetTrace.Active)
					liveSetTrace.Log("Block # " + block.BasicBlock.Sequence.ToString());

				var liveGen = new BitArray(IndexCount, false);
				var liveKill = new BitArray(IndexCount, false);

				if (BasicBlocks.HeadBlocks.Contains(block.BasicBlock))
				{
					for (int s = 0; s < IndexCount; s++)
					{
						liveKill.Set(s, true);
					}

					if (liveSetTrace.Active)
						liveSetTrace.Log("KILL ALL PHYSICAL");
				}

				for (var node = block.BasicBlock.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (liveSetTrace.Active)
						liveSetTrace.Log(node.ToString());

					foreach (var index in Environment.GetInput(node))
					{
						//if (liveSetTrace.Active)
						//	liveSetTrace.Log("INPUT:  " + op);

						if (!liveKill.Get(index))
						{
							liveGen.Set(index, true);

							if (liveSetTrace.Active)
							{
								liveSetTrace.Log("GEN:  " + index.ToString()/* + " " + op*/);
							}
						}
					}

					foreach (var index in Environment.GetKill(node))
					{
						liveKill.Set(index, true);
					}

					foreach (var index in Environment.GetOutput(node))
					{
						//if (liveSetTrace.Active)
						//liveSetTrace.Log("OUTPUT: " + op);

						liveKill.Set(index, true);

						if (liveSetTrace.Active)
							liveSetTrace.Log("KILL: " + index.ToString() /*+ " " + op*/);
					}
				}

				block.LiveGen = liveGen;
				block.LiveKill = liveKill;
				block.LiveKillNot = ((BitArray)liveKill.Clone()).Not();

				if (liveSetTrace.Active)
				{
					liveSetTrace.Log("GEN:     " + block.LiveGen.ToString2());
					liveSetTrace.Log("KILL:    " + block.LiveKill.ToString2());
					liveSetTrace.Log("KILLNOT: " + block.LiveKillNot.ToString2());
					liveSetTrace.Log(string.Empty);
				}
			}
		}

		private void ComputeGlobalLiveSets()
		{
			bool changed = true;

			while (changed)
			{
				changed = false;

				for (int i = BasicBlocks.Count - 1; i >= 0; i--)
				{
					var block = ExtendedBlocks[i];

					var liveOut = new BitArray(IndexCount);

					foreach (var next in block.BasicBlock.NextBlocks)
					{
						liveOut.Or(ExtendedBlocks[next.Sequence].LiveIn);
					}

					var liveIn = (BitArray)block.LiveOut.Clone();
					liveIn.And(block.LiveKillNot);
					liveIn.Or(block.LiveGen);

					// compare them for any changes
					if (!changed)
					{
						if (!block.LiveOut.AreSame(liveOut) || !block.LiveIn.AreSame(liveIn))
						{
							changed = true;
						}
					}

					block.LiveOut = liveOut;
					block.LiveIn = liveIn;
				}
			}
		}
	}
}
