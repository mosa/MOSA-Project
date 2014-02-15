/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Intrinsics
{
	public sealed class InternalInitializeArray : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			string runtime = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName + ".Runtime";

			var type = methodCompiler.TypeSystem.GetTypeByName(runtime);
			Debug.Assert(type != null, "Cannot find " + runtime);

			var method = TypeSystem.GetMethodByName(type, "InitializeArray");

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand arrayOperand = context.Operand1;
			Operand fieldOperand = context.Operand2;

			context.SetInstruction(IRInstruction.Call, null, callTargetOperand, arrayOperand, fieldOperand);
			context.InvokeMethod = method;
		}
	}
}