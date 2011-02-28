/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.CompilerFramework.Intrinsics
{

	public sealed class InternalAllocateString : IIntrinsicMethod
	{
		private const string StringClassMethodTableSymbolName = @"System.String$mtable";

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
		{
			SymbolOperand callTargetOperand = this.GetInternalAllocateStringCallTarget(typeSystem);
			SymbolOperand methodTableOperand = new SymbolOperand(BuiltInSigType.IntPtr, StringClassMethodTableSymbolName);
			Operand lengthOperand = context.Operand1;
			Operand result = context.Result;

			context.SetInstruction(IR.Instruction.CallInstruction, result, callTargetOperand, methodTableOperand, lengthOperand);
		}

		private SymbolOperand GetInternalAllocateStringCallTarget(ITypeSystem typeSystem)
		{
			RuntimeType runtimeType = typeSystem.GetType(@"Mosa.Intrinsic.Runtime");
			RuntimeMethod callTarget = runtimeType.FindMethod(@"AllocateString");

			return SymbolOperand.FromMethod(callTarget);
		}
	}
}
