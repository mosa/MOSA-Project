// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class PromoteTempVariablesStage : PromoteLocalVariablesStage
	{
		protected override void Run()
		{
			//HACK!!! HACK!!! HACK!!!
			//if (MethodCompiler.Method.FullName.Contains(" Mosa.Platform.Internal.x86.Runtime::GetProtectedRegionEntryByAddress"))
			//	return;

			if (!HasCode)
				return;

			// Unable to optimize SSA w/ exceptions or finally handlers present
			if (HasProtectedRegions)
				return;

			trace = CreateTraceLog();

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

				Promote(local);
			}
		}
	}
}
