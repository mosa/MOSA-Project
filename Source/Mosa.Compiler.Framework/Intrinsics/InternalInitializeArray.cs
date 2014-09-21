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
	[ReplacementTarget("System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray")]
	public sealed class InternalInitializeArray : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var method = methodCompiler.Compiler.PlatformInternalRuntimeType.FindMethodByName("InitializeArray");

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand arrayOperand = context.Operand1;
			Operand fieldOperand = context.Operand2;

			context.SetInstruction(IRInstruction.Call, null, callTargetOperand, arrayOperand, fieldOperand);
			context.MosaMethod = method;
		}
	}
}