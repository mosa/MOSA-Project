/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class PlatformIntrinsicTransformationStage : BaseMethodCompilerStage, IMethodCompilerStage, IPlatformStage
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context context = CreateContext(block); !context.EndOfInstruction; context.GotoNext())
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

					instance.ReplaceIntrinsicCall(context, typeSystem, methodCompiler.Method.Parameters);
				}
			}
		}

		#endregion // IMethodCompilerStage Members

	}
}


