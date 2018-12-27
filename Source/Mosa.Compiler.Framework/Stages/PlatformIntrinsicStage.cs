// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Platform Intrinsic Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public sealed class PlatformIntrinsicStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					if (node.Instruction != IRInstruction.IntrinsicMethodCall)
						continue;

					string external = node.Operand1.Method.ExternMethodModule;

					var intrinsicType = Type.GetType(external);

					if (intrinsicType == null)
						return;

					var instance = Activator.CreateInstance(intrinsicType) as IIntrinsicPlatformMethod;

					if (instance == null)
						return;

					var operands = node.GetOperands();
					operands.RemoveAt(0);
					node.SetInstruction(IRInstruction.IntrinsicMethodCall, node.Result, operands);

					var context = new Context(node);

					instance.ReplaceIntrinsicCall(context, MethodCompiler);
				}
			}
		}
	}
}
