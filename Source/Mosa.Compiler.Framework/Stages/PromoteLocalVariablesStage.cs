/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class PromoteLocalVariablesStage : BaseMethodCompilerStage
	{
		protected SectionTrace trace;

		protected override void Run()
		{
			// Unable to optimize SSA w/ exceptions or finally handlers present
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			if (HasProtectedRegions)
				return;

			trace = CreateTrace();

			foreach (var local in MethodCompiler.LocalVariables)
			{
				if (local.IsVirtualRegister)
					continue;

				if (local.Definitions.Count != 1)
					continue;

				if (!local.IsReferenceType && !local.IsInteger && !local.IsR && !local.IsChar && !local.IsBoolean && !local.IsPointer)
					continue;

				if (ContainsAddressOf(local))
					continue;

				Promote(local);
			}
		}

		protected bool ContainsAddressOf(Operand local)
		{
			foreach (int index in local.Uses)
			{
				Context ctx = new Context(InstructionSet, index);

				if (ctx.Instruction == IRInstruction.AddressOf)
					return true;
			}

			return false;
		}

		protected void Promote(Operand local)
		{
			var stacktype = local.Type.GetStackType();

			var v = MethodCompiler.CreateVirtualRegister(stacktype);

			if (trace.Active) trace.Log("*** Replacing: " + local.ToString() + " with " + v.ToString());

			foreach (int index in local.Uses.ToArray())
			{
				Context ctx = new Context(InstructionSet, index);

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetOperand(i);

					if (local == operand)
					{
						if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
						ctx.SetOperand(i, v);
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}
			}

			foreach (int index in local.Definitions.ToArray())
			{
				Context ctx = new Context(InstructionSet, index);

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetResult(i);

					if (local == operand)
					{
						if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
						ctx.SetResult(i, v);
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}
			}
		}
	}
}