/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
 */

using System;
using Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs IR constant folding of arithmetic instructions to optimize
	/// the code down to fewer calculations.
	/// </summary>
	public sealed class StrengthReductionStage : CodeTransformationStage, IMethodCompilerStage,
		ICILVisitor,
		IR.IIRVisitor<Context>
	{

		/// <summary>
		/// Determines whether the value is zero.
		/// </summary>
		/// <param name="cilElementType">Type of the cil element.</param>
		/// <param name="constantOperand">The constant operand.</param>
		/// <returns>
		/// 	<c>true</c> if the value is zero; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueZero(Metadata.CilElementType cilElementType, ConstantOperand constantOperand)
		{
			switch (cilElementType) {
				case Metadata.CilElementType.Char:
					goto case Metadata.CilElementType.U2;
				case Metadata.CilElementType.U1:
					return (byte)(constantOperand.Value) == 0;
				case Metadata.CilElementType.U2:
					return (ushort)(constantOperand.Value) == 0;
				case Metadata.CilElementType.U4:
					return (int)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I1:
					return (sbyte)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I2:
					return (short)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I4:
					return (int)(constantOperand.Value) == 0;
				case Metadata.CilElementType.R4:
					return (float)(constantOperand.Value) == 0;
				case Metadata.CilElementType.R8:
					return (double)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I:
					goto case Metadata.CilElementType.I4;
				case Metadata.CilElementType.U:
					goto case Metadata.CilElementType.U4;
				default:
					goto case Metadata.CilElementType.I4;
			}
		}

		/// <summary>
		/// Determines whether the value is one.
		/// </summary>
		/// <param name="cilElementType">Type of the cil element.</param>
		/// <param name="constantOperand">The constant operand.</param>
		/// <returns>
		/// 	<c>true</c> if the value is one; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueOne(Metadata.CilElementType cilElementType, ConstantOperand constantOperand)
		{
			switch (cilElementType) {
				case Metadata.CilElementType.Char:
					goto case Metadata.CilElementType.U2;
				case Metadata.CilElementType.U1:
					return (byte)(constantOperand.Value) == 1;
				case Metadata.CilElementType.U2:
					return (ushort)(constantOperand.Value) == 1;
				case Metadata.CilElementType.U4:
					return (int)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I1:
					return (sbyte)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I2:
					return (short)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I4:
					return (int)(constantOperand.Value) == 1;
				case Metadata.CilElementType.R4:
					return (float)(constantOperand.Value) == 1;
				case Metadata.CilElementType.R8:
					return (double)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I:
					goto case Metadata.CilElementType.I4;
				case Metadata.CilElementType.U:
					goto case Metadata.CilElementType.U4;
				default:
					goto case Metadata.CilElementType.I4;
			}
		}

		/// <summary>
		/// Folds multiplication when one of the constants is zero
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Mul(Context ctx)
		{
			bool multiplyByZero = false;

			if (ctx.Operand1 is ConstantOperand)
				if (IsValueZero(ctx.Result.Type.Type, ctx.Operand1 as ConstantOperand))
					multiplyByZero = true;

			if (ctx.Operand2 is ConstantOperand)
				if (IsValueZero(ctx.Result.Type.Type, ctx.Operand2 as ConstantOperand))
					multiplyByZero = true;

			if (multiplyByZero) {
				if (ctx.Result.Type.Type == Metadata.CilElementType.R4)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, 0)));
				else if (ctx.Result.Type.Type == Metadata.CilElementType.R8)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, 0)));
				else
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, 0)));

				return;
			}

			if (ctx.Operand1 is ConstantOperand)
				if (IsValueOne(ctx.Result.Type.Type, ctx.Operand1 as ConstantOperand)) {
					Replace(ctx, new IR.MoveInstruction(ctx.Result, ctx.Operand2));
					return;
				}

			if (ctx.Operand2 is ConstantOperand)
				if (IsValueOne(ctx.Result.Type.Type, ctx.Operand2 as ConstantOperand)) {
					Replace(ctx, new IR.MoveInstruction(ctx.Result, ctx.Operand1));
					return;
				}

		}

		/// <summary>
		/// Folds divisions with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Div(Context ctx)
		{

		}

		/// <summary>
		/// Folds the remainder of 2 constants
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		public void Rem(Context ctx)
		{

		}

		// FIXME PG
		//void IInstructionVisitor<Context>.Visit(Context ctx)
		//{
		//}

		/// <summary>
		/// Folds additions with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Add(Context ctx)
		{

		}

		/// <summary>
		/// Folds substractions with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Sub(Context ctx)
		{
		}

		/// <summary>
		/// Folds logical ANDs with 2 constants
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor<Context>.Visit(IR.LogicalAndInstruction instruction, Context ctx)
		{

		}

		/// <summary>
		/// Visits the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The CTX.</param>
		void IR.IIRVisitor<Context>.Visit(IR.LogicalNotInstruction instruction, Context ctx)
		{
		}

		/// <summary>
		/// Folds logical ORs with 2 constants
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor<Context>.Visit(IR.LogicalOrInstruction instruction, Context ctx)
		{

		}

		/// <summary>
		/// Folds logical XORs with 2 constants
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor<Context>.Visit(IR.LogicalXorInstruction instruction, Context ctx)
		{
		}

		#region IMethodCompilerStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"Strength Reduction"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		void IMethodCompilerStage.Run(IMethodCompiler compiler)
		{
			if (null == compiler)
				throw new ArgumentNullException(@"compiler");
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
			if (null == blockProvider)
				throw new InvalidOperationException(@"Instruction stream must have been split to basic Blocks.");


			foreach (BasicBlock block in blockProvider.Blocks) {
				Context ctx = new Context(block);
				for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++) {
					block.Instructions[ctx.Index].Visit(this, ctx);
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="pipeline"></param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<CilToIrTransformationStage>(this);
		}
		#endregion


		#region Non-Folding

		/// <summary>
		/// Nops the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Nop(Context ctx)
		{
		}

		/// <summary>
		/// Breaks the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Break(Context ctx)
		{
		}

		/// <summary>
		/// Ldargs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldarg(Context ctx)
		{
		}

		/// <summary>
		/// Ldargas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldarga(Context ctx)
		{
		}

		/// <summary>
		/// Ldlocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldloc(Context ctx)
		{
		}

		/// <summary>
		/// Ldlocas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldloca(Context ctx)
		{
		}

		/// <summary>
		/// LDCs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldc(Context ctx)
		{
		}

		/// <summary>
		/// Ldobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldobj(Context ctx)
		{
		}

		/// <summary>
		/// LDSTRs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldstr(Context ctx)
		{
		}

		/// <summary>
		/// LDFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldfld(Context ctx)
		{
		}

		/// <summary>
		/// Ldfldas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldflda(Context ctx)
		{
		}

		/// <summary>
		/// LDSFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldsfld(Context ctx)
		{
		}

		/// <summary>
		/// Ldsfldas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldsflda(Context ctx)
		{
		}

		/// <summary>
		/// LDFTNs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldftn(Context ctx)
		{
		}

		/// <summary>
		/// Ldvirtftns the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldvirtftn(Context ctx)
		{
		}

		/// <summary>
		/// Ldtokens the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldtoken(Context ctx)
		{
		}

		/// <summary>
		/// Stlocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Stloc(Context ctx)
		{
		}

		/// <summary>
		/// Stargs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Starg(Context ctx)
		{
		}

		/// <summary>
		/// Stobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Stobj(Context ctx)
		{
		}

		/// <summary>
		/// STFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Stfld(Context ctx)
		{
		}

		/// <summary>
		/// STSFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Stsfld(Context ctx)
		{
		}

		/// <summary>
		/// Dups the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Dup(Context ctx)
		{
		}

		/// <summary>
		/// Pops the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Pop(Context ctx)
		{
		}

		/// <summary>
		/// JMPs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Jmp(Context ctx)
		{
		}

		/// <summary>
		/// Calls the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Call(Context ctx)
		{
		}

		/// <summary>
		/// Callis the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Calli(Context ctx)
		{
		}

		/// <summary>
		/// Rets the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ret(Context ctx)
		{
		}

		/// <summary>
		/// Brancs the dh.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Branch(Context ctx)
		{
		}

		/// <summary>
		/// Unaries the branch.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void UnaryBranch(Context ctx)
		{
		}

		/// <summary>
		/// Binaries the branch.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void BinaryBranch(Context ctx)
		{
		}

		/// <summary>
		/// Switches the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Switch(Context ctx)
		{
		}

		/// <summary>
		/// Binaries the logic.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void BinaryLogic(Context ctx)
		{
		}

		/// <summary>
		/// Shifts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Shift(Context ctx)
		{
		}

		/// <summary>
		/// Negs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Neg(Context ctx)
		{
		}

		/// <summary>
		/// Nots the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Not(Context ctx)
		{
		}

		/// <summary>
		/// Conversions the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Conversion(Context ctx)
		{
		}

		/// <summary>
		/// Callvirts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Callvirt(Context ctx)
		{
		}

		/// <summary>
		/// Cpobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Cpobj(Context ctx)
		{
		}

		/// <summary>
		/// Newobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Newobj(Context ctx)
		{
		}

		/// <summary>
		/// Castclasses the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Castclass(Context ctx)
		{
		}

		/// <summary>
		/// Isinsts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Isinst(Context ctx)
		{
		}

		/// <summary>
		/// Unboxes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Unbox(Context ctx)
		{
		}

		/// <summary>
		/// Throws the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Throw(Context ctx)
		{
		}

		/// <summary>
		/// Boxes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Box(Context ctx)
		{
		}

		/// <summary>
		/// Newarrs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Newarr(Context ctx)
		{
		}

		/// <summary>
		/// Ldlens the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldlen(Context ctx)
		{
		}

		/// <summary>
		/// Ldelemas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldelema(Context ctx)
		{
		}

		/// <summary>
		/// Ldelems the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldelem(Context ctx)
		{
		}

		/// <summary>
		/// Stelems the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Stelem(Context ctx)
		{
		}

		/// <summary>
		/// Unboxes any.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void UnboxAny(Context ctx)
		{
		}

		/// <summary>
		/// Refanyvals the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Refanyval(Context ctx)
		{
		}

		/// <summary>
		/// Unaries the arithmetic.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void UnaryArithmetic(Context ctx)
		{
		}

		/// <summary>
		/// Mkrefanies the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Mkrefany(Context ctx)
		{
		}

		/// <summary>
		/// Arithmetics the overflow.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		public void ArithmeticOverflow(Context ctx)
		{
		}

		/// <summary>
		/// Endfinallies the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Endfinally(Context ctx)
		{
		}

		/// <summary>
		/// Leaves the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Leave(Context ctx)
		{
		}

		/// <summary>
		/// Arglists the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Arglist(Context ctx)
		{
		}

		/// <summary>
		/// Binaries the comparison.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void BinaryComparison(Context ctx)
		{
		}

		/// <summary>
		/// Localallocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Localalloc(Context ctx)
		{
		}

		/// <summary>
		/// Endfilters the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Endfilter(Context ctx)
		{
		}

		/// <summary>
		/// Inits the obj.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void InitObj(Context ctx)
		{
		}

		/// <summary>
		/// CPBLKs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Cpblk(Context ctx)
		{
		}

		/// <summary>
		/// Initblks the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Initblk(Context ctx)
		{
		}

		/// <summary>
		/// Prefixes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Prefix(Context ctx)
		{
		}

		/// <summary>
		/// Rethrows the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Rethrow(Context ctx)
		{
		}

		/// <summary>
		/// Sizeofs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Sizeof(Context ctx)
		{
		}

		/// <summary>
		/// Refanytypes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Refanytype(Context ctx)
		{
		}
		
		#endregion

		#region IR
		void IR.IIRVisitor<Context>.Visit(IR.AddressOfInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.ArithmeticShiftRightInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.BranchInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.CallInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.EpilogueInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.FloatingPointCompareInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.FloatingPointToIntegerConversionInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.IntegerCompareInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.IntegerToFloatingPointConversionInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.JmpInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.LiteralInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.LoadInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.PhiInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.PopInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.PrologueInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.PushInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.MoveInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.ReturnInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.ShiftLeftInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.ShiftRightInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.SignExtendedMoveInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.StoreInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.UDivInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.URemInstruction instruction, Context ctx)
		{
		}

		void IR.IIRVisitor<Context>.Visit(IR.ZeroExtendedMoveInstruction instruction, Context ctx)
		{
		}
		#endregion

	}
}
