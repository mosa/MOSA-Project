/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public interface IILVisitor
    {
        void Nop(NopInstruction instruction);
        void Break(BreakInstruction instruction);
        void Ldarg(LdargInstruction instruction);
        void Ldarga(LdargaInstruction instruction);
        void Ldloc(LdlocInstruction instruction);
        void Ldloca(LdlocaInstruction instruction);
        void Ldc(LdcInstruction instruction);
        void Ldobj(LdobjInstruction instruction);
        void Ldstr(LdstrInstruction instruction);
        void Ldfld(LdfldInstruction instruction);
        void Ldflda(LdfldaInstruction instruction);
        void Ldsfld(LdsfldInstruction instruction);
        void Ldsflda(LdsfldaInstruction instruction);
        void Ldftn(LdftnInstruction instruction);
        void Ldvirtftn(LdvirtftnInstruction instruction);
        void Ldtoken(LdtokenInstruction instruction);
        void Stloc(StlocInstruction instruction);
        void Starg(StargInstruction instruction);
        void Stobj(StobjInstruction instruction);
        void Stfld(StfldInstruction instruction);
        void Stsfld(StsfldInstruction instruction);
        void Dup(DupInstruction instruction);
        void Pop(PopInstruction instruction);
        void Jmp(JumpInstruction instruction);
        void Call(CallInstruction instruction);
        void Calli(CalliInstruction instruction);
        void Ret(ReturnInstruction instruction);
        void Branch(BranchInstruction instruction);
        void UnaryBranch(UnaryBranchInstruction instruction);
        void BinaryBranch(BinaryBranchInstruction instruction);
        void Switch(SwitchInstruction instruction);
        void BinaryLogic(BinaryLogicInstruction instruction);
        void Shift(ShiftInstruction instruction);
        void Neg(NegInstruction instruction);
        void Not(NotInstruction instruction);
        void Conversion(ConversionInstruction instruction);
        void Callvirt(CallvirtInstruction instruction);
        void Cpobj(CpobjInstruction instruction);
        void Newobj(NewobjInstruction instruction);
        void Castclass(CastclassInstruction instruction);
        void Isinst(IsInstInstruction instruction);
        void Unbox(UnboxInstruction instruction);
        void Throw(ThrowInstruction instruction);
        void Box(BoxInstruction instruction);
        void Newarr(NewarrInstruction instruction);
        void Ldlen(LdlenInstruction instruction);
        void Ldelema(LdelemaInstruction instruction);
        void Ldelem(LdelemInstruction instruction);
        void Stelem(StelemInstruction instruction);
        void UnboxAny(UnboxAnyInstruction instruction);
        void Refanyval(RefanyvalInstruction instruction);
        void UnaryArithmetic(UnaryArithmeticInstruction instruction);
        void Mkrefany(MkrefanyInstruction instruction);
        void ArithmeticOverflow(ArithmeticOverflowInstruction instruction);
        void Endfinally(EndfinallyInstruction instruction);
        void Leave(LeaveInstruction instruction);
        void Arglist(ArglistInstruction instruction);
        void BinaryComparison(BinaryComparisonInstruction instruction);
        void Localalloc(LocalallocInstruction instruction);
        void Endfilter(EndfilterInstruction instruction);
        void InitObj(InitObjInstruction instruction);
        void Cpblk(CpblkInstruction instruction);
        void Initblk(InitblkInstruction instruction);
        void Prefix(PrefixInstruction instruction);
        void Rethrow(RethrowInstruction instruction);
        void Sizeof(SizeofInstruction instruction);
        void Refanytype(RefanytypeInstruction instruction);

        void Add(AddInstruction instruction);
        void Sub(SubInstruction instruction);
        void Mul(MulInstruction instruction);
        void Div(DivInstruction instruction);
        void Rem(RemInstruction instruction);
    }
}
