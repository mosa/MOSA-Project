/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{

	public sealed class InternalAllocateString : IIntrinsicInternalMethod
	{
		private const string StringClassMethodTableSymbolName = @"System.String$mtable";

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Operand callTargetOperand = this.GetInternalAllocateStringCallTarget(typeSystem);
			Operand methodTableOperand = Operand.CreateSymbol(BuiltInSigType.IntPtr, StringClassMethodTableSymbolName);
			Operand lengthOperand = context.Operand1;
			Operand result = context.Result;

			context.SetInstruction(IRInstruction.Call, result, callTargetOperand, methodTableOperand, lengthOperand);
		}

		private Operand GetInternalAllocateStringCallTarget(ITypeSystem typeSystem)
		{
			RuntimeType runtimeType = typeSystem.GetType(@"Mosa.Internal.Runtime");
			RuntimeMethod callTarget = runtimeType.FindMethod(@"AllocateString");

			return Operand.CreateSymbolFromMethod(callTarget);
		}
	}
}
