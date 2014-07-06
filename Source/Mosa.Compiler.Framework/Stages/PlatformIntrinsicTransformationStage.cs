/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using System;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class PlatformIntrinsicTransformationStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			foreach (BasicBlock block in BasicBlocks)
			{
				for (Context context = CreateContext(block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					if (!(context.Instruction is IntrinsicMethodCall))
						continue;

					string external = context.MosaMethod.ExternMethod;

					//TODO: Verify!

					Type intrinsicType = Type.GetType(external);

					if (intrinsicType == null)
						return;

					var instance = Activator.CreateInstance(intrinsicType) as IIntrinsicPlatformMethod;

					if (instance == null)
						return;

					instance.ReplaceIntrinsicCall(context, MethodCompiler);
				}
			}
		}
	}
}