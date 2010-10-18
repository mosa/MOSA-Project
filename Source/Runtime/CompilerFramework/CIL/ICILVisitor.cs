/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICILVisitor : IVisitor
	{
		/// <summary>
		/// Visitation function for <see cref="Nop"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Nop(Context context);
		/// <summary>
		/// Visitation function for <see cref="Break"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Break(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldarg"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldarg(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldarga"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldarga(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldloc"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldloc(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldloca"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldloca(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldc"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldc(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldobj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldobj(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldstr"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldstr(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldfld"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldfld(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldflda"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldflda(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldsfld"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldsfld(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldsflda"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldsflda(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldftn"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldftn(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldvirtftn"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldvirtftn(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldtoken"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldtoken(Context context);
		/// <summary>
		/// Visitation function for <see cref="Stloc"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stloc(Context context);
		/// <summary>
		/// Visitation function for <see cref="Starg"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Starg(Context context);
		/// <summary>
		/// Visitation function for <see cref="Stobj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stobj(Context context);
		/// <summary>
		/// Visitation function for <see cref="Stfld"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stfld(Context context);
		/// <summary>
		/// Visitation function for <see cref="Stsfld"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stsfld(Context context);
		/// <summary>
		/// Visitation function for <see cref="Dup"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Dup(Context context);
		/// <summary>
		/// Visitation function for <see cref="Pop"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Pop(Context context);
		/// <summary>
		/// Visitation function for <see cref="Jmp"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Jmp(Context context);
		/// <summary>
		/// Visitation function for <see cref="Call"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Call(Context context);
		/// <summary>
		/// Visitation function for <see cref="Calli"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Calli(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ret"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ret(Context context);
		/// <summary>
		/// Visitation function for <see cref="Branch"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Branch(Context context);
		/// <summary>
		/// Visitation function for <see cref="UnaryBranch"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void UnaryBranch(Context context);
		/// <summary>
		/// Visitation function for <see cref="BinaryBranch"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void BinaryBranch(Context context);
		/// <summary>
		/// Visitation function for <see cref="Switch"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Switch(Context context);
		/// <summary>
		/// Visitation function for <see cref="BinaryLogic"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void BinaryLogic(Context context);
		/// <summary>
		/// Visitation function for <see cref="Shift"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Shift(Context context);
		/// <summary>
		/// Visitation function for <see cref="Neg"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Neg(Context context);
		/// <summary>
		/// Visitation function for <see cref="Not"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Not(Context context);
		/// <summary>
		/// Visitation function for <see cref="Conversion"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Conversion(Context context);
		/// <summary>
		/// Visitation function for <see cref="Callvirt"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Callvirt(Context context);
		/// <summary>
		/// Visitation function for <see cref="Cpobj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cpobj(Context context);
		/// <summary>
		/// Visitation function for <see cref="Newobj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Newobj(Context context);
		/// <summary>
		/// Visitation function for <see cref="Castclass"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Castclass(Context context);
		/// <summary>
		/// Visitation function for <see cref="Isinst"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Isinst(Context context);
		/// <summary>
		/// Visitation function for <see cref="Unbox"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Unbox(Context context);
		/// <summary>
		/// Visitation function for <see cref="Throw"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Throw(Context context);
		/// <summary>
		/// Visitation function for <see cref="Box"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Box(Context context);
		/// <summary>
		/// Visitation function for <see cref="Newarr"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Newarr(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldlen"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldlen(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldelema"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldelema(Context context);
		/// <summary>
		/// Visitation function for <see cref="Ldelem"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldelem(Context context);
		/// <summary>
		/// Visitation function for <see cref="Stelem"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stelem(Context context);
		/// <summary>
		/// Visitation function for <see cref="UnboxAny"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void UnboxAny(Context context);
		/// <summary>
		/// Visitation function for <see cref="Refanyval"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Refanyval(Context context);
		/// <summary>
		/// Visitation function for <see cref="UnaryArithmetic"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void UnaryArithmetic(Context context);
		/// <summary>
		/// Visitation function for <see cref="Mkrefany"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Mkrefany(Context context);
		/// <summary>
		/// Visitation function for <see cref="ArithmeticOverflow"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void ArithmeticOverflow(Context context);
		/// <summary>
		/// Visitation function for <see cref="Endfinally"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Endfinally(Context context);
		/// <summary>
		/// Visitation function for <see cref="Leave"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Leave(Context context);
		/// <summary>
		/// Visitation function for <see cref="Arglist"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Arglist(Context context);
		/// <summary>
		/// Visitation function for <see cref="BinaryComparison"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void BinaryComparison(Context context);
		/// <summary>
		/// Visitation function for <see cref="Localalloc"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Localalloc(Context context);
		/// <summary>
		/// Visitation function for <see cref="Endfilter"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Endfilter(Context context);
		/// <summary>
		/// Visitation function for <see cref="InitObj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void InitObj(Context context);
		/// <summary>
		/// Visitation function for <see cref="Cpblk"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cpblk(Context context);
		/// <summary>
		/// Visitation function for <see cref="Initblk"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Initblk(Context context);
		/// <summary>
		/// Visitation function for <see cref="Prefix"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Prefix(Context context);
		/// <summary>
		/// Visitation function for <see cref="Rethrow"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Rethrow(Context context);
		/// <summary>
		/// Visitation function for <see cref="Sizeof"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Sizeof(Context context);
		/// <summary>
		/// Visitation function for <see cref="Refanytype"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Refanytype(Context context);
		/// <summary>
		/// Visitation function for <see cref="Add"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Add(Context context);
		/// <summary>
		/// Visitation function for <see cref="Sub"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Sub(Context context);
		/// <summary>
		/// Visitation function for <see cref="Mul"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Mul(Context context);
		/// <summary>
		/// Visitation function for <see cref="Div"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Div(Context context);
		/// <summary>
		/// Visitation function for <see cref="Rem"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void Rem(Context context);
	}
}
