// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Trace;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Devirtualize Call Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class DevirtualizeCallStage : BaseCodeTransformationStage
	{
		private TraceLog trace;
		private int devirtualizedCount = 0;

		protected override void PopulateVisitationDictionary()
		{
			//AddVisitation(IRInstruction.CallInterface, CallInterface);
			AddVisitation(IRInstruction.CallVirtual, CallVirtual);
		}

		protected override void Setup()
		{
			base.Setup();
			trace = CreateTraceLog();
		}

		protected override void Finish()
		{
			UpdateCounter("Devirtualize.CallCount", devirtualizedCount);
		}

		private void CallVirtual(InstructionNode node)
		{
			var call = node.Operand1;
			var method = call.Method;

			if (TypeLayout.IsMethodOverridden(method))
				return;

			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			var operands = node.GetOperands();
			operands.RemoveAt(0);

			if (trace.Active)
			{
				trace.Log("De-virtualize: " + method);
			}

			devirtualizedCount++;

			node.SetInstruction(IRInstruction.CallStatic, node.Result, symbol, operands);
		}
	}
}
