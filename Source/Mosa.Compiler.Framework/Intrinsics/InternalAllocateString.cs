/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("System.String::InternalAllocateString")]
	public sealed class InternalAllocateString : IIntrinsicInternalMethod
	{
		private const string StringClassTypeDefinitionSymbolName = @"System.String" + Metadata.TypeDefinition;

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

			Operand typeDefinitionOperand = GetRuntimeTypeHandle(context, methodCompiler);
			Operand lengthOperand = context.Operand1;
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand, typeDefinitionOperand, lengthOperand);
			context.MosaMethod = method;
		}

		private Operand GetRuntimeTypeHandle(Context context, BaseMethodCompiler methodCompiler)
		{
			var typeDef = Operand.CreateUnmanagedSymbolPointer(methodCompiler.TypeSystem, StringClassTypeDefinitionSymbolName);
			var runtimeTypeHandle = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"));
			var before = context.InsertBefore();
			before.SetInstruction(IRInstruction.Move, runtimeTypeHandle, typeDef);
			return runtimeTypeHandle;
		}
	}
}