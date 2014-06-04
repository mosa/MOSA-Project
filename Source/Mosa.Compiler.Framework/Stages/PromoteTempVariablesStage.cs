/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class PromoteTempVariablesStage : PromoteLocalVariablesStage
	{
		protected override void Run()
		{
			//if (MethodCompiler.Method.FullName.Contains("Mosa.Kernel.x86.Screen::Write(System.S"))
			//	return;

			// Unable to optimize SSA w/ exceptions or finally handlers present
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			trace = CreateTrace();

			foreach (var local in MethodCompiler.LocalVariables)
			{
				if (local.IsVirtualRegister)
					continue;

				if (local.IsParameter)
					continue;

				if (!local.IsStackLocal)
					continue;

				if (!local.IsReferenceType && !local.IsInteger && !local.IsR && !local.IsChar && !local.IsBoolean && !local.IsPointer)
					continue;

				if (ContainsAddressOf(local))
					continue;

				if (local.Definitions.Count == 0 || local.Uses.Count == 0)
					continue;

				//System.Diagnostics.Debug.WriteLine(this.MethodCompiler.Method.FullName.ToString() + " - " + local.ToString());

				Promote(local);
			}
		}
	}
}