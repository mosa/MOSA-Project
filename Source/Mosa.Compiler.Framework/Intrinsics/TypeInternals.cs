/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("System.Type::GetTypeHandle")]
	[ReplacementTarget("System.Type::InternalGetFullName")]
	[ReplacementTarget("System.Type::InternalGetAttributes")]
	[ReplacementTarget("System.Type::InternalGetTypeHandleByName")]
	public sealed class TypeInternals : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			switch (context.MosaMethod.Name)
			{
				case "GetTypeHandle":
					this.Internal(context, methodCompiler, "Metadata_Type_GetHandleFromObject");
					break;

				case "InternalGetFullName":
					this.Internal(context, methodCompiler, "Metadata_Type_GetFullName");
					break;

				case "InternalGetAttributes":
					this.Internal(context, methodCompiler, "Metadata_Type_GetAttributes");
					break;

				case "InternalGetTypeHandleByName":
					this.Internal(context, methodCompiler, "Metadata_Type_GetHandleByName");
					break;

				default:
					throw new NotSupportedException("No platform specific method implementation found for " + context.MosaMethod.Name + "!");
			}
		}

		private void Internal(Context context, BaseMethodCompiler methodCompiler, string internalMethod)
		{
			string arch = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName;

			var type = methodCompiler.TypeSystem.GetTypeByName(arch, "Runtime");
			Debug.Assert(type != null, "Cannot find " + arch + ".Runtime");

			var method = type.FindMethodByName(internalMethod);

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand[] operands = new Operand[context.OperandCount];
			for (int i = 0; i < context.OperandCount; i++)
				operands[i] = context.GetOperand(i);
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand);
			for (int i = 0; i < operands.Length; i++)
				context.SetOperand(1 + i, operands[i]);
			context.OperandCount = (byte)(1 + operands.Length);
			context.MosaMethod = method;
		}
	}
}