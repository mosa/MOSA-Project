/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("System.Threading.SpinLock::EnterLock")]
	[ReplacementTarget("System.Threading.SpinLock::ExitLock")]
	public sealed class InternalSpinLock : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			string arch = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName;

			var type = methodCompiler.TypeSystem.GetTypeByName(arch, "Native");
			Debug.Assert(type != null, "Cannot find " + arch + ".Native");

			var method = context.MosaMethod.Name.Equals("EnterLock") ? type.FindMethodByName("SpinLock") : type.FindMethodByName("SpinUnlock");

			Operand callTargetOperand = Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method);

			Operand refBooleanOperand = context.Operand1;

			context.SetInstruction(IRInstruction.IntrinsicMethodCall, null, callTargetOperand, refBooleanOperand);
			context.MosaMethod = method;
		}
	}
}