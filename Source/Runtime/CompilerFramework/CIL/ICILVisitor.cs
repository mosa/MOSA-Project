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
		/// Visitation function for Nop instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Nop(Context context);
		/// <summary>
		/// Visitation function for Break instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Break(Context context);
		/// <summary>
		/// Visitation function for Ldarg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldarg(Context context);
		/// <summary>
		/// Visitation function for Ldarga instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldarga(Context context);
		/// <summary>
		/// Visitation function for Ldloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldloc(Context context);
		/// <summary>
		/// Visitation function for Ldloca instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldloca(Context context);
		/// <summary>
		/// Visitation function for Ldc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldc(Context context);
		/// <summary>
		/// Visitation function for Ldobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldobj(Context context);
		/// <summary>
		/// Visitation function for Ldstr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldstr(Context context);
		/// <summary>
		/// Visitation function for Ldfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldfld(Context context);
		/// <summary>
		/// Visitation function for Ldflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldflda(Context context);
		/// <summary>
		/// Visitation function for Ldsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldsfld(Context context);
		/// <summary>
		/// Visitation function for Ldsflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldsflda(Context context);
		/// <summary>
		/// Visitation function for Ldftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldftn(Context context);
		/// <summary>
		/// Visitation function for Ldvirtftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldvirtftn(Context context);
		/// <summary>
		/// Visitation function for Ldtoken instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldtoken(Context context);
		/// <summary>
		/// Visitation function for Stloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stloc(Context context);
		/// <summary>
		/// Visitation function for Starg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Starg(Context context);
		/// <summary>
		/// Visitation function for Stobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stobj(Context context);
		/// <summary>
		/// Visitation function for Stfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stfld(Context context);
		/// <summary>
		/// Visitation function for Stsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stsfld(Context context);
		/// <summary>
		/// Visitation function for Dup instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Dup(Context context);
		/// <summary>
		/// Visitation function for Pop instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Pop(Context context);
		/// <summary>
		/// Visitation function for Jmp instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Jmp(Context context);
		/// <summary>
		/// Visitation function for Call instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Call(Context context);
		/// <summary>
		/// Visitation function for Calli instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Calli(Context context);
		/// <summary>
		/// Visitation function for Ret instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ret(Context context);
		/// <summary>
		/// Visitation function for Branch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Branch(Context context);
		/// <summary>
		/// Visitation function for UnaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void UnaryBranch(Context context);
		/// <summary>
		/// Visitation function for BinaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void BinaryBranch(Context context);
		/// <summary>
		/// Visitation function for Switch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Switch(Context context);
		/// <summary>
		/// Visitation function for BinaryLogic instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void BinaryLogic(Context context);
		/// <summary>
		/// Visitation function for Shift instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Shift(Context context);
		/// <summary>
		/// Visitation function for Neg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Neg(Context context);
		/// <summary>
		/// Visitation function for Not instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Not(Context context);
		/// <summary>
		/// Visitation function for Conversion instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Conversion(Context context);
		/// <summary>
		/// Visitation function for Callvirt instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Callvirt(Context context);
		/// <summary>
		/// Visitation function for Cpobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cpobj(Context context);
		/// <summary>
		/// Visitation function for Newobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Newobj(Context context);
		/// <summary>
		/// Visitation function for Castclass instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Castclass(Context context);
		/// <summary>
		/// Visitation function for Isinst instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Isinst(Context context);
		/// <summary>
		/// Visitation function for Unbox instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Unbox(Context context);
		/// <summary>
		/// Visitation function for Throw instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Throw(Context context);
		/// <summary>
		/// Visitation function for Box instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Box(Context context);
		/// <summary>
		/// Visitation function for Newarr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Newarr(Context context);
		/// <summary>
		/// Visitation function for Ldlen instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldlen(Context context);
		/// <summary>
		/// Visitation function for Ldelema instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldelema(Context context);
		/// <summary>
		/// Visitation function for Ldelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Ldelem(Context context);
		/// <summary>
		/// Visitation function for Stelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Stelem(Context context);
		/// <summary>
		/// Visitation function for UnboxAny instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void UnboxAny(Context context);
		/// <summary>
		/// Visitation function for Refanyval instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Refanyval(Context context);
		/// <summary>
		/// Visitation function for UnaryArithmetic instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void UnaryArithmetic(Context context);
		/// <summary>
		/// Visitation function for Mkrefany instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Mkrefany(Context context);
		/// <summary>
		/// Visitation function for ArithmeticOverflow instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void ArithmeticOverflow(Context context);
		/// <summary>
		/// Visitation function for Endfinally instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Endfinally(Context context);
		/// <summary>
		/// Visitation function for Leave instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Leave(Context context);
		/// <summary>
		/// Visitation function for Arglist instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Arglist(Context context);
		/// <summary>
		/// Visitation function for BinaryComparison instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void BinaryComparison(Context context);
		/// <summary>
		/// Visitation function for Localalloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Localalloc(Context context);
		/// <summary>
		/// Visitation function for Endfilter instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Endfilter(Context context);
		/// <summary>
		/// Visitation function for InitObj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void InitObj(Context context);
		/// <summary>
		/// Visitation function for Cpblk instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Cpblk(Context context);
		/// <summary>
		/// Visitation function for Initblk instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Initblk(Context context);
		/// <summary>
		/// Visitation function for Prefix instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Prefix(Context context);
		/// <summary>
		/// Visitation function for Rethrow instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Rethrow(Context context);
		/// <summary>
		/// Visitation function for Sizeof instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Sizeof(Context context);
		/// <summary>
		/// Visitation function for Refanytype instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Refanytype(Context context);
		/// <summary>
		/// Visitation function for Add instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Add(Context context);
		/// <summary>
		/// Visitation function for Sub instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Sub(Context context);
		/// <summary>
		/// Visitation function for Mul instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Mul(Context context);
		/// <summary>
		/// Visitation function for Div instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Div(Context context);
		/// <summary>
		/// Visitation function for Rem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void Rem(Context context);
	}
}
