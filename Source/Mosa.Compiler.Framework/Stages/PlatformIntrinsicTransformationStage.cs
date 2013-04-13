/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class PlatformIntrinsicTransformationStage : BaseMethodCompilerStage, IMethodCompilerStage
	{
		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context context = CreateContext(block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					if (!(context.Instruction is IntrinsicMethodCall))
						continue;

					string external = context.InvokeTarget.Module.GetExternalName(context.InvokeTarget.Token);

					//TODO: Verify!

					Type intrinsicType = Type.GetType(external);

					if (intrinsicType == null)
						return;

					var instance = Activator.CreateInstance(intrinsicType) as IIntrinsicPlatformMethod;

					if (instance == null)
						return;

					instance.ReplaceIntrinsicCall(context, methodCompiler);
				}
			}
		}

		#endregion IMethodCompilerStage Members
	}
}