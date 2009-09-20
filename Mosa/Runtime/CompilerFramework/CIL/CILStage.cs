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

		#region Methods

		/// <summary>
		/// Nops the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Nop(Context ctx)
		{
		}

		/// <summary>
		/// Breaks the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Break(Context ctx)
		{
		}

		/// <summary>
		/// Ldargs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldarg(Context ctx)
		{
		}

		/// <summary>
		/// Ldargas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldarga(Context ctx)
		{
		}

		/// <summary>
		/// Ldlocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldloc(Context ctx)
		{
		}

		/// <summary>
		/// Ldlocas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldloca(Context ctx)
		{
		}

		/// <summary>
		/// LDCs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldc(Context ctx)
		{
		}

		/// <summary>
		/// Ldobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldobj(Context ctx)
		{
		}

		/// <summary>
		/// LDSTRs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldstr(Context ctx)
		{
		}

		/// <summary>
		/// LDFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldfld(Context ctx)
		{
		}

		/// <summary>
		/// Ldfldas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldflda(Context ctx)
		{
		}

		/// <summary>
		/// LDSFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldsfld(Context ctx)
		{
		}

		/// <summary>
		/// Ldsfldas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldsflda(Context ctx)
		{
		}

		/// <summary>
		/// LDFTNs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldftn(Context ctx)
		{
		}

		/// <summary>
		/// Ldvirtftns the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldvirtftn(Context ctx)
		{
		}

		/// <summary>
		/// Ldtokens the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldtoken(Context ctx)
		{
		}

		/// <summary>
		/// Stlocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stloc(Context ctx)
		{
		}

		/// <summary>
		/// Stargs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Starg(Context ctx)
		{
		}

		/// <summary>
		/// Stobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stobj(Context ctx)
		{
		}

		/// <summary>
		/// STFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stfld(Context ctx)
		{
		}

		/// <summary>
		/// STSFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stsfld(Context ctx)
		{
		}

		/// <summary>
		/// Dups the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Dup(Context ctx)
		{
		}

		/// <summary>
		/// Pops the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Pop(Context ctx)
		{
		}

		/// <summary>
		/// JMPs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Jmp(Context ctx)
		{
		}

		/// <summary>
		/// Calls the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Call(Context ctx)
		{
		}

		/// <summary>
		/// Callis the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Calli(Context ctx)
		{
		}

		/// <summary>
		/// Rets the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ret(Context ctx)
		{
		}

		/// <summary>
		/// Brancs the dh.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Branch(Context ctx)
		{
		}

		/// <summary>
		/// Unaries the branch.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void UnaryBranch(Context ctx)
		{
		}

		/// <summary>
		/// Binaries the branch.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void BinaryBranch(Context ctx)
		{
		}

		/// <summary>
		/// Switches the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Switch(Context ctx)
		{
		}

		/// <summary>
		/// Binaries the logic.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void BinaryLogic(Context ctx)
		{
		}

		/// <summary>
		/// Shifts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Shift(Context ctx)
		{
		}

		/// <summary>
		/// Negs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Neg(Context ctx)
		{
		}

		/// <summary>
		/// Nots the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Not(Context ctx)
		{
		}

		/// <summary>
		/// Conversions the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Conversion(Context ctx)
		{
		}

		/// <summary>
		/// Callvirts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Callvirt(Context ctx)
		{
		}

		/// <summary>
		/// Cpobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Cpobj(Context ctx)
		{
		}

		/// <summary>
		/// Newobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Newobj(Context ctx)
		{
		}

		/// <summary>
		/// Castclasses the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Castclass(Context ctx)
		{
		}

		/// <summary>
		/// Isinsts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Isinst(Context ctx)
		{
		}

		/// <summary>
		/// Unboxes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Unbox(Context ctx)
		{
		}

		/// <summary>
		/// Throws the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Throw(Context ctx)
		{
		}

		/// <summary>
		/// Boxes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Box(Context ctx)
		{
		}

		/// <summary>
		/// Newarrs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Newarr(Context ctx)
		{
		}

		/// <summary>
		/// Ldlens the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldlen(Context ctx)
		{
		}

		/// <summary>
		/// Ldelemas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldelema(Context ctx)
		{
		}

		/// <summary>
		/// Ldelems the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Ldelem(Context ctx)
		{
		}

		/// <summary>
		/// Stelems the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Stelem(Context ctx)
		{
		}

		/// <summary>
		/// Unboxes any.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void UnboxAny(Context ctx)
		{
		}

		/// <summary>
		/// Refanyvals the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Refanyval(Context ctx)
		{
		}

		/// <summary>
		/// Unaries the arithmetic.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void UnaryArithmetic(Context ctx)
		{
		}

		/// <summary>
		/// Mkrefanies the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Mkrefany(Context ctx)
		{
		}

		/// <summary>
		/// Arithmetics the overflow.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void ArithmeticOverflow(Context ctx)
		{
		}

		/// <summary>
		/// Endfinallies the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Endfinally(Context ctx)
		{
		}

		/// <summary>
		/// Leaves the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Leave(Context ctx)
		{
		}

		/// <summary>
		/// Arglists the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Arglist(Context ctx)
		{
		}

		/// <summary>
		/// Binaries the comparison.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void BinaryComparison(Context ctx)
		{
		}

		/// <summary>
		/// Localallocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Localalloc(Context ctx)
		{
		}

		/// <summary>
		/// Endfilters the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Endfilter(Context ctx)
		{
		}

		/// <summary>
		/// Inits the obj.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void InitObj(Context ctx)
		{
		}

		/// <summary>
		/// CPBLKs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Cpblk(Context ctx)
		{
		}

		/// <summary>
		/// Initblks the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Initblk(Context ctx)
		{
		}

		/// <summary>
		/// Prefixes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Prefix(Context ctx)
		{
		}

		/// <summary>
		/// Rethrows the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Rethrow(Context ctx)
		{
		}

		/// <summary>
		/// Sizeofs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Sizeof(Context ctx)
		{
		}

		/// <summary>
		/// Refanytypes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Refanytype(Context ctx)
		{
		}

		/// <summary>
		/// Adds the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Add(Context ctx)
		{
		}

		/// <summary>
		/// Subs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Sub(Context ctx)
		{
		}

		/// <summary>
		/// Muls the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Mul(Context ctx)
		{
		}

		/// <summary>
		/// Divs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Div(Context ctx)
		{
		}

		/// <summary>
		/// Rems the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public virtual void Rem(Context ctx)
		{
		}

		#endregion // Methods
	}
}
