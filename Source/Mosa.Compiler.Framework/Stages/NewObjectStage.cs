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

			// fixme: move is a hack because the x86 emitter can't handle it
			var v1 = AllocateVirtualRegister(runtimeHandle.Type);
			context.SetInstruction(IRInstruction.MoveInteger, v1, runtimeHandle);

			context.AppendInstruction(IRInstruction.Call, result, symbol, v1, size);
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

			// fixme: move is a hack because the x86 emitter can't handle it
			var v1 = AllocateVirtualRegister(runtimeHandle.Type);
			context.SetInstruction(IRInstruction.MoveInteger, v1, runtimeHandle);

			context.AppendInstruction(IRInstruction.Call, result, symbol, v1, size, elements);
			context.InvokeMethod = method;
		}
	}
}
