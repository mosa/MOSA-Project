/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework
{

	/// <summary>
	/// 
	/// </summary>
	public sealed class LiveRangeAnalysis
	{

		public struct LiveRange
		{
			int Start { get; set; }
			int End { get; set; }

			public LiveRange(int start, int end)
				: this()
			{
				Start = start;
				End = end;
			}
		}

		public sealed class TemporaryLiveRange
		{
			public List<LiveRange> liveRanges = new List<LiveRange>(1);

			public Operand Operand { get; private set; }
			public List<LiveRange> LiveRanges { get { return liveRanges; } }
			public int Count { get { return liveRanges.Count; } }

			public void AddRange(LiveRange liveRange)
			{
				liveRanges.Add(liveRange);
			}

			public TemporaryLiveRange(Operand temporary)
			{
				Operand = temporary;
			}
		}

		private BaseMethodCompiler methodCompiler;
		private BasicBlocks basicBlocks;
		private TemporaryLiveRange[] temporaryLiveRanges;
		private int[] instructionNumbering;
		//private int[] contextIndexToBlock;

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public LiveRangeAnalysis(BaseMethodCompiler methodCompiler)
		{
			this.methodCompiler = methodCompiler;
			this.basicBlocks = methodCompiler.BasicBlocks;
			this.instructionNumbering = new int[methodCompiler.InstructionSet.Size];
			this.temporaryLiveRanges = new TemporaryLiveRange[methodCompiler.StackLayout.StackLocalTempCount];

			// Number all the instructions in block order
			NumberInstructions();

			// Collect all virtual registers and setup live ranges for each
			CollectVirtualRegisters();

			// For each block, determine what virtual registers were used and defined
			// TODO:

			// Calculate live ranges of temporary variables
			foreach (var temp in temporaryLiveRanges)
			{
				CalculateLiveRanges(temp);
			}
		}

		private void NumberInstructions()
		{
			int index = 2;
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context context = new Context(methodCompiler.InstructionSet, block); !context.EndOfInstruction; context.GotoNext())
				{
					if (!context.IsEmpty)
					{
						instructionNumbering[context.Index] = index;
						index = index + 2;
					}
				}
			}
		}

		private void CollectVirtualRegisters()
		{
			foreach (var temp in methodCompiler.StackLayout.Stack)
			{
				if (temp.IsVirtualRegister)
				{
					temporaryLiveRanges[temp.Sequence] = new TemporaryLiveRange(temp);
				}
			}
		}

		private void CalculateLiveRanges(TemporaryLiveRange temp)
		{
			var definitions = temp.Operand.Definitions;

			if (definitions.Count == 0)
				return;

			var uses = temp.Operand.Uses;

			// TODO
		}

	}

}

