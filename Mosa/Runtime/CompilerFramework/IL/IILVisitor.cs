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
    public interface IILVisitor<ArgType>
    {
        void Nop(NopInstruction instruction, ArgType arg);
        void Break(BreakInstruction instruction, ArgType arg);
        void Ldarg(LdargInstruction instruction, ArgType arg);
        void Ldarga(LdargaInstruction instruction, ArgType arg);
        void Ldloc(LdlocInstruction instruction, ArgType arg);
        void Ldloca(LdlocaInstruction instruction, ArgType arg);
        void Ldc(LdcInstruction instruction, ArgType arg);
        void Ldobj(LdobjInstruction instruction, ArgType arg);
        void Ldstr(LdstrInstruction instruction, ArgType arg);
        void Ldfld(LdfldInstruction instruction, ArgType arg);
        void Ldflda(LdfldaInstruction instruction, ArgType arg);
        void Ldsfld(LdsfldInstruction instruction, ArgType arg);
        void Ldsflda(LdsfldaInstruction instruction, ArgType arg);
        void Ldftn(LdftnInstruction instruction, ArgType arg);
        void Ldvirtftn(LdvirtftnInstruction instruction, ArgType arg);
        void Ldtoken(LdtokenInstruction instruction, ArgType arg);
        void Stloc(StlocInstruction instruction, ArgType arg);
        void Starg(StargInstruction instruction, ArgType arg);
        void Stobj(StobjInstruction instruction, ArgType arg);
        void Stfld(StfldInstruction instruction, ArgType arg);
        void Stsfld(StsfldInstruction instruction, ArgType arg);
        void Dup(DupInstruction instruction, ArgType arg);
        void Pop(PopInstruction instruction, ArgType arg);
        void Jmp(JumpInstruction instruction, ArgType arg);
        void Call(CallInstruction instruction, ArgType arg);
        void Calli(CalliInstruction instruction, ArgType arg);
        void Ret(ReturnInstruction instruction, ArgType arg);
        void Branch(BranchInstruction instruction, ArgType arg);
        void UnaryBranch(UnaryBranchInstruction instruction, ArgType arg);
        void BinaryBranch(BinaryBranchInstruction instruction, ArgType arg);
        void Switch(SwitchInstruction instruction, ArgType arg);
        void BinaryLogic(BinaryLogicInstruction instruction, ArgType arg);
        void Shift(ShiftInstruction instruction, ArgType arg);
        void Neg(NegInstruction instruction, ArgType arg);
        void Not(NotInstruction instruction, ArgType arg);
        void Conversion(ConversionInstruction instruction, ArgType arg);
        void Callvirt(CallvirtInstruction instruction, ArgType arg);
        void Cpobj(CpobjInstruction instruction, ArgType arg);
        void Newobj(NewobjInstruction instruction, ArgType arg);
        void Castclass(CastclassInstruction instruction, ArgType arg);
        void Isinst(IsInstInstruction instruction, ArgType arg);
        void Unbox(UnboxInstruction instruction, ArgType arg);
        void Throw(ThrowInstruction instruction, ArgType arg);
        void Box(BoxInstruction instruction, ArgType arg);
        void Newarr(NewarrInstruction instruction, ArgType arg);
        void Ldlen(LdlenInstruction instruction, ArgType arg);
        void Ldelema(LdelemaInstruction instruction, ArgType arg);
        void Ldelem(LdelemInstruction instruction, ArgType arg);
        void Stelem(StelemInstruction instruction, ArgType arg);
        void UnboxAny(UnboxAnyInstruction instruction, ArgType arg);
        void Refanyval(RefanyvalInstruction instruction, ArgType arg);
        void UnaryArithmetic(UnaryArithmeticInstruction instruction, ArgType arg);
        void Mkrefany(MkrefanyInstruction instruction, ArgType arg);
        void ArithmeticOverflow(ArithmeticOverflowInstruction instruction, ArgType arg);
        void Endfinally(EndfinallyInstruction instruction, ArgType arg);
        void Leave(LeaveInstruction instruction, ArgType arg);
        void Arglist(ArglistInstruction instruction, ArgType arg);
        void BinaryComparison(BinaryComparisonInstruction instruction, ArgType arg);
        void Localalloc(LocalallocInstruction instruction, ArgType arg);
        void Endfilter(EndfilterInstruction instruction, ArgType arg);
        void InitObj(InitObjInstruction instruction, ArgType arg);
        void Cpblk(CpblkInstruction instruction, ArgType arg);
        void Initblk(InitblkInstruction instruction, ArgType arg);
        void Prefix(PrefixInstruction instruction, ArgType arg);
        void Rethrow(RethrowInstruction instruction, ArgType arg);
        void Sizeof(SizeofInstruction instruction, ArgType arg);
        void Refanytype(RefanytypeInstruction instruction, ArgType arg);

        void Add(AddInstruction instruction, ArgType arg);
        void Sub(SubInstruction instruction, ArgType arg);
        void Mul(MulInstruction instruction, ArgType arg);
        void Div(DivInstruction instruction, ArgType arg);
        void Rem(RemInstruction instruction, ArgType arg);
    }
}
