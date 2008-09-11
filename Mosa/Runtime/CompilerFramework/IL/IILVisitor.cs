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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ArgType">The type of the rg type.</typeparam>
    public interface IILVisitor<ArgType>
    {
        /// <summary>
        /// Nops the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Nop(NopInstruction instruction, ArgType arg);
        /// <summary>
        /// Breaks the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Break(BreakInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldargs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldarg(LdargInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldargas the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldarga(LdargaInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldlocs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldloc(LdlocInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldlocas the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldloca(LdlocaInstruction instruction, ArgType arg);
        /// <summary>
        /// LDCs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldc(LdcInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldobjs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldobj(LdobjInstruction instruction, ArgType arg);
        /// <summary>
        /// LDSTRs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldstr(LdstrInstruction instruction, ArgType arg);
        /// <summary>
        /// LDFLDs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldfld(LdfldInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldfldas the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldflda(LdfldaInstruction instruction, ArgType arg);
        /// <summary>
        /// LDSFLDs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldsfld(LdsfldInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldsfldas the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldsflda(LdsfldaInstruction instruction, ArgType arg);
        /// <summary>
        /// LDFTNs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldftn(LdftnInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldvirtftns the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldvirtftn(LdvirtftnInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldtokens the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldtoken(LdtokenInstruction instruction, ArgType arg);
        /// <summary>
        /// Stlocs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Stloc(StlocInstruction instruction, ArgType arg);
        /// <summary>
        /// Stargs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Starg(StargInstruction instruction, ArgType arg);
        /// <summary>
        /// Stobjs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Stobj(StobjInstruction instruction, ArgType arg);
        /// <summary>
        /// STFLDs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Stfld(StfldInstruction instruction, ArgType arg);
        /// <summary>
        /// STSFLDs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Stsfld(StsfldInstruction instruction, ArgType arg);
        /// <summary>
        /// Dups the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Dup(DupInstruction instruction, ArgType arg);
        /// <summary>
        /// Pops the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Pop(PopInstruction instruction, ArgType arg);
        /// <summary>
        /// JMPs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Jmp(JumpInstruction instruction, ArgType arg);
        /// <summary>
        /// Calls the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Call(CallInstruction instruction, ArgType arg);
        /// <summary>
        /// Callis the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Calli(CalliInstruction instruction, ArgType arg);
        /// <summary>
        /// Rets the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ret(ReturnInstruction instruction, ArgType arg);
        /// <summary>
        /// Brancs the dh.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Branch(BranchInstruction instruction, ArgType arg);
        /// <summary>
        /// Unaries the branch.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void UnaryBranch(UnaryBranchInstruction instruction, ArgType arg);
        /// <summary>
        /// Binaries the branch.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void BinaryBranch(BinaryBranchInstruction instruction, ArgType arg);
        /// <summary>
        /// Switches the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Switch(SwitchInstruction instruction, ArgType arg);
        /// <summary>
        /// Binaries the logic.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void BinaryLogic(BinaryLogicInstruction instruction, ArgType arg);
        /// <summary>
        /// Shifts the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Shift(ShiftInstruction instruction, ArgType arg);
        /// <summary>
        /// Negs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Neg(NegInstruction instruction, ArgType arg);
        /// <summary>
        /// Nots the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Not(NotInstruction instruction, ArgType arg);
        /// <summary>
        /// Conversions the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Conversion(ConversionInstruction instruction, ArgType arg);
        /// <summary>
        /// Callvirts the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Callvirt(CallvirtInstruction instruction, ArgType arg);
        /// <summary>
        /// Cpobjs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Cpobj(CpobjInstruction instruction, ArgType arg);
        /// <summary>
        /// Newobjs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Newobj(NewobjInstruction instruction, ArgType arg);
        /// <summary>
        /// Castclasses the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Castclass(CastclassInstruction instruction, ArgType arg);
        /// <summary>
        /// Isinsts the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Isinst(IsInstInstruction instruction, ArgType arg);
        /// <summary>
        /// Unboxes the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Unbox(UnboxInstruction instruction, ArgType arg);
        /// <summary>
        /// Throws the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Throw(ThrowInstruction instruction, ArgType arg);
        /// <summary>
        /// Boxes the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Box(BoxInstruction instruction, ArgType arg);
        /// <summary>
        /// Newarrs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Newarr(NewarrInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldlens the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldlen(LdlenInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldelemas the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldelema(LdelemaInstruction instruction, ArgType arg);
        /// <summary>
        /// Ldelems the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Ldelem(LdelemInstruction instruction, ArgType arg);
        /// <summary>
        /// Stelems the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Stelem(StelemInstruction instruction, ArgType arg);
        /// <summary>
        /// Unboxes any.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void UnboxAny(UnboxAnyInstruction instruction, ArgType arg);
        /// <summary>
        /// Refanyvals the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Refanyval(RefanyvalInstruction instruction, ArgType arg);
        /// <summary>
        /// Unaries the arithmetic.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void UnaryArithmetic(UnaryArithmeticInstruction instruction, ArgType arg);
        /// <summary>
        /// Mkrefanies the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Mkrefany(MkrefanyInstruction instruction, ArgType arg);
        /// <summary>
        /// Arithmetics the overflow.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void ArithmeticOverflow(ArithmeticOverflowInstruction instruction, ArgType arg);
        /// <summary>
        /// Endfinallies the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Endfinally(EndfinallyInstruction instruction, ArgType arg);
        /// <summary>
        /// Leaves the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Leave(LeaveInstruction instruction, ArgType arg);
        /// <summary>
        /// Arglists the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Arglist(ArglistInstruction instruction, ArgType arg);
        /// <summary>
        /// Binaries the comparison.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void BinaryComparison(BinaryComparisonInstruction instruction, ArgType arg);
        /// <summary>
        /// Localallocs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Localalloc(LocalallocInstruction instruction, ArgType arg);
        /// <summary>
        /// Endfilters the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Endfilter(EndfilterInstruction instruction, ArgType arg);
        /// <summary>
        /// Inits the obj.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void InitObj(InitObjInstruction instruction, ArgType arg);
        /// <summary>
        /// CPBLKs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Cpblk(CpblkInstruction instruction, ArgType arg);
        /// <summary>
        /// Initblks the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Initblk(InitblkInstruction instruction, ArgType arg);
        /// <summary>
        /// Prefixes the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Prefix(PrefixInstruction instruction, ArgType arg);
        /// <summary>
        /// Rethrows the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Rethrow(RethrowInstruction instruction, ArgType arg);
        /// <summary>
        /// Sizeofs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Sizeof(SizeofInstruction instruction, ArgType arg);
        /// <summary>
        /// Refanytypes the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Refanytype(RefanytypeInstruction instruction, ArgType arg);

        /// <summary>
        /// Adds the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Add(AddInstruction instruction, ArgType arg);
        /// <summary>
        /// Subs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Sub(SubInstruction instruction, ArgType arg);
        /// <summary>
        /// Muls the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Mul(MulInstruction instruction, ArgType arg);
        /// <summary>
        /// Divs the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Div(DivInstruction instruction, ArgType arg);
        /// <summary>
        /// Rems the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arg.</param>
        void Rem(RemInstruction instruction, ArgType arg);
    }
}
