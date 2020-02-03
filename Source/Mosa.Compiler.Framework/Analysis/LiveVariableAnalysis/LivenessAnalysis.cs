// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using static Mosa.Compiler.Framework.BaseMethodCompilerStage;

// INCOMPLETE

namespace Mosa.Compiler.Framework.Analysis.LiveVariableAnalysis
{
	/// <summary>
	/// This stage determine were object references are located in code.
	/// </summary>
	public class LivenessAnalysis
	{
		protected BaseLivenessAnalysisEnvironment Environment;
		protected BasicBlocks BasicBlocks { get { return Environment.BasicBlocks; } }
		protected int IndexCount { get { return Environment.IndexCount; } }

		protected List<ExtendedBlock2> ExtendedBlocks;
		public LiveRanges[] LiveRanges;

		private readonly CreateTraceHandler CreateTrace;

		public LivenessAnalysis(BaseLivenessAnalysisEnvironment environment, CreateTraceHandler createTrace, bool numberInstructions)
		{
			Environment = environment;
			CreateTrace = createTrace;

			LiveRanges = new LiveRanges[IndexCount];

			for (int i = 0; i < IndexCount; i++)
			{
				LiveRanges[i] = new LiveRanges();
			}

			if (numberInstructions)
			{
				NumberInstructions();
			}

			TraceNumberInstructions();

			CreateExtendedBlocks();

			ComputeLocalLiveSets();

			ComputeLocalLiveSets();

			BuildLiveIntervals();
		}

		protected void NumberInstructions()
		{
			const int increment = 2;
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
		}

		private void TraceNumberInstructions()
		{
			var numberTrace = CreateTrace("InstructionNumber", 9);

			if (numberTrace == null)
				return;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; ; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					string log = $"{node.Offset} = {node}";

					if (node.IsBlockStartInstruction)
					{
						log = $"{log} # {block}";
					}

					numberTrace.Log(log);

					if (node.IsBlockEndInstruction)
						break;
				}
			}
		}

		protected void CreateExtendedBlocks()
		{
			ExtendedBlocks = new List<ExtendedBlock2>(BasicBlocks.Count);

			foreach (var block in BasicBlocks)
			{
				var extendedBlock = new ExtendedBlock2(block, IndexCount, 0)
				{
					Range = new Range(block.First.Offset, block.Last.Offset)
				};

				ExtendedBlocks.Add(extendedBlock);
			}
		}

		protected void ComputeLocalLiveSets()
		{
			var liveSetTrace = CreateTrace("ComputeLocalLiveSets", 9);

			foreach (var block in ExtendedBlocks)
			{
				var liveGen = new BitArray(IndexCount, false);
				var liveKill = new BitArray(IndexCount, false);

				if (block.BasicBlock.IsHeadBlock)
				{
					for (int s = 0; s < IndexCount; s++)
					{
						liveKill.Set(s, true);
					}
				}

				for (var node = block.BasicBlock.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					foreach (var index in Environment.GetInputs(node))
					{
						if (!liveKill.Get(index))
						{
							liveGen.Set(index, true);
						}
					}

					foreach (var index in Environment.GetKills(node))
					{
						liveKill.Set(index, true);
					}

					foreach (var index in Environment.GetOutputs(node))
					{
						liveKill.Set(index, true);
					}
				}

				block.LiveGen = liveGen;
				block.LiveKill = liveKill;
				block.LiveKillNot = ((BitArray)liveKill.Clone()).Not();

				liveSetTrace?.Log($"Block #  {block.BasicBlock.Sequence}");
				liveSetTrace?.Log($"GEN:     {block.LiveGen.ToString2()}");
				liveSetTrace?.Log($"KILL:    {block.LiveKill.ToString2()}");
				liveSetTrace?.Log($"KILLNOT: {block.LiveKillNot.ToString2()}");
				liveSetTrace?.Log();
			}
		}

		protected void ComputeGlobalLiveSets()
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

		protected void BuildLiveIntervals()
		{
			for (int b = BasicBlocks.Count - 1; b >= 0; b--)
			{
				var block = ExtendedBlocks[b];

				for (int r = 0; r < IndexCount; r++)
				{
					if (!block.LiveOut.Get(r))
						continue;

					if (b + 1 != BasicBlocks.Count && ExtendedBlocks[b + 1].LiveIn.Get(r))
					{
						LiveRanges[r].Add(block.Start, ExtendedBlocks[b + 1].Start);
					}
					else
					{
						LiveRanges[r].Add(block.Range);
					}
				}

				for (var node = block.BasicBlock.Last; !node.IsBlockStartInstruction; node = node.Previous)
				{
					if (node.IsEmpty)
						continue;

					foreach (var index in Environment.GetKills(node))
					{
						LiveRanges[index].Add(node.Offset, node.Offset + 1);
					}

					foreach (var index in Environment.GetOutputs(node))
					{
						var liveRange = LiveRanges[index];

						if (liveRange.Count != 0)
						{
							liveRange.FirstRange = new Range(node.Offset, liveRange.FirstRange.End);
						}
						else
						{
							// This is necessary to handled a result that is never used!
							// This is common with instructions with more than one result.
							liveRange.Add(node.Offset, node.Offset + 1);
						}
					}

					foreach (var index in Environment.GetInputs(node))
					{
						LiveRanges[index].Add(block.Start, node.Offset);
					}
				}
			}
		}
	}
}
