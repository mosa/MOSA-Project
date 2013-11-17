/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Intrinsics
{
	public sealed class InternalAllocateString : IIntrinsicInternalMethod
	{
		private const string StringClassMethodTableSymbolName = @"System.String$mtable";

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			string runtime = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName + ".Runtime";

			RuntimeType runtimeType = methodCompiler.TypeSystem.GetType(runtime);
			Debug.Assert(runtimeType != null, "Cannot find " + runtime);

			RuntimeMethod callTarget = runtimeType.FindMethod("AllocateString");

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(callTarget);

			Operand methodTableOperand = Operand.CreateSymbol(BuiltInSigType.IntPtr, StringClassMethodTableSymbolName);
			Operand lengthOperand = context.Operand1;
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand, methodTableOperand, lengthOperand);
			context.InvokeMethod = callTarget;
		}
	}
}