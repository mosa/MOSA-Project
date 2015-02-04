/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	internal class CreateInstanceSimple : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var ctor = context.Operand1;
			var thisObject = context.Operand2;
			var result = context.Result;
			var esp = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Pointer, GeneralPurposeRegister.ESP);
			var edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.Pointer, GeneralPurposeRegister.EDX);
			var espMovment = Operand.CreateConstant(methodCompiler.TypeSystem, 4);

			context.SetInstruction(X86.Sub, esp, esp, espMovment);
			context.AppendInstruction(X86.Mov, edx, esp);
			context.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(methodCompiler.TypeSystem.BuiltIn.Pointer, edx, 0), thisObject);
			context.AppendInstruction(X86.Call, null, ctor);
			context.AppendInstruction(X86.Add, esp, esp, espMovment);
			context.AppendInstruction(X86.Mov, result, thisObject);
		}

		#endregion Methods
	}
}
