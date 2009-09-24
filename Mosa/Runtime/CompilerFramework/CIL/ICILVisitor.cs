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
	public interface ICILVisitor
	{
		/// <summary>
		/// Visitation function for <see cref="Nop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Nop(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Break"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Break(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldarg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldarg(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldarga"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldarga(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldloc(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldloca"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldloca(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldc(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldobj(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldstr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldstr(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldfld(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldflda(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldsfld(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldsflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldsflda(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldftn(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldvirtftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldvirtftn(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldtoken"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldtoken(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Stloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stloc(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Starg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Starg(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Stobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stobj(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Stfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stfld(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Stsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stsfld(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Dup"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Dup(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Pop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Pop(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Jmp"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Jmp(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Call"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Call(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Calli"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Calli(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ret"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ret(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Branch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Branch(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="UnaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void UnaryBranch(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="BinaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void BinaryBranch(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Switch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Switch(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="BinaryLogic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void BinaryLogic(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Shift"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Shift(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Neg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Neg(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Not"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Not(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Conversion"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Conversion(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Callvirt"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Callvirt(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Cpobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Cpobj(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Newobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Newobj(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Castclass"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Castclass(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Isinst"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Isinst(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Unbox"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Unbox(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Throw"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Throw(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Box"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Box(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Newarr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Newarr(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldlen"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldlen(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldelema"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldelema(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Ldelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Ldelem(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Stelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Stelem(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="UnboxAny"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void UnboxAny(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Refanyval"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Refanyval(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="UnaryArithmetic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void UnaryArithmetic(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Mkrefany"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Mkrefany(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="ArithmeticOverflow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ArithmeticOverflow(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Endfinally"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Endfinally(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Leave"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Leave(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Arglist"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Arglist(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="BinaryComparison"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void BinaryComparison(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Localalloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Localalloc(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Endfilter"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Endfilter(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="InitObj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void InitObj(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Cpblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Cpblk(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Initblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Initblk(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Prefix"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Prefix(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Rethrow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Rethrow(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Sizeof"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Sizeof(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Refanytype"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Refanytype(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Add"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Add(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Sub"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Sub(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Mul"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Mul(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Div"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Div(Context ctx);
		/// <summary>
		/// Visitation function for <see cref="Rem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Rem(Context ctx);
	}
}
