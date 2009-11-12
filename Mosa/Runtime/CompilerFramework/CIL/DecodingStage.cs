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
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.Loader;
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
	public sealed class DecodingStage : BaseStage, IMethodCompilerStage, IInstructionDecoder, IPipelineStage
	{
		private readonly System.DataConverter LittleEndianBitConverter = System.DataConverter.LittleEndian;

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

		#region Construction

		/// <summary>
		/// The instruction factory used to emit instructions.
		/// </summary>
		public DecodingStage()
		{
		}

		#endregion // Construction

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"CIL decoder"; } }

		private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
				new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(BasicBlockBuilderStage))
			};

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			// The size of the code in bytes
			CIL.MethodHeader header = new CIL.MethodHeader();

			using (Stream code = MethodCompiler.GetInstructionStream()) {

				// Initalize the instruction, setting the initalize size to 10 times the code stream
				MethodCompiler.InstructionSet = new InstructionSet((int)code.Length * 10);

				// update the base class 
				InstructionSet = MethodCompiler.InstructionSet;

				using (BinaryReader reader = new BinaryReader(code)) {
					_compiler = MethodCompiler;
					_method = MethodCompiler.Method;
					_codeReader = reader;

					ReadMethodHeader(reader, ref header);
					//Debug.WriteLine("Decoding " + compiler.Method.ToString());

					if (0 != header.localsSignature) {
						StandAloneSigRow row;
						IMetadataProvider md = _method.Module.Metadata;
						md.Read(header.localsSignature, out row);
						MethodCompiler.SetLocalVariableSignature(LocalVariableSignature.Parse(md, row.SignatureBlobIdx));
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
		/// Reads the method _header From the instruction stream.
		/// </summary>
		/// <param name="reader">The reader used to decode the instruction stream.</param>
		/// <param name="header">The method _header structure to populate.</param>
		private void ReadMethodHeader(BinaryReader reader, ref CIL.MethodHeader header)
		{
			header.flags = (CIL.MethodFlags)reader.ReadByte();
			switch (header.flags & CIL.MethodFlags.HeaderMask) {
				case CIL.MethodFlags.TinyFormat:
					header.codeSize = ((uint)(header.flags & CIL.MethodFlags.TinyCodeSizeMask) >> 2);
					header.flags &= CIL.MethodFlags.HeaderMask;
					break;

				case CIL.MethodFlags.FatFormat:
					header.flags = (CIL.MethodFlags)(reader.ReadByte() << 8 | (byte)header.flags);
					if (CIL.MethodFlags.ValidHeader != (header.flags & Mosa.Runtime.CompilerFramework.CIL.MethodFlags.HeaderSizeMask))
						throw new InvalidDataException(@"Invalid method _header.");
					header.maxStack = reader.ReadUInt16();
					header.codeSize = reader.ReadUInt32();
					header.localsSignature = (TokenTypes)reader.ReadUInt32();
					break;

				default:
					throw new InvalidDataException(@"Invalid method _header.");
			}

			// Are there sections following the code?
			if (CIL.MethodFlags.MoreSections == (header.flags & CIL.MethodFlags.MoreSections)) {
				// Yes, seek to them and process those sections
				long codepos = reader.BaseStream.Position;

				// Seek to the end of the code...
				long dataSectPos = codepos + header.codeSize;
				if (0 != (dataSectPos & 3))
					dataSectPos += (4 - (dataSectPos % 4));
				reader.BaseStream.Position = dataSectPos;

				// Read all headers, so the IL decoder knows how to handle these...
				byte flags;
				int length, blocks;
				bool isFat;
				CIL.EhClause clause = new CIL.EhClause();

				do {
					flags = reader.ReadByte();
					isFat = (0x40 == (flags & 0x40));
					if (isFat) {
						byte[] buffer = new byte[4];
						reader.Read(buffer, 0, 3);
						length = LittleEndianBitConverter.GetInt32(buffer, 0);
						blocks = (length - 4) / 24;
					}
					else {
						length = reader.ReadByte();
						blocks = (length - 4) / 12;

						/* Read & skip the padding. */
						reader.ReadInt16();
					}

					Debug.Assert(0x01 == (flags & 0x3F), @"Unsupported method datta section.");
					// Read the clause
					for (int i = 0; i < blocks; i++) {
						clause.Read(reader, isFat);
						// FIXME: Create proper basic Blocks for each item in the clause
					}
				}
				while (0x80 == (flags & 0x80));

				reader.BaseStream.Position = codepos;
			}
		}

		/// <summary>
		/// Decodes the instruction stream of the reader and populates the compiler.
		/// </summary>
		/// <param name="compiler">The compiler to populate.</param>
		/// <param name="header">The method _header.</param>
		private void Decode(IMethodCompiler compiler, ref CIL.MethodHeader header)
		{
			// Start of the code stream
			long codeStart = _codeReader.BaseStream.Position;

			// End of the code stream
			long codeEnd = _codeReader.BaseStream.Position + header.codeSize;

			// Prefix instruction
			//PrefixInstruction prefix = null;

			// Setup context
			Context ctx = new Context(InstructionSet, -1);

			while (codeEnd != _codeReader.BaseStream.Position) {
				// Determine the instruction offset
				int instOffset = (int)(_codeReader.BaseStream.Position - codeStart);

				// Read the next opcode From the stream
				OpCode op = (OpCode)_codeReader.ReadByte();
				if (OpCode.Extop == op)
					op = (OpCode)(0x100 | _codeReader.ReadByte());

				ICILInstruction instruction = Instruction.Get(op);

				if (instruction == null)
					throw new Exception("CIL " + op.ToString() + " is not yet supported");

				//if (instruction is PrefixInstruction) {
				//    prefix = instruction as PrefixInstruction;
				//    continue;
				//}

				// Create and initialize the corresponding instruction
				ctx.AppendInstruction(instruction);
				instruction.Decode(ctx, this);
				ctx.Label = instOffset;
				//ctx.Prefix = prefix;

				Debug.Assert(ctx.Instruction != null);

				// Do we need to patch branch targets?
				if (instruction is IBranchInstruction && instruction.FlowControl != FlowControl.Return) {
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
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out byte value)
		{
			value = _codeReader.ReadByte();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out sbyte value)
		{
			value = _codeReader.ReadSByte();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out short value)
		{
			value = _codeReader.ReadInt16();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out ushort value)
		{
			value = _codeReader.ReadUInt16();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out int value)
		{
			value = _codeReader.ReadInt32();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out uint value)
		{
			value = _codeReader.ReadUInt32();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out long value)
		{
			value = _codeReader.ReadInt64();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out float value)
		{
			value = _codeReader.ReadSingle();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out double value)
		{
			value = _codeReader.ReadDouble();
		}

		/// <summary>
		/// Decodes <paramref name="value"/> From the instruction stream.
		/// </summary>
		/// <param name="value">Receives the decoded value From the instruction stream.</param>
		void IInstructionDecoder.Decode(out TokenTypes value)
		{
			value = (TokenTypes)_codeReader.ReadInt32();
		}

		#endregion
	}
}
