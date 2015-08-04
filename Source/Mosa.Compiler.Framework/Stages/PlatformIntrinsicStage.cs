// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class PlatformIntrinsicStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (!(node.Instruction is IntrinsicMethodCall))
						continue;

					string external = node.InvokeMethod.ExternMethod;

					//TODO: Verify!

					Type intrinsicType = Type.GetType(external);

					if (intrinsicType == null)
						return;

					var instance = Activator.CreateInstance(intrinsicType) as IIntrinsicPlatformMethod;

					if (instance == null)
						return;

					var context = new Context(node);

					instance.ReplaceIntrinsicCall(context, MethodCompiler);
				}
			}
		}
	}
}
