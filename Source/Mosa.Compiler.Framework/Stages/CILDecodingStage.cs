/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Represents the IL decoding compilation stage.
	/// </summary>
	/// <remarks>
	/// The IL decoding stage takes a stream of bytes and decodes the
	/// instructions represented into an MSIL based intermediate
	/// representation. The instructions are grouped into basic Blocks
	/// for easier local optimizations in later compiler stages.
	/// </remarks>
	public sealed class CILDecodingStage : BaseMethodCompilerStage, IMethodCompilerStage, IInstructionDecoder, IPipelineStage
	{
		#region Data members

		/// <summary>
		/// The instruction being decoded.
		/// </summary>
		private MosaInstruction instruction;

		#endregion Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Execute()
		{
			// No CIL decoding if this is a linker generated method
			if (MethodCompiler.Method.IsLinkerGenerated)
				return;

			MosaMethod plugMethod = MethodCompiler.Compiler.PlugSystem.GetPlugMethod(MethodCompiler.Method);

			if (plugMethod != null)
			{
				Operand plugSymbol = Operand.CreateSymbolFromMethod(TypeSystem, plugMethod);
				Context context = CreateNewBlockWithContext(-1);
				context.AppendInstruction(IRInstruction.Jmp, null, plugSymbol);
				BasicBlocks.AddHeaderBlock(context.BasicBlock);
				return;
			}

			if (MethodCompiler.Method.Code.Count == 0)
			{
				if (DelegatePatcher.PatchDelegate(MethodCompiler))
					return;

				MethodCompiler.Stop();
				return;
			}

			MethodCompiler.SetLocalVariables(MethodCompiler.Method.LocalVariables);

			/* Decode the instructions */
			Decode(MethodCompiler);
		}

		#endregion IMethodCompilerStage Members

		#region Internals

		/// <summary>
		/// Decodes the instruction stream of the reader and populates the compiler.
		/// </summary>
		/// <param name="compiler">The compiler to populate.</param>
		/// <exception cref="InvalidMetadataException"></exception>
		private void Decode(BaseMethodCompiler compiler)
		{
			// Create context
			Context context = new Context(InstructionSet);

			// Prefix instruction
			bool prefix = false;

			for (int i = 0; i < MethodCompiler.Method.Code.Count; i++)
			{
				MosaInstruction instr = MethodCompiler.Method.Code[i];
				var op = (OpCode)instr.OpCode;

				BaseCILInstruction instruction = CILInstruction.Get(op);

				if (instruction == null)
				{
					throw new InvalidMetadataException();
				}

				// Create and initialize the corresponding instruction
				context.AppendInstruction(instruction);
				context.Label = instr.Offset;
				context.HasPrefix = prefix;
				this.instruction = instr;
				instruction.Decode(context, this);

				Debug.Assert(context.Instruction != null);

				prefix = (instruction is PrefixInstruction);
			}
		}

		#endregion Internals

		#region BaseInstructionDecoder Members

		/// <summary>
		/// Gets the compiler, that is currently executing.
		/// </summary>
		/// <value></value>
		BaseMethodCompiler IInstructionDecoder.Compiler
		{
			get { return MethodCompiler; }
		}

		/// <summary>
		/// Gets the MosaMethod being compiled.
		/// </summary>
		/// <value></value>
		MosaMethod IInstructionDecoder.Method
		{
			get { return MethodCompiler.Method; }
		}

		/// <summary>
		/// Gets the Instruction being decoded.
		/// </summary>
		MosaInstruction IInstructionDecoder.Instruction
		{
			get { return instruction; }
		}

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>
		/// The type system.
		/// </value>
		TypeSystem IInstructionDecoder.TypeSystem { get { return TypeSystem; } }

		#endregion BaseInstructionDecoder Members
	}
}