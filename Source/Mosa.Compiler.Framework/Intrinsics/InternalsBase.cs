// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Intrinsics
{
	public abstract class InternalsBase
	{
		/// <summary>
		/// Allows quick internal call replacements
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <param name="internalMethod">The internal method to replace with.</param>
		/// <param name="internalClass">The internal class that has the internal method.</param>
		protected void Internal(Context context, BaseMethodCompiler methodCompiler, string internalMethod, string internalClass = "Runtime")
		{
			if (context == null || methodCompiler == null || internalMethod == null || internalClass == null)
				throw new ArgumentNullException();

			string arch = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName;

			var type = methodCompiler.TypeSystem.GetTypeByName(arch, internalClass);
			Debug.Assert(type != null, "Cannot find " + arch + "." + internalClass);

			var method = type.FindMethodByName(internalMethod);

			Debug.Assert(method != null, "Cannot find " + internalMethod + " in " + type.Name);

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand[] operands = new Operand[context.OperandCount];
			for (int i = 0; i < context.OperandCount; i++)
				operands[i] = context.GetOperand(i);
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand);
			for (int i = 0; i < operands.Length; i++)
				context.SetOperand(1 + i, operands[i]);
			context.OperandCount = (byte)(1 + operands.Length);
			context.InvokeMethod = method;
		}
	}
}