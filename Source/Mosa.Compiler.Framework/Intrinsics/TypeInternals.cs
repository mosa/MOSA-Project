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
					this.GetTypeHandle(context, methodCompiler);
					break;

				case "InternalGetFullName":
					this.InternalGetFullname(context, methodCompiler);
					break;

				case "InternalGetTypeHandleByName":
					this.InternalGetTypeHandleByName(context, methodCompiler);
					break;

				default:
					throw new NotSupportedException("No platform specific method implementation found for " + context.MosaMethod.Name + "!");
			}
		}

		private void GetTypeHandle(Context context, BaseMethodCompiler methodCompiler)
		{
			string arch = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName;

			var type = methodCompiler.TypeSystem.GetTypeByName(arch, "Runtime");
			Debug.Assert(type != null, "Cannot find " + arch + ".Runtime");

			var method = type.FindMethodByName("Metadata_Type_GetHandleFromObject");

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand objectOperand = context.Operand1;
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand, objectOperand);
			context.MosaMethod = method;
		}

		private void InternalGetFullname(Context context, BaseMethodCompiler methodCompiler)
		{
			string arch = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName;

			var type = methodCompiler.TypeSystem.GetTypeByName(arch, "Runtime");
			Debug.Assert(type != null, "Cannot find " + arch + ".Runtime");

			var method = type.FindMethodByName("Metadata_Type_GetFullName");

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand objectOperand = context.Operand1;
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand, objectOperand);
			context.MosaMethod = method;
		}

		private void InternalGetTypeHandleByName(Context context, BaseMethodCompiler methodCompiler)
		{
			string arch = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName;

			var type = methodCompiler.TypeSystem.GetTypeByName(arch, "Runtime");
			Debug.Assert(type != null, "Cannot find " + arch + ".Runtime");

			var method = type.FindMethodByName("Metadata_Type_GetHandleByName");

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand typeNameOperand = context.Operand1;
			Operand throwOnErrorOperand = context.Operand2;
			Operand ignoreCaseOperand = context.Operand3;
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand, typeNameOperand);
			context.SetOperand(2, throwOnErrorOperand);
			context.SetOperand(3, ignoreCaseOperand);
			context.OperandCount = 4;
			context.MosaMethod = method;
		}
	}
}