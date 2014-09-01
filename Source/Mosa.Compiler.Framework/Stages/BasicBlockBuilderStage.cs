/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This compilation stage is used by method compilers after the
	/// IL decoding stage to build basic Blocks out of the instruction list.
	/// </summary>
	public sealed class BasicBlockBuilderStage : BaseMethodCompilerStage
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private BasicBlock epilogue;

		/// <summary>
		///
		/// </summary>
		private BasicBlock prologue;

		#endregion Data members

		protected override void Run()
		{
			// No basic block building if this is a linker generated method
			if (MethodCompiler.Method.IsLinkerGenerated)
				return;

			if (MethodCompiler.Compiler.PlugSystem.GetPlugMethod(MethodCompiler.Method) != null)
				return;

			if (MethodCompiler.Method.Code.Count == 0)
				return;

			// Create the prologue block
			var context = CreateNewBlockWithContext(BasicBlock.PrologueLabel);

			// Add a jump instruction to the first block from the prologue
			context.AppendInstruction(IRInstruction.Jmp);
			context.SetBranch(0);
			prologue = context.BasicBlock;
			BasicBlocks.AddHeaderBlock(prologue);

			// Create the basic blocks
			var targets = FindBasicBlockTargets();
			CreateBasicBlocksFromTargets(targets);

			// Create the epilogue block
			context = CreateNewBlockWithContext(BasicBlock.EpilogueLabel);
			epilogue = context.BasicBlock;

			// Link all the blocks together
			BuildBasicBlockLinks(prologue);

			foreach (var clause in MethodCompiler.Method.ExceptionBlocks)
			{
				if (clause.HandlerStart != 0)
				{
					//trace.Log("Exception HandlerOffset: 0x" + clause.HandlerStart.ToString("X4"));

					var basicBlock = BasicBlocks.GetByLabel(clause.HandlerStart);
					BuildBasicBlockLinks(basicBlock);
					BasicBlocks.AddHeaderBlock(basicBlock);
				}
				if (clause.FilterOffset != null)
				{
					//trace.Log("Exception FilterOffset: 0x" + clause.FilterOffset.Value.ToString("X4"));

					var basicBlock = BasicBlocks.GetByLabel(clause.FilterOffset.Value);
					BuildBasicBlockLinks(basicBlock);
					BasicBlocks.AddHeaderBlock(basicBlock);
				}
			}
		}

		/// <summary>
		/// Finds basic block targets.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidCompilerException"></exception>
		private SortedSet<int> FindBasicBlockTargets()
		{
			var targets = new SortedSet<int>();

			targets.Add(0);

			// Find out all targets labels
			for (var ctx = new Context(InstructionSet, 0); ctx.Index >= 0; ctx.GotoNext())
			{
				switch (ctx.Instruction.FlowControl)
				{
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Break: goto case FlowControl.UnconditionalBranch;
					case FlowControl.Return: continue;
					case FlowControl.Throw: continue;
					case FlowControl.UnconditionalBranch:
						{
							//Debug.Assert(ctx.BranchTargets.Length == 1);
							targets.AddIfNew(ctx.BranchTargets[0]);
							continue;
						}
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.ConditionalBranch:
						{
							foreach (int target in ctx.BranchTargets)
								targets.AddIfNew(target);
							targets.AddIfNew(ctx.Next.Label);
							continue;
						}
					case FlowControl.EndFinally: continue;
					case FlowControl.Leave:
						{
							//Debug.Assert(ctx.BranchTargets.Length == 1);
							targets.AddIfNew(ctx.BranchTargets[0]);
							continue;
						}
					default:
						throw new InvalidCompilerException();
				}
			}

			// Add exception targets
			foreach (var clause in MethodCompiler.Method.ExceptionBlocks)
			{
				targets.AddIfNew(clause.HandlerStart);
				targets.AddIfNew(clause.TryStart);

				if (clause.FilterOffset != null)
					targets.AddIfNew(clause.FilterOffset.Value);
			}

			//foreach (var target in targets)
			//{
			//	//trace.Log("Target: " + target.ToString("X4"));
			//}

			return targets;
		}

		/// <summary>
		/// Creates the basic blocks from targets.
		/// </summary>
		/// <param name="targets">The targets.</param>
		private void CreateBasicBlocksFromTargets(SortedSet<int> targets)
		{
			BasicBlock currentBlock = null;
			Context previous = null;

			for (var ctx = new Context(InstructionSet, 0); ctx.Index >= 0; previous = ctx.Clone(), ctx.GotoNext())
			{
				if (!targets.Contains(ctx.Label))
					continue;

				if (currentBlock != null)
				{
					previous = ctx.Previous;

					var flow = previous.Instruction.FlowControl;

					if (flow == FlowControl.Next || flow == FlowControl.Call || flow == FlowControl.ConditionalBranch || flow == FlowControl.Switch)
					{
						// This jump joins fall-through blocks by giving them a proper end.
						previous.AppendInstruction(IRInstruction.Jmp);
						previous.SetBranch(ctx.Label);
					}

					// Close current block
					previous.AppendInstruction(IRInstruction.BlockEnd);
					currentBlock.EndIndex = previous.Index;
				}

				Context prev = ctx.InsertBefore();
				prev.SetInstruction(IRInstruction.BlockStart);
				currentBlock = BasicBlocks.CreateBlock(ctx.Label, prev.Index);
			}

			// Close current block
			previous.AppendInstruction(IRInstruction.BlockEnd);
			currentBlock.EndIndex = previous.Index;
		}

		/// <summary>
		/// Builds the basic block links.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <exception cref="InvalidCompilerException"></exception>
		private void BuildBasicBlockLinks(BasicBlock block)
		{
			for (var ctx = CreateContext(block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
			{
				switch (ctx.Instruction.FlowControl)
				{
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Return:
						{
							if (!block.NextBlocks.Contains(epilogue))
								LinkBlocks(block, epilogue);
							return;
						}
					case FlowControl.Break: goto case FlowControl.UnconditionalBranch;
					case FlowControl.Throw: continue;
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.UnconditionalBranch:
						{
							LinkBlocks(block, ctx.BranchTargets[0]);
							return;
						}
					case FlowControl.ConditionalBranch:
						{
							foreach (int target in ctx.BranchTargets)
								LinkBlocks(block, target);

							int nextIndex = ctx.Index + 1;
							if (nextIndex < this.InstructionSet.Used)
								LinkBlocks(block, InstructionSet.Data[nextIndex].Label);
							continue;
						}
					case FlowControl.EndFinally: return;
					case FlowControl.Leave:
						{
							bool createLink = false;

							var entry = FindImmediateExceptionEntry(ctx);

							if (entry != null)
							{
								if (entry.IsLabelWithinTry(ctx.Label))
									createLink = true;
							}

							if (createLink)
							{
								foreach (int target in ctx.BranchTargets)
									LinkBlocks(block, target);
							}

							return;
						}
					default:
						throw new InvalidCompilerException();
				}
			}
		}

		private void LinkBlocks(BasicBlock block, int target)
		{
			var next = BasicBlocks.GetByLabel(target);
			if (!block.NextBlocks.Contains(next))
			{
				LinkBlocks(block, next);
				BuildBasicBlockLinks(next);
			}
		}

		/// <summary>
		/// Links the Blocks.
		/// </summary>
		/// <param name="caller">The caller.</param>
		/// <param name="callee">The callee.</param>
		private void LinkBlocks(BasicBlock caller, BasicBlock callee)
		{
			// Chain the blocks together
			caller.NextBlocks.Add(callee);
			callee.PreviousBlocks.Add(caller);
		}
	}
}