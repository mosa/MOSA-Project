/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Intrinsics
{
	public sealed class InternalGetType : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			string arch = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName;

			var type = methodCompiler.TypeSystem.GetTypeByName(arch, "Runtime");
			Debug.Assert(type != null, "Cannot find " + arch + ".Runtime");

			var method = type.FindMethodByName("GetTypeHandle");

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand objectOperand = context.Operand1;
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand, objectOperand);
			context.MosaMethod = method;
		}
	}
}