/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;
using System.IO;

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
		/// The reader used to process the code stream.
		/// </summary>
		private EndianAwareBinaryReader codeReader;

		#endregion Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// No CIL decoding if this is a linker generated method
			if (!methodCompiler.Method.IsCILGenerated)
				return;

			MosaMethod plugMethod = methodCompiler.Compiler.PlugSystem.GetPlugMethod(methodCompiler.Method);

			if (plugMethod != null)
			{
				Operand plugSymbol = Operand.CreateSymbolFromMethod(typeSystem, plugMethod);
				Context context = CreateNewBlockWithContext(-1);
				context.AppendInstruction(IRInstruction.Jmp, null, plugSymbol);
				basicBlocks.AddHeaderBlock(context.BasicBlock);
				return;
			}

			if (!methodCompiler.Method.HasCode)
			{
				if (DelegatePatcher.PatchDelegate(methodCompiler))
					return;

				methodCompiler.Stop();
				return;
			}

			methodCompiler.AllocateLocalVariableVirtualRegisters(methodCompiler.Method.LocalVariables);

			/* Decode the instructions */
			Decode(methodCompiler);
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
			codeReader = new EndianAwareBinaryReader(new MemoryStream(methodCompiler.Method.Code), Endianness.Little);

			// Create context
			Context context = new Context(instructionSet);

			// Prefix instruction
			bool prefix = false;

			while (codeReader.BaseStream.Length != codeReader.BaseStream.Position)
			{
				// Determine the instruction offset
				int instOffset = (int)(codeReader.BaseStream.Position);

				// Read the next opcode from the stream
				OpCode op = (OpCode)codeReader.ReadByte();
				if (OpCode.Extop == op)
					op = (OpCode)(0x100 | codeReader.ReadByte());

				BaseCILInstruction instruction = CILInstruction.Get(op);

				if (instruction == null)
				{
					throw new InvalidMetadataException();
				}

				// Create and initialize the corresponding instruction
				context.AppendInstruction(instruction);
				context.Label = instOffset;
				instruction.Decode(context, this);
				context.HasPrefix = prefix;

				Debug.Assert(context.Instruction != null);

				// Do we need to patch branch targets?
				if (instruction is IBranchInstruction && instruction.FlowControl != FlowControl.Return)
				{
					int pc = (int)(codeReader.BaseStream.Position);

					for (int i = 0; i < context.BranchTargets.Length; i++)
					{
						context.BranchTargets[i] += pc;
					}
				}

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
			get { return methodCompiler; }
		}

		/// <summary>
		/// Gets the MosaMethod being compiled.
		/// </summary>
		/// <value></value>
		MosaMethod IInstructionDecoder.Method
		{
			get { return methodCompiler.Method; }
		}

		/// <summary>
		/// Decodes the byte value from the instruction stream
		/// </summary>
		/// <returns></returns>
		byte IInstructionDecoder.DecodeByte()
		{
			return codeReader.ReadByte();
		}

		/// <summary>
		/// Decodes the sbyte value from the instruction stream
		/// </summary>
		/// <returns></returns>
		sbyte IInstructionDecoder.DecodeSByte()
		{
			return codeReader.ReadSByte();
		}

		/// <summary>
		/// Decodes the short value from the instruction stream
		/// </summary>
		/// <returns></returns>
		short IInstructionDecoder.DecodeShort()
		{
			return codeReader.ReadInt16();
		}

		/// <summary>
		/// Decodes the ushort value from the instruction stream
		/// </summary>
		/// <returns></returns>
		ushort IInstructionDecoder.DecodeUShort()
		{
			return codeReader.ReadUInt16();
		}

		/// <summary>
		/// Decodes the int value from the instruction stream
		/// </summary>
		/// <returns></returns>
		int IInstructionDecoder.DecodeInt()
		{
			return codeReader.ReadInt32();
		}

		/// <summary>
		/// Decodes the uint value from the instruction stream
		/// </summary>
		/// <returns></returns>
		uint IInstructionDecoder.DecodeUInt()
		{
			return codeReader.ReadUInt32();
		}

		/// <summary>
		/// Decodes the long value from the instruction stream
		/// </summary>
		/// <returns></returns>
		long IInstructionDecoder.DecodeLong()
		{
			return codeReader.ReadInt64();
		}

		/// <summary>
		/// Decodes the float value from the instruction stream
		/// </summary>
		/// <returns></returns>
		float IInstructionDecoder.DecodeFloat()
		{
			return codeReader.ReadSingle();
		}

		/// <summary>
		/// Decodes the double value from the instruction stream
		/// </summary>
		/// <returns></returns>
		double IInstructionDecoder.DecodeDouble()
		{
			return codeReader.ReadDouble();
		}

		/// <summary>
		/// Decodes the tokentype from the instruction stream
		/// </summary>
		/// <returns></returns>
		Token IInstructionDecoder.DecodeTokenType()
		{
			return new Token((uint)codeReader.ReadInt32());
		}

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>
		/// The type system.
		/// </value>
		TypeSystem IInstructionDecoder.TypeSystem { get { return typeSystem; } }

		#endregion BaseInstructionDecoder Members
	}
}