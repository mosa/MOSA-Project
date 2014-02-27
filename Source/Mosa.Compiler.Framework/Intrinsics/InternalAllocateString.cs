/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
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
			string arch = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName;

			var type = methodCompiler.TypeSystem.GetTypeByName(arch, "Runtime");
			Debug.Assert(type != null, "Cannot find " + arch + ".Runtime");

			var method = type.FindMethodByName("AllocateString");

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand methodTableOperand = Operand.CreateUnmanagedSymbolPointer(methodCompiler.TypeSystem, StringClassMethodTableSymbolName);
			Operand lengthOperand = context.Operand1;
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand, methodTableOperand, lengthOperand);
			context.MosaMethod = method;
		}
	}
}