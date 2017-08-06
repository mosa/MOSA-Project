// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// New Object Stage
	/// </summary>
	public sealed class NewObjectStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction == IRInstruction.NewObject)
					{
						NewObject(node);
					}
					else if (node.Instruction == IRInstruction.NewArray)
					{
						NewArray(node);
					}
				}
			}
		}

		private void NewObject(InstructionNode node)
		{
			string methodName = VmCall.AllocateObject.ToString();
			var context = new Context(node);

			var method = InternalRuntimeType.FindMethodByName(methodName) ?? PlatformInternalRuntimeType.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			var runtimeHandle = context.Operand1;
			var size = context.Operand2;
			var result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, symbol, runtimeHandle, size);
			context.InvokeMethod = method;
		}

		private void NewArray(InstructionNode node)
		{
			string methodName = VmCall.AllocateArray.ToString();
			var context = new Context(node);

			var method = InternalRuntimeType.FindMethodByName(methodName) ?? PlatformInternalRuntimeType.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			var runtimeHandle = context.Operand1;
			var size = context.Operand2;
			var elements = context.Operand3;
			var result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, symbol, runtimeHandle, size, elements);
			context.InvokeMethod = method;
		}
	}
}
