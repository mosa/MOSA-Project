/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Compiler.Common;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.TypeSystem.Generic;

namespace Mosa.Runtime.CompilerFramework.CIL
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
	public sealed class DecodingStage : BaseMethodCompilerStage, IMethodCompilerStage, IInstructionDecoder, IPipelineStage
	{
		private readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		#region Data members

		/// <summary>
		/// The reader used to process the code stream.
		/// </summary>
		private BinaryReader codeReader;

		#endregion // Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"CIL.DecodingStage"; } }

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			using (Stream code = methodCompiler.GetInstructionStream())
			{
				// Initialize the instruction
				methodCompiler.InstructionSet = new InstructionSet(256);

				// update the base class 
				instructionSet = methodCompiler.InstructionSet;

				using (codeReader = new BinaryReader(code))
				{
					// The size of the code in bytes
					MethodHeader header = new MethodHeader();
					ReadMethodHeader(codeReader, ref header);

					if (header.localsSignature.RID != 0)
					{
						StandAloneSigRow row = methodCompiler.Method.Module.MetadataModule.Metadata.ReadStandAloneSigRow(header.localsSignature);

						LocalVariableSignature localsSignature;

						if (methodCompiler.Method.DeclaringType is CilGenericType)
						{
							localsSignature = new LocalVariableSignature(methodCompiler.Method.Module.MetadataModule.Metadata, row.SignatureBlobIdx, (methodCompiler.Method.DeclaringType as CilGenericType).GenericArguments);
						}
						else
						{
							localsSignature = new LocalVariableSignature(methodCompiler.Method.Module.MetadataModule.Metadata, row.SignatureBlobIdx);
						}

						methodCompiler.SetLocalVariableSignature(localsSignature);
					}

					/* Decode the instructions */
					Decode(methodCompiler, ref header);

					// When we leave, the operand stack must only contain the locals...
					//Debug.Assert(_operandStack.Count == _method.Locals.Count);
				}
			}
		}

		#endregion // IMethodCompilerStage Members

		#region Internals

		/// <summary>
		/// Reads the method header from the instruction stream.
		/// </summary>
		/// <param name="reader">The reader used to decode the instruction stream.</param>
		/// <param name="header">The method header structure to populate.</param>
		private void ReadMethodHeader(BinaryReader reader, ref MethodHeader header)
		{
			// Read first byte
			header.flags = (MethodFlags)reader.ReadByte();

			// Check least significant 2 bits
			switch (header.flags & MethodFlags.HeaderMask)
			{
				case MethodFlags.TinyFormat:
					header.codeSize = ((uint)(header.flags & MethodFlags.TinyCodeSizeMask) >> 2);
					header.flags &= MethodFlags.HeaderMask;
					break;

				case MethodFlags.FatFormat:
					// Read second byte of flags
					header.flags = (MethodFlags)(reader.ReadByte() << 8 | (byte)header.flags);
					if (MethodFlags.ValidHeader != (header.flags & MethodFlags.HeaderSizeMask))
						throw new InvalidDataException(@"Invalid method _header.");
					header.maxStack = reader.ReadUInt16();
					header.codeSize = reader.ReadUInt32();
					header.localsSignature = new Token(reader.ReadUInt32()); // ReadStandAloneSigRow
					break;

				default:
					throw new InvalidDataException(@"Invalid method header while trying to decode " + this.methodCompiler.Method.ToString() + ". (Flags = " + header.flags.ToString("X") + ", Rva = " + this.methodCompiler.Method.Rva + ")");
			}

			// Are there sections following the code?
			if (MethodFlags.MoreSections == (header.flags & MethodFlags.MoreSections))
			{
				// Yes, seek to them and process those sections
				long codepos = reader.BaseStream.Position;

				// Seek to the end of the code...
				long dataSectPos = codepos + header.codeSize;
				if (0 != (dataSectPos & 3))
					dataSectPos += (4 - (dataSectPos % 4));
				reader.BaseStream.Position = dataSectPos;

				// Read all headers, so the IL decoder knows how to handle these...
				byte flags;

				do
				{
					flags = reader.ReadByte();
					bool isFat = (0x40 == (flags & 0x40));
					int length;
					int blocks;
					if (isFat)
					{
						byte[] buffer = new byte[4];
						reader.Read(buffer, 0, 3);
						length = LittleEndianBitConverter.GetInt32(buffer, 0);
						blocks = (length - 4) / 24;
					}
					else
					{
						length = reader.ReadByte();
						blocks = (length - 4) / 12;

						/* Read & skip the padding. */
						reader.ReadInt16();
					}

					Debug.Assert(0x01 == (flags & 0x3F), @"Unsupported method data section.");
					// Read the clause
					for (int i = 0; i < blocks; i++)
					{
						EhClause clause = new EhClause();
						clause.Read(reader, isFat);
						//this.methodCompiler.Method.ExceptionClauseHeader.AddClause(clause);
						// FIXME: Create proper basic Blocks for each item in the clause
					}
				}
				while (0x80 == (flags & 0x80));

				//methodCompiler.Method.ExceptionClauseHeader.Sort();
				reader.BaseStream.Position = codepos;
			}
		}

		/// <summary>
		/// Decodes the instruction stream of the reader and populates the compiler.
		/// </summary>
		/// <param name="compiler">The compiler to populate.</param>
		/// <param name="header">The method _header.</param>
		private void Decode(IMethodCompiler compiler, ref MethodHeader header)
		{
			// Start of the code stream
			long codeStart = codeReader.BaseStream.Position;

			// End of the code stream
			long codeEnd = codeReader.BaseStream.Position + header.codeSize;

			// Prefix instruction
			PrefixInstruction prefix = null;

			// Setup context
			Context ctx = new Context(instructionSet);

			while (codeEnd != codeReader.BaseStream.Position)
			{
				// Determine the instruction offset
				int instOffset = (int)(codeReader.BaseStream.Position - codeStart);

				// Read the next opcode from the stream
				OpCode op = (OpCode)codeReader.ReadByte();
				if (OpCode.Extop == op)
					op = (OpCode)(0x100 | codeReader.ReadByte());

				ICILInstruction instruction = Instruction.Get(op);

				if (instruction == null)
					throw new Exception("CIL " + op + " is not yet supported");

				if (instruction is PrefixInstruction)
				{
					prefix = instruction as PrefixInstruction;
					prefix.Decode(ctx, this);
					continue;
				}

				// Create and initialize the corresponding instruction
				ctx.AppendInstruction(instruction);
				ctx.Label = instOffset;
				instruction.Decode(ctx, this);
				ctx.Prefix = prefix;

				Debug.Assert(ctx.Instruction != null);

				// Do we need to patch branch targets?
				if (instruction is IBranchInstruction && instruction.FlowControl != FlowControl.Return)
				{
					int pc = (int)(codeReader.BaseStream.Position - codeStart);

					for (int i = 0; i < ctx.Branch.Targets.Length; i++)
						ctx.Branch.Targets[i] += pc;
				}

				prefix = null;
			}
		}

		#endregion // Internals

		#region IInstructionDecoder Members

		/// <summary>
		/// Gets the compiler, that is currently executing.
		/// </summary>
		/// <value></value>
		IMethodCompiler IInstructionDecoder.Compiler
		{
			get { return methodCompiler; }
		}

		/// <summary>
		/// Gets the RuntimeMethod being compiled.
		/// </summary>
		/// <value></value>
		RuntimeMethod IInstructionDecoder.Method
		{
			get { return methodCompiler.Method; }
		}

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		ITypeModule IInstructionDecoder.TypeModule
		{
			get { return typeModule; }
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

		#endregion

	}
}
