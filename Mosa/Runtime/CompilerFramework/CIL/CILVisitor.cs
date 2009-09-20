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

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public interface CILVisitor
	{
		/// <summary>
		/// Nops the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Nop(Context ctx);
		/// <summary>
		/// Breaks the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Break(Context ctx);
		/// <summary>
		/// Ldargs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldarg(Context ctx);
		/// <summary>
		/// Ldargas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldarga(Context ctx);
		/// <summary>
		/// Ldlocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldloc(Context ctx);
		/// <summary>
		/// Ldlocas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldloca(Context ctx);
		/// <summary>
		/// LDCs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldc(Context ctx);
		/// <summary>
		/// Ldobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldobj(Context ctx);
		/// <summary>
		/// LDSTRs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldstr(Context ctx);
		/// <summary>
		/// LDFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldfld(Context ctx);
		/// <summary>
		/// Ldfldas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldflda(Context ctx);
		/// <summary>
		/// LDSFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldsfld(Context ctx);
		/// <summary>
		/// Ldsfldas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldsflda(Context ctx);
		/// <summary>
		/// LDFTNs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldftn(Context ctx);
		/// <summary>
		/// Ldvirtftns the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldvirtftn(Context ctx);
		/// <summary>
		/// Ldtokens the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldtoken(Context ctx);
		/// <summary>
		/// Stlocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stloc(Context ctx);
		/// <summary>
		/// Stargs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Starg(Context ctx);
		/// <summary>
		/// Stobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stobj(Context ctx);
		/// <summary>
		/// STFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stfld(Context ctx);
		/// <summary>
		/// STSFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stsfld(Context ctx);
		/// <summary>
		/// Dups the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Dup(Context ctx);
		/// <summary>
		/// Pops the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Pop(Context ctx);
		/// <summary>
		/// JMPs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Jmp(Context ctx);
		/// <summary>
		/// Calls the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Call(Context ctx);
		/// <summary>
		/// Callis the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Calli(Context ctx);
		/// <summary>
		/// Rets the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ret(Context ctx);
		/// <summary>
		/// Brancs the dh.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Branch(Context ctx);
		/// <summary>
		/// Unaries the branch.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void UnaryBranch(Context ctx);
		/// <summary>
		/// Binaries the branch.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void BinaryBranch(Context ctx);
		/// <summary>
		/// Switches the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Switch(Context ctx);
		/// <summary>
		/// Binaries the logic.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void BinaryLogic(Context ctx);
		/// <summary>
		/// Shifts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Shift(Context ctx);
		/// <summary>
		/// Negs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Neg(Context ctx);
		/// <summary>
		/// Nots the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Not(Context ctx);
		/// <summary>
		/// Conversions the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Conversion(Context ctx);
		/// <summary>
		/// Callvirts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Callvirt(Context ctx);
		/// <summary>
		/// Cpobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Cpobj(Context ctx);
		/// <summary>
		/// Newobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Newobj(Context ctx);
		/// <summary>
		/// Castclasses the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Castclass(Context ctx);
		/// <summary>
		/// Isinsts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Isinst(Context ctx);
		/// <summary>
		/// Unboxes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Unbox(Context ctx);
		/// <summary>
		/// Throws the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Throw(Context ctx);
		/// <summary>
		/// Boxes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Box(Context ctx);
		/// <summary>
		/// Newarrs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Newarr(Context ctx);
		/// <summary>
		/// Ldlens the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldlen(Context ctx);
		/// <summary>
		/// Ldelemas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldelema(Context ctx);
		/// <summary>
		/// Ldelems the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldelem(Context ctx);
		/// <summary>
		/// Stelems the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stelem(Context ctx);
		/// <summary>
		/// Unboxes any.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void UnboxAny(Context ctx);
		/// <summary>
		/// Refanyvals the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Refanyval(Context ctx);
		/// <summary>
		/// Unaries the arithmetic.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void UnaryArithmetic(Context ctx);
		/// <summary>
		/// Mkrefanies the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Mkrefany(Context ctx);
		/// <summary>
		/// Arithmetics the overflow.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ArithmeticOverflow(Context ctx);
		/// <summary>
		/// Endfinallies the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Endfinally(Context ctx);
		/// <summary>
		/// Leaves the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Leave(Context ctx);
		/// <summary>
		/// Arglists the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Arglist(Context ctx);
		/// <summary>
		/// Binaries the comparison.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void BinaryComparison(Context ctx);
		/// <summary>
		/// Localallocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Localalloc(Context ctx);
		/// <summary>
		/// Endfilters the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Endfilter(Context ctx);
		/// <summary>
		/// Inits the obj.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void InitObj(Context ctx);
		/// <summary>
		/// CPBLKs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Cpblk(Context ctx);
		/// <summary>
		/// Initblks the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Initblk(Context ctx);
		/// <summary>
		/// Prefixes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Prefix(Context ctx);
		/// <summary>
		/// Rethrows the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Rethrow(Context ctx);
		/// <summary>
		/// Sizeofs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Sizeof(Context ctx);
		/// <summary>
		/// Refanytypes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Refanytype(Context ctx);
		/// <summary>
		/// Adds the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Add(Context ctx);
		/// <summary>
		/// Subs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Sub(Context ctx);
		/// <summary>
		/// Muls the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Mul(Context ctx);
		/// <summary>
		/// Divs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Div(Context ctx);
		/// <summary>
		/// Rems the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Rem(Context ctx);
	}
}
