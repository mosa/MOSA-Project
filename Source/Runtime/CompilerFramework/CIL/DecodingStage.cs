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
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;

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
		private BinaryReader _codeReader;

		/// <summary>
		/// The current compiler context.
		/// </summary>
		private IMethodCompiler _compiler;

		/// <summary>
		/// The method implementation of the currently compiled method.
		/// </summary>
		private RuntimeMethod _method;

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
			// The size of the code in bytes
			MethodHeader header = new MethodHeader();

			using (Stream code = MethodCompiler.GetInstructionStream())
			{
				// Initalize the instruction, setting the initalize size to 10 times the code stream
				MethodCompiler.InstructionSet = new InstructionSet((int)code.Length * 10);

				// update the base class 
				InstructionSet = MethodCompiler.InstructionSet;

				using (BinaryReader reader = new BinaryReader(code))
				{
					_compiler = MethodCompiler;
					_method = MethodCompiler.Method;
					_codeReader = reader;

					//Debug.WriteLine("Decoding " + _compiler.Method.ToString());
					ReadMethodHeader(reader, ref header);

					if (header.localsSignature != 0)
					{
						IMetadataProvider md = _method.MetadataModule.Metadata;
						StandAloneSigRow row = md.ReadStandAloneSigRow(header.localsSignature);

						LocalVariableSignature localsSignature = new LocalVariableSignature();
						localsSignature.LoadSignature(this._method, md, row.SignatureBlobIdx);
						this.MethodCompiler.SetLocalVariableSignature(localsSignature);
					}

					/* Decode the instructions */
					Decode(MethodCompiler, ref header);

					// When we leave, the operand stack must only contain the locals...
					//Debug.Assert(_operandStack.Count == _method.Locals.Count);
					_codeReader = null;
					_compiler = null;
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
			header.flags = (MethodFlags)reader.ReadByte();
			switch (header.flags & MethodFlags.HeaderMask)
			{
				case MethodFlags.TinyFormat:
					header.codeSize = ((uint)(header.flags & MethodFlags.TinyCodeSizeMask) >> 2);
					header.flags &= MethodFlags.HeaderMask;
					break;

				case MethodFlags.FatFormat:
					header.flags = (MethodFlags)(reader.ReadByte() << 8 | (byte)header.flags);
					if (MethodFlags.ValidHeader != (header.flags & MethodFlags.HeaderSizeMask))
						throw new InvalidDataException(@"Invalid method _header.");
					header.maxStack = reader.ReadUInt16();
					header.codeSize = reader.ReadUInt32();
					header.localsSignature = (TokenTypes)reader.ReadUInt32();
					break;

				default:
					throw new InvalidDataException(@"Invalid method header.");
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
						this.MethodCompiler.Method.ExceptionClauseHeader.AddClause(clause);
						// FIXME: Create proper basic Blocks for each item in the clause
					}
				}
				while (0x80 == (flags & 0x80));

				this.MethodCompiler.Method.ExceptionClauseHeader.Sort();
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
			long codeStart = _codeReader.BaseStream.Position;

			// End of the code stream
			long codeEnd = _codeReader.BaseStream.Position + header.codeSize;

			// Prefix instruction
			//PrefixInstruction prefix = null;

			// Setup context
			Context ctx = new Context(InstructionSet, -1);

			while (codeEnd != _codeReader.BaseStream.Position)
			{
				// Determine the instruction offset
				int instOffset = (int)(_codeReader.BaseStream.Position - codeStart);

				// Read the next opcode from the stream
				OpCode op = (OpCode)_codeReader.ReadByte();
				if (OpCode.Extop == op)
					op = (OpCode)(0x100 | _codeReader.ReadByte());

				ICILInstruction instruction = Instruction.Get(op);

				if (instruction == null)
					throw new Exception("CIL " + op + " is not yet supported");

				//if (instruction is PrefixInstruction) {
				//    prefix = instruction as PrefixInstruction;
				//    continue;
				//}

				// Create and initialize the corresponding instruction
				ctx.AppendInstruction(instruction);
				ctx.Label = instOffset;
				instruction.Decode(ctx, this);
				//ctx.Prefix = prefix;

				Debug.Assert(ctx.Instruction != null);

				// Do we need to patch branch targets?
				if (instruction is IBranchInstruction && instruction.FlowControl != FlowControl.Return)
				{
					int pc = (int)(_codeReader.BaseStream.Position - codeStart);

					for (int i = 0; i < ctx.Branch.Targets.Length; i++)
						ctx.Branch.Targets[i] += pc;
				}

				//prefix = null;
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
			get { return _compiler; }
		}

		/// <summary>
		/// Gets the RuntimeMethod being compiled.
		/// </summary>
		/// <value></value>
		RuntimeMethod IInstructionDecoder.Method
		{
			get { return _method; }
		}

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		IModuleTypeSystem IInstructionDecoder.ModuleTypeSystem
		{
			get { return moduleTypeSystem; }
		}

		/// <summary>
		/// Decodes the byte value from the instruction stream
		/// </summary>
		/// <returns></returns>
		byte IInstructionDecoder.DecodeByte()
		{
			return _codeReader.ReadByte();
		}

		/// <summary>
		/// Decodes the sbyte value from the instruction stream
		/// </summary>
		/// <returns></returns>
		sbyte IInstructionDecoder.DecodeSByte()
		{
			return _codeReader.ReadSByte();
		}

		/// <summary>
		/// Decodes the short value from the instruction stream
		/// </summary>
		/// <returns></returns>
		short IInstructionDecoder.DecodeShort()
		{
			return _codeReader.ReadInt16();
		}

		/// <summary>
		/// Decodes the ushort value from the instruction stream
		/// </summary>
		/// <returns></returns>
		ushort IInstructionDecoder.DecodeUShort()
		{
			return _codeReader.ReadUInt16();
		}

		/// <summary>
		/// Decodes the int value from the instruction stream
		/// </summary>
		/// <returns></returns>
		int IInstructionDecoder.DecodeInt()
		{
			return _codeReader.ReadInt32();
		}

		/// <summary>
		/// Decodes the uint value from the instruction stream
		/// </summary>
		/// <returns></returns>
		uint IInstructionDecoder.DecodeUInt()
		{
			return _codeReader.ReadUInt32();
		}

		/// <summary>
		/// Decodes the long value from the instruction stream
		/// </summary>
		/// <returns></returns>
		long IInstructionDecoder.DecodeLong()
		{
			return _codeReader.ReadInt64();
		}

		/// <summary>
		/// Decodes the float value from the instruction stream
		/// </summary>
		/// <returns></returns>
		float IInstructionDecoder.DecodeFloat()
		{
			return _codeReader.ReadSingle();
		}

		/// <summary>
		/// Decodes the double value from the instruction stream
		/// </summary>
		/// <returns></returns>
		double IInstructionDecoder.DecodeDouble()
		{
			return _codeReader.ReadDouble();
		}

		/// <summary>
		/// Decodes the tokentype from the instruction stream
		/// </summary>
		/// <returns></returns>
		TokenTypes IInstructionDecoder.DecodeTokenType()
		{
			return (TokenTypes)_codeReader.ReadInt32();
		}

		#endregion

	}
}
