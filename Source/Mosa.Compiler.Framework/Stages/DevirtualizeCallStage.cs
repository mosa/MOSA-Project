// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Devirtualize Call Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class DevirtualizeCallStage : BaseCodeTransformationStage
	{
		private Counter DevirtualizedMethodCallsCount = new Counter("DevirtualizeCallStage.DevirtualizedMethodCalls");

		private TraceLog trace;

		protected override void Initialize()
		{
			base.Initialize();

			Register(DevirtualizedMethodCallsCount);
		}

		protected override void PopulateVisitationDictionary()
		{
			//AddVisitation(IRInstruction.CallInterface, CallInterface);
			AddVisitation(IRInstruction.CallVirtual, CallVirtual);
		}

		protected override void Setup()
		{
			trace = CreateTraceLog();
		}

		protected override void Finish()
		{
			trace = null;
		}

		private void CallVirtual(InstructionNode node)
		{
			var call = node.Operand1;
			var method = call.Method;

			if (TypeLayout.IsMethodOverridden(method))
				return;

			if (!method.HasImplementation && method.IsAbstract)
				return;

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			var operands = node.GetOperands();
			operands.RemoveAt(0);

			trace?.Log($"De-virtualize: {method}");

			DevirtualizedMethodCallsCount++;

			node.SetInstruction(IRInstruction.CallStatic, node.Result, symbol, operands);

			MethodScanner.MethodInvoked(method, Method);
		}
	}
}
