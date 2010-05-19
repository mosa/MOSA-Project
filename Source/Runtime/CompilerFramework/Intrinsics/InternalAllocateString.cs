/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */
namespace Mosa.Runtime.CompilerFramework.Intrinsics
{
    using Mosa.Runtime.CompilerFramework.Operands;
    using Mosa.Runtime.Metadata.Signatures;
    using Mosa.Runtime.Vm;

    public sealed class InternalAllocateString : IIntrinsicMethod
    {
        private const string StringClassMethodTableSymbolName = @"System.String$mtable";

        public void ReplaceIntrinsicCall(Context context)
        {
            SymbolOperand callTargetOperand = this.GetInternalAllocateStringCallTarget();
            SymbolOperand methodTableOperand = new SymbolOperand(BuiltInSigType.IntPtr, StringClassMethodTableSymbolName);
            Operand lengthOperand = context.Operand1;
            Operand result = context.Result;

            context.SetInstruction(IR.Instruction.CallInstruction, result, callTargetOperand, methodTableOperand, lengthOperand);
        }

        private SymbolOperand GetInternalAllocateStringCallTarget()
        {
            RuntimeType runtimeType = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Runtime.RuntimeBase");
            RuntimeMethod callTarget = runtimeType.FindMethod(@"AllocateString");

            return SymbolOperand.FromMethod(callTarget);
        }
    }
}
