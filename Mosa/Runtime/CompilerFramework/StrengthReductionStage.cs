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

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs IR constant folding of arithmetic instructions to optimize
	/// the code down to fewer calculations.
	/// </summary>
	public sealed class StrengthReductionStage : CodeTransformationStage, IMethodCompilerStage, 
		IL.IILVisitor<Context>, 
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
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The context.</param>
		void IL.IILVisitor<Context>.Mul(IL.MulInstruction instruction, Context ctx)
		{
			bool multiplyByZero = false;

			if (instruction.First is ConstantOperand) 
				if (IsValueZero(instruction.Results[0].Type.Type, instruction.First as ConstantOperand))
					multiplyByZero = true;

			if (instruction.Second is ConstantOperand)
				if (IsValueZero(instruction.Results[0].Type.Type, instruction.Second as ConstantOperand))
					multiplyByZero = true;

			if (multiplyByZero) {
				if (instruction.Results[0].Type.Type == Metadata.CilElementType.R4)
					Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, 0)));
				else if (instruction.Results[0].Type.Type == Metadata.CilElementType.R8)
					Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, 0)));
				else
					Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, 0)));

				return;
			}

			if (instruction.First is ConstantOperand)
				if (IsValueOne(instruction.Results[0].Type.Type, instruction.First as ConstantOperand)) {
					Replace(ctx, new IR.MoveInstruction(instruction.Results[0], instruction.Second));
					return;
				}

			if (instruction.Second is ConstantOperand)
				if (IsValueOne(instruction.Results[0].Type.Type, instruction.Second as ConstantOperand)) {
					Replace(ctx, new IR.MoveInstruction(instruction.Results[0], instruction.First));
					return;
				}

		}


		/// <summary>
		/// Folds divisions with 2 constants
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The context.</param>
		void IL.IILVisitor<Context>.Div(IL.DivInstruction instruction, Context ctx)
		{

		}

		/// <summary>
		/// Folds the remainder of 2 constants
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The CTX.</param>
		void IL.IILVisitor<Context>.Rem(IL.RemInstruction instruction, Context ctx)
		{

		}

		void IInstructionVisitor<Context>.Visit(Instruction instruction, Context ctx)
		{
		}

		/// <summary>
		/// Folds additions with 2 constants
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The context.</param>
		void IL.IILVisitor<Context>.Add(IL.AddInstruction instruction, Context ctx)
		{

		}

		/// <summary>
		/// Folds substractions with 2 constants
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The context.</param>
		void IL.IILVisitor<Context>.Sub(IL.SubInstruction instruction, Context ctx)
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
			pipeline.InsertBefore<IL.CilToIrTransformationStage>(this);
		}
		#endregion


		#region Non-Folding
		void IL.IILVisitor<Context>.Nop(IL.NopInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Break(IL.BreakInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldarg(IL.LdargInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldarga(IL.LdargaInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldloc(IL.LdlocInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldloca(IL.LdlocaInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldc(IL.LdcInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldobj(IL.LdobjInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldstr(IL.LdstrInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldfld(IL.LdfldInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldflda(IL.LdfldaInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldsfld(IL.LdsfldInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldsflda(IL.LdsfldaInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldftn(IL.LdftnInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldvirtftn(IL.LdvirtftnInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldtoken(IL.LdtokenInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Stloc(IL.StlocInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Starg(IL.StargInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Stobj(IL.StobjInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Stfld(IL.StfldInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Stsfld(IL.StsfldInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Dup(IL.DupInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Pop(IL.PopInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Jmp(IL.JumpInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Call(IL.CallInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Calli(IL.CalliInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ret(IL.ReturnInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Branch(IL.BranchInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.UnaryBranch(IL.UnaryBranchInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.BinaryBranch(IL.BinaryBranchInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Switch(IL.SwitchInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.BinaryLogic(IL.BinaryLogicInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Shift(IL.ShiftInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Neg(IL.NegInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Not(IL.NotInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Conversion(IL.ConversionInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Callvirt(IL.CallvirtInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Cpobj(IL.CpobjInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Newobj(IL.NewobjInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Castclass(IL.CastclassInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Isinst(IL.IsInstInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Unbox(IL.UnboxInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Throw(IL.ThrowInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Box(IL.BoxInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Newarr(IL.NewarrInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldlen(IL.LdlenInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldelema(IL.LdelemaInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Ldelem(IL.LdelemInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Stelem(IL.StelemInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.UnboxAny(IL.UnboxAnyInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Refanyval(IL.RefanyvalInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Mkrefany(IL.MkrefanyInstruction instruction, Context ctx)
		{
		}

		/// <summary>
		/// Arithmetics the overflow.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The CTX.</param>
	    public void ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Endfinally(IL.EndfinallyInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Leave(IL.LeaveInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Arglist(IL.ArglistInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.BinaryComparison(IL.BinaryComparisonInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Localalloc(IL.LocalallocInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Endfilter(IL.EndfilterInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.InitObj(IL.InitObjInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Cpblk(IL.CpblkInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Initblk(IL.InitblkInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Prefix(IL.PrefixInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Rethrow(IL.RethrowInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Sizeof(IL.SizeofInstruction instruction, Context ctx)
		{
		}

		void IL.IILVisitor<Context>.Refanytype(IL.RefanytypeInstruction instruction, Context ctx)
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
