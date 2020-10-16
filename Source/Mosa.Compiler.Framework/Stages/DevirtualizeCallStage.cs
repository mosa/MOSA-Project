// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		private void CallVirtual(Context context)
		{
			var method = context.Operand1.Method;

			// Next lines are not necessary but faster then getting the method data
			if (!method.HasImplementation && method.IsAbstract)
				return;

			var methodData = MethodCompiler.Compiler.GetMethodData(method);

			if (!methodData.IsDevirtualized)
				return;

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			var operands = context.GetOperands();
			operands.RemoveAt(0);

			trace?.Log($"De-virtualize: {method}");

			DevirtualizedMethodCallsCount++;

			context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, operands);

			MethodScanner.MethodInvoked(method, Method);
		}
	}
}
