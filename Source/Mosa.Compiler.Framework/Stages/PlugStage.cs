﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Replaces methods with their plugged implementation methods
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public class PlugStage : BaseMethodCompilerStage
	{
		private TraceLog trace;

		protected override void Setup()
		{
			base.Setup();

			trace = CreateTraceLog();
		}

		protected override void Run()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction == IRInstruction.CallStatic
						|| node.Instruction == IRInstruction.CallDirect
						|| node.Instruction == IRInstruction.CallVirtual)
					{
						CheckForReplacement(node);
					}
				}
			}
		}

		protected void CheckForReplacement(InstructionNode node)
		{
			var method = node.Operand1.Method;

			if (method == null)
				return;

			var newTarget = MethodCompiler.Compiler.PlugSystem.GetReplacement(method);

			if (newTarget != null)
			{
				Replace(node, newTarget);
			}
		}

		public void Replace(InstructionNode node, MosaMethod newTarget)
		{
			if (trace.Active) trace.Log("*** New Target: " + newTarget);

			if (trace.Active) trace.Log("BEFORE:\t" + node);

			if (node.InvokeMethod != null)
			{
				node.InvokeMethod = newTarget;
			}

			if (node.Operand1 != null && node.Operand1.IsSymbol && node.Operand1.Method != null)
			{
				node.Operand1 = Operand.CreateSymbolFromMethod(newTarget, TypeSystem);
			}

			if (trace.Active) trace.Log("AFTER: \t" + node);
		}
	}
}
