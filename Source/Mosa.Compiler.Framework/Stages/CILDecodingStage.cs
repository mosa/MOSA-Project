/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata.Tables;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.TypeSystem.Cil;
using Mosa.Compiler.Framework.Linker;
using System;
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
			if (methodCompiler.Method is LinkerGeneratedMethod)
				return;

			RuntimeMethod plugMethod = methodCompiler.Compiler.PlugSystem.GetPlugMethod(methodCompiler.Method);

			if (plugMethod != null)
			{
				Operand plugSymbol = Operand.CreateSymbolFromMethod(plugMethod);
				Context context = CreateNewBlockWithContext(-1);
				context.AppendInstruction(IRInstruction.Jmp, null, plugSymbol);
				basicBlocks.AddHeaderBlock(context.BasicBlock);
				return;
			}

			if (!methodCompiler.Method.HasCode)
			{
				if (DelegatePatcher.PatchDelegate(methodCompiler))
					return;

				methodCompiler.StopMethodCompiler();
				return;
			}

			using (Stream code = methodCompiler.GetInstructionStream())
			{
				using (codeReader = new EndianAwareBinaryReader(code, Endianness.Little))
				{
					MethodHeader header = ReadMethodHeader(codeReader);

					if (header.LocalsSignature.RID != 0)
					{
						StandAloneSigRow row = methodCompiler.Method.Module.MetadataModule.Metadata.ReadStandAloneSigRow(header.LocalsSignature);

						LocalVariableSignature localsSignature = new LocalVariableSignature(methodCompiler.Method.Module.MetadataModule.Metadata, row.SignatureBlobIdx);

						SigType[] localSigTypes = new SigType[localsSignature.Locals.Length];

						for (int i = 0; i < localsSignature.Locals.Length; i++)
						{
							localSigTypes[i] = localsSignature.Locals[i].Type;
						}

						var genericDeclaringType = methodCompiler.Method.DeclaringType as CilGenericType;
						if (genericDeclaringType != null)
						{
							localSigTypes = GenericSigTypeResolver.Resolve(localSigTypes, genericDeclaringType.GenericArguments);
						}

						methodCompiler.SetLocalVariableSignature(localSigTypes);
					}

					/* Decode the instructions */
					Decode(methodCompiler, header);
				}
			}
		}

		#endregion IMethodCompilerStage Members

		#region Internals

		/// <summary>
		/// Reads the method header from the instruction stream.
		/// </summary>
		/// <param name="reader">The reader used to decode the instruction stream.</param>
		/// <returns></returns>
		private MethodHeader ReadMethodHeader(EndianAwareBinaryReader reader)
		{
			MethodHeader header = new MethodHeader();

			// Read first byte
			header.Flags = (MethodFlags)reader.ReadByte();

			// Check least significant 2 bits
			switch (header.Flags & MethodFlags.HeaderMask)
			{
				case MethodFlags.TinyFormat:
					header.CodeSize = ((uint)(header.Flags & MethodFlags.TinyCodeSizeMask) >> 2);
					header.Flags &= MethodFlags.HeaderMask;
					break;

				case MethodFlags.FatFormat:

					// Read second byte of flags
					header.Flags = (MethodFlags)(reader.ReadByte() << 8 | (byte)header.Flags);
					if (MethodFlags.ValidHeader != (header.Flags & MethodFlags.HeaderSizeMask))
						throw new InvalidDataException(@"Invalid method header.");
					header.MaxStack = reader.ReadUInt16();
					header.CodeSize = reader.ReadUInt32();
					header.LocalsSignature = new Token(reader.ReadUInt32()); // ReadStandAloneSigRow
					break;

				default:
					throw new InvalidDataException(@"Invalid method header while trying to decode " + this.methodCompiler.Method.FullName + ". (Flags = " + header.Flags.ToString("X") + ", Rva = " + this.methodCompiler.Method.Rva + ")");
			}

			// Are there sections following the code?
			if (MethodFlags.MoreSections != (header.Flags & MethodFlags.MoreSections))
				return header;

			// Yes, seek to them and process those sections
			long codepos = reader.BaseStream.Position;

			// Seek to the end of the code...
			long dataSectPos = codepos + header.CodeSize;
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
					byte a = reader.ReadByte();
					byte b = reader.ReadByte();
					byte c = reader.ReadByte();

					length = (c << 24) | (b << 16) | a;
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
					ExceptionHandlingClause clause = new ExceptionHandlingClause();
					clause.Read(reader, isFat);
					methodCompiler.ExceptionClauseHeader.AddClause(clause);
				}
			}
			while (0x80 == (flags & 0x80));

			reader.BaseStream.Position = codepos;

			return header;
		}

		/// <summary>
		/// Decodes the instruction stream of the reader and populates the compiler.
		/// </summary>
		/// <param name="compiler">The compiler to populate.</param>
		/// <param name="header">The method header.</param>
		private void Decode(BaseMethodCompiler compiler, MethodHeader header)
		{
			// Start of the code stream
			long codeStart = codeReader.BaseStream.Position;

			// End of the code stream
			long codeEnd = codeReader.BaseStream.Position + header.CodeSize;

			// Create context
			Context context = new Context(instructionSet);

			// Prefix instruction
			bool prefix = false;

			while (codeEnd != codeReader.BaseStream.Position)
			{
				// Determine the instruction offset
				int instOffset = (int)(codeReader.BaseStream.Position - codeStart);

				// Read the next opcode from the stream
				OpCode op = (OpCode)codeReader.ReadByte();
				if (OpCode.Extop == op)
					op = (OpCode)(0x100 | codeReader.ReadByte());

				BaseCILInstruction instruction = CILInstruction.Get(op);

				if (instruction == null)
					throw new Exception("CIL " + op + " is not yet supported");

				// Create and initialize the corresponding instruction
				context.AppendInstruction(instruction);
				context.Label = instOffset;
				instruction.Decode(context, this);
				context.HasPrefix = prefix;

				Debug.Assert(context.Instruction != null);

				// Do we need to patch branch targets?
				if (instruction is IBranchInstruction && instruction.FlowControl != FlowControl.Return)
				{
					int pc = (int)(codeReader.BaseStream.Position - codeStart);

					for (int i = 0; i < context.BranchTargets.Length; i++)
						context.BranchTargets[i] += pc;
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
		/// Gets the generic type patcher.
		/// </summary>
		GenericTypePatcher IInstructionDecoder.GenericTypePatcher
		{
			get { return methodCompiler.Compiler.GenericTypePatcher; }
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

		#endregion BaseInstructionDecoder Members
	}
}