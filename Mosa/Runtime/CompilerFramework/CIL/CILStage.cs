/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	///
	/// </summary>
	public class CILStage : CodeTransformationStage, ICILVisitor
	{

		#region Override Methods

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name { get { return string.Empty; } }

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{ }

		#endregion // Override Methods

		#region ICILVisitor

		/// <summary>
		/// Visitation function for <see cref="Nop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Nop(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Break"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Break(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldarg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldarg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldarga"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldarga(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldloca"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldloca(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldstr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldstr(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldflda(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldsfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldsflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldsflda(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldftn(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldvirtftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldvirtftn(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldtoken"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldtoken(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Starg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Starg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stsfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Dup"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Dup(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Pop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Pop(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Jmp"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Jmp(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Call"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Call(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Calli"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Calli(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ret"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ret(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Branch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Branch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="UnaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void UnaryBranch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="BinaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void BinaryBranch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Switch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Switch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="BinaryLogic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void BinaryLogic(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Shift"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Shift(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Neg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Neg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Not"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Not(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Conversion"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Conversion(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Callvirt"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Callvirt(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Cpobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Cpobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Newobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Newobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Castclass"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Castclass(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Isinst"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Isinst(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Unbox"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Unbox(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Throw"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Throw(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Box"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Box(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Newarr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Newarr(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldlen"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldlen(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldelema"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldelema(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="UnboxAny"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void UnboxAny(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Refanyval"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Refanyval(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="UnaryArithmetic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void UnaryArithmetic(Context ctx) { }

		/// <summary>
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Mkrefany(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ArithmeticOverflow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void ArithmeticOverflow(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Endfinally"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Endfinally(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Leave"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Leave(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Arglist"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Arglist(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="BinaryComparison"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void BinaryComparison(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Localalloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Localalloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Endfilter"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Endfilter(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="InitObj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void InitObj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Cpblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Cpblk(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Initblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Initblk(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Prefix"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Prefix(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Rethrow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Rethrow(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Sizeof"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Sizeof(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Refanytype"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Refanytype(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Add"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Add(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Sub"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Sub(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Mul"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Mul(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Div"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Div(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Rem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Rem(Context ctx) { }

		#endregion // ICILVisitor
	}
}
