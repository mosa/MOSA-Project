/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Scott Balmos <sbalmos@fastmail.fm>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// An x86 machine code emitter.
	/// </summary>
	public sealed class MachineCodeEmitter : ICodeEmitter, IDisposable
	{
		private static readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		#region Types

		/// <summary>
		/// Patch
		/// </summary>
		struct Patch
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Patch"/> struct.
			/// </summary>
			/// <param name="label">The label.</param>
			/// <param name="position">The position.</param>
			public Patch(int label, long position)
			{
				Label = label;
				Position = position;
			}

			/// <summary>
			/// Patch label
			/// </summary>
			public int Label;

			/// <summary>
			/// The patch's position in the stream
			/// </summary>
			public long Position;

			/// <summary>
			/// Returns a <see cref="System.String"/> that represents this instance.
			/// </summary>
			/// <returns>
			/// A <see cref="System.String"/> that represents this instance.
			/// </returns>
			public override string ToString()
			{
				return "[@" + Position.ToString() + " -> " + Label.ToString() + "]";
			}
		}

		#endregion // Types

		#region Data members

		/// <summary>
		/// The compiler that is generating the code.
		/// </summary>
		private IMethodCompiler compiler;

		/// <summary>
		/// The stream used to write machine code bytes to.
		/// </summary>
		private Stream codeStream;

		/// <summary>
		/// The position that the code stream starts.
		/// </summary>
		private long codeStreamBasePosition;

		/// <summary>
		/// List of labels that were emitted.
		/// </summary>
		private readonly Dictionary<int, long> labels = new Dictionary<int, long>();

		/// <summary>
		/// Holds the linker used to resolve externals.
		/// </summary>
		private IAssemblyLinker linker;

		/// <summary>
		/// List of literal patches we need to perform.
		/// </summary>
		private readonly List<Patch> literals = new List<Patch>();

		/// <summary>
		/// Patches we need to perform.
		/// </summary>
		private readonly List<Patch> patches = new List<Patch>();

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Completes emitting the code of a method.
		/// </summary>
		public void Dispose()
		{
			// Flush the stream - we're not responsible for disposing it, as it belongs
			// to another component that gave it to the code generator.
			codeStream.Flush();
		}

		#endregion // IDisposable Members

		#region ICodeEmitter Members

		/// <summary>
		/// Initializes a new instance of <see cref="MachineCodeEmitter"/>.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="codeStream">The stream the machine code is written to.</param>
		/// <param name="linker">The linker used to resolve external addresses.</param>
		void ICodeEmitter.Initialize(IMethodCompiler compiler, Stream codeStream, IAssemblyLinker linker)
		{
			Debug.Assert(null != compiler, @"MachineCodeEmitter needs a method compiler.");
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");
			Debug.Assert(null != codeStream, @"MachineCodeEmitter needs a code stream.");
			if (codeStream == null)
				throw new ArgumentNullException(@"codeStream");
			Debug.Assert(null != linker, @"MachineCodeEmitter needs a linker.");
			if (linker == null)
				throw new ArgumentNullException(@"linker");

			this.compiler = compiler;
			this.codeStream = codeStream;
			this.codeStreamBasePosition = codeStream.Position;
			this.linker = linker;
		}

		/// <summary>
		/// Emits a label into the code stream.
		/// </summary>
		/// <param name="label">The label name to emit.</param>
		void ICodeEmitter.Label(int label)
		{
			/*
			 * Labels are used to resolve branches inside a procedure. Branches outside
			 * of procedures are handled differently, t.b.d. 
			 * 
			 * So we store the current instruction offset with the label info to be able to 
			 * resolve jumps to this location.
			 *
			 */

			long labelPosition;
			if (labels.TryGetValue(label, out labelPosition))
			{
				if (labelPosition != codeStream.Position)
					throw new ArgumentException(@"Label already defined for another code point.", @"label");
			}

			// Add this label to the label list, so we can resolve the jump later on
			labels.Add(label, codeStream.Position);

			//Debug.WriteLine("LABEL: " + label.ToString() + " @" + codeStream.Position.ToString());
		}

		void ICodeEmitter.ResolvePatches()
		{
			// Save the current position
			long currentPosition = codeStream.Position;

			foreach (Patch p in patches)
			{
				long labelPosition;
				if (!labels.TryGetValue(p.Label, out labelPosition))
				{
					throw new ArgumentException(@"Missing label while resolving patches.", @"label");
				}

				codeStream.Position = p.Position;

				// Compute relative branch offset
				int relOffset = (int)labelPosition - ((int)p.Position + 4);

				// Write relative offset to stream
				byte[] bytes = LittleEndianBitConverter.GetBytes(relOffset);
				codeStream.Write(bytes, 0, bytes.Length);

				//Debug.WriteLine("PATCH: " + p.ToString() + " Offset: " + relOffset.ToString());
			}

			patches.Clear();

			// Reset the position
			codeStream.Position = currentPosition;
		}

		#endregion // ICodeEmitter Members

		#region Code Generation

		/// <summary>
		/// Writes the byte.
		/// </summary>
		/// <param name="data">The data.</param>
		public void WriteByte(byte data)
		{
			codeStream.WriteByte(data);
		}

		/// <summary>
		/// Writes the byte.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="count">The count.</param>
		public void Write(byte[] buffer, int offset, int count)
		{
			codeStream.Write(buffer, offset, count);
		}

		/// <summary>
		/// Emits relative branch code.
		/// </summary>
		/// <param name="code">The branch instruction code.</param>
		/// <param name="dest">The destination label.</param>
		public void EmitBranch(byte[] code, int dest)
		{
			codeStream.Write(code, 0, code.Length);
			EmitRelativeBranchTarget(dest);
		}

		/// <summary>
		/// Calls the specified target.
		/// </summary>
		/// <param name="symbolOperand">The symbol operand.</param>
		public void Call(SymbolOperand symbolOperand)
		{
			long address = linker.Link(
				LinkType.RelativeOffset | LinkType.I4,
				compiler.Method.ToString(),
				(int)(codeStream.Position - codeStreamBasePosition),
				(int)(codeStream.Position - codeStreamBasePosition) + 4,
				symbolOperand.Name,
				IntPtr.Zero
			);

			if (address == 0L)
			{
				this.WriteByte(0);
				this.WriteByte(0);
				this.WriteByte(0);
				this.WriteByte(0);
			}
			else
			{
				this.codeStream.Position += 4;
			}
		}

		/// <summary>
		/// Emits the specified op code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The dest.</param>
		public void Emit(OpCode opCode, Operand dest)
		{
			byte? sib, modRM;
			Operand displacement;

			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			// Write the mod R/M byte
			modRM = CalculateModRM(opCode.RegField, dest, null, out sib, out displacement);
			if (modRM != null)
			{
				codeStream.WriteByte(modRM.Value);
				if (sib.HasValue)
					codeStream.WriteByte(sib.Value);
			}

			// Add displacement to the code
			if (displacement != null)
				WriteDisplacement(displacement);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The destination operand.</param>
		/// <param name="src">The source operand.</param>
		public void Emit(OpCode opCode, Operand dest, Operand src)
		{
			byte? sib, modRM;
			Operand displacement;

			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			if (dest == null && src == null)
				return;

			// Write the mod R/M byte
			modRM = CalculateModRM(opCode.RegField, dest, src, out sib, out displacement);
			if (modRM != null)
			{
				codeStream.WriteByte(modRM.Value);
				if (sib.HasValue)
					codeStream.WriteByte(sib.Value);
			}

			// Add displacement to the code
			if (displacement != null)
				WriteDisplacement(displacement);

			// Add immediate bytes
			if (dest is ConstantOperand)
				WriteImmediate(dest);
			if (src is ConstantOperand)
				WriteImmediate(src);
			if (src is SymbolOperand)
				WriteDisplacement(src);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The source.</param>
		/// <param name="third">The third.</param>
		public void Emit(OpCode opCode, Operand dest, Operand src, Operand third)
		{
			byte? sib = null, modRM = null;
			Operand displacement = null;

			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			if (dest == null && src == null)
				return;

			// Write the mod R/M byte
			modRM = CalculateModRM(opCode.RegField, dest, src, out sib, out displacement);
			if (modRM != null)
			{
				codeStream.WriteByte(modRM.Value);
				if (sib.HasValue)
					codeStream.WriteByte(sib.Value);
			}

			// Add displacement to the code
			if (displacement != null)
				WriteDisplacement(displacement);

			// Add immediate bytes
			if (third is ConstantOperand)
				WriteImmediate(third);
		}

		/// <summary>
		/// Emits the displacement operand.
		/// </summary>
		/// <param name="displacement">The displacement operand.</param>
		public void WriteDisplacement(Operand displacement)
		{
			byte[] disp;

			MemberOperand member = displacement as MemberOperand;
			LabelOperand label = displacement as LabelOperand;
			SymbolOperand symbol = displacement as SymbolOperand;

			if (label != null)
			{
				int pos = (int)(codeStream.Position - codeStreamBasePosition);
				disp = LittleEndianBitConverter.GetBytes((uint)linker.Link(LinkType.AbsoluteAddress | LinkType.I4, compiler.Method.ToString(), pos, 0, label.Name, IntPtr.Zero));
			}
			else if (member != null)
			{
				int pos = (int)(codeStream.Position - codeStreamBasePosition);
				disp = LittleEndianBitConverter.GetBytes((uint)linker.Link(LinkType.AbsoluteAddress | LinkType.I4, compiler.Method.ToString(), pos, 0, member.Member.ToString(), member.Offset));
			}
			else if (symbol != null)
			{
				int pos = (int)(codeStream.Position - codeStreamBasePosition);
				disp = LittleEndianBitConverter.GetBytes((uint)linker.Link(LinkType.AbsoluteAddress | LinkType.I4, compiler.Method.ToString(), pos, 0, symbol.Name, IntPtr.Zero));
			}
			else
				disp = LittleEndianBitConverter.GetBytes((displacement as MemoryOperand).Offset.ToInt32());

			codeStream.Write(disp, 0, disp.Length);
		}

		/// <summary>
		/// Emits an immediate operand.
		/// </summary>
		/// <param name="op">The immediate operand to emit.</param>
		private void WriteImmediate(Operand op)
		{
			byte[] imm = null;
			if (op is LocalVariableOperand)
			{
				// Add the displacement
				StackOperand so = (StackOperand)op;
				imm = LittleEndianBitConverter.GetBytes(so.Offset.ToInt32());
			}
			else if (op is LabelOperand)
			{
				literals.Add(new Patch((op as LabelOperand).Label, codeStream.Position));
				imm = new byte[4];
			}
			else if (op is MemoryOperand)
			{
				// Add the displacement
				MemoryOperand mo = (MemoryOperand)op;
				imm = LittleEndianBitConverter.GetBytes(mo.Offset.ToInt32());
			}
			else if (op is ConstantOperand)
			{
				// Add the immediate
				ConstantOperand co = (ConstantOperand)op;
				switch (op.Type.Type)
				{
					case CilElementType.I:
						try
						{
							if (co.Value is Token)
							{
								imm = LittleEndianBitConverter.GetBytes(((Token)co.Value).ToInt32());
							}
							else
							{
								imm = LittleEndianBitConverter.GetBytes(Convert.ToInt32(co.Value));
							}
						}
						catch (OverflowException)
						{
							imm = LittleEndianBitConverter.GetBytes(Convert.ToUInt64(co.Value));
						}
						break;

					case CilElementType.I1:
						//imm = LittleEndianBitConverter.GetBytes(Convert.ToSByte(co.Value));
						imm = new byte[1] { Convert.ToByte(co.Value) };
						break;

					case CilElementType.I2:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToInt16(co.Value));
						break;
					case CilElementType.I4: goto case CilElementType.I;

					case CilElementType.U1:
						//imm = LittleEndianBitConverter.GetBytes(Convert.ToByte(co.Value));
						imm = new byte[1] { Convert.ToByte(co.Value) };
						break;
					case CilElementType.Char:
						goto case CilElementType.U2;
					case CilElementType.U2:
						imm = LittleEndianBitConverter.GetBytes((ushort)Convert.ToUInt64(co.Value));
						break;
					case CilElementType.Ptr:
					case CilElementType.U4:
						imm = LittleEndianBitConverter.GetBytes((uint)Convert.ToUInt64(co.Value));
						break;
					case CilElementType.I8:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToInt64(co.Value));
						break;
					case CilElementType.U8:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToUInt64(co.Value));
						break;
					case CilElementType.R4:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToSingle(co.Value));
						break;
					case CilElementType.R8: goto default;
					case CilElementType.Object: goto case CilElementType.I;
					default:
						throw new NotSupportedException(String.Format(@"CilElementType.{0} is not supported.", op.Type.Type));
				}
			}
			else if (op is RegisterOperand)
			{
				// Nothing to do...
			}
			else
			{
				throw new NotImplementedException();
			}

			// Emit the immediate constant to the code
			if (imm != null)
				codeStream.Write(imm, 0, imm.Length);
		}

		/// <summary>
		/// Emits the relative branch target.
		/// </summary>
		/// <param name="label">The label.</param>
		private void EmitRelativeBranchTarget(int label)
		{
			// The relative offset of the label
			int relOffset = 0;

			// The position in the code stream of the label
			long position;

			// Did we see the label?
			if (labels.TryGetValue(label, out position))
			{
				// Yes, calculate the relative offset
				relOffset = (int)position - ((int)codeStream.Position + 4);
			}
			else
			{
				// Forward jump, we can't resolve yet - store a patch
				patches.Add(new Patch(label, codeStream.Position));
			}

			// Emit the relative jump offset (zero if we don't know it yet!)
			byte[] bytes = LittleEndianBitConverter.GetBytes(relOffset);
			codeStream.Write(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Emits a far jump to next instruction.
		/// </summary>
		public void EmitFarJumpToNextInstruction()
		{
			codeStream.WriteByte(0xEA);

			// HACK: Determines the IP address of current instruction
			// TODO: Link it instead
			byte[] bytes = LittleEndianBitConverter.GetBytes((int)(linker.GetSection(SectionKind.Text).VirtualAddress.ToInt32() + linker.GetSection(SectionKind.Text).Length + 6));			
			codeStream.Write(bytes, 0, bytes.Length);

			codeStream.WriteByte(0x08);
			codeStream.WriteByte(0x00);
		}

		/// <summary>
		/// Emits an immediate operand.
		/// </summary>
		/// <param name="op">The immediate operand to emit.</param>
		public void EmitImmediate(Operand op)
		{
			byte[] imm = null;
			if (op is LocalVariableOperand)
			{
				// Add the displacement
				StackOperand so = (StackOperand)op;
				imm = LittleEndianBitConverter.GetBytes(so.Offset.ToInt32());
			}
			else if (op is LabelOperand)
			{
				literals.Add(new Patch((op as LabelOperand).Label, codeStream.Position));
				imm = new byte[4];
			}
			else if (op is MemoryOperand)
			{
				// Add the displacement
				MemoryOperand mo = (MemoryOperand)op;
				imm = LittleEndianBitConverter.GetBytes(mo.Offset.ToInt32());
			}
			else if (op is ConstantOperand)
			{
				// Add the immediate
				ConstantOperand co = (ConstantOperand)op;
				switch (op.Type.Type)
				{
					case CilElementType.I:
						try
						{
							imm = LittleEndianBitConverter.GetBytes(Convert.ToInt32(co.Value));
						}
						catch (OverflowException)
						{
							imm = LittleEndianBitConverter.GetBytes(Convert.ToUInt32(co.Value));
						}
						break;

					case CilElementType.I1:
						//imm = LittleEndianBitConverter.GetBytes(Convert.ToSByte(co.Value));
						imm = new byte[1] { Convert.ToByte(co.Value) };
						break;

					case CilElementType.I2:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToInt16(co.Value));
						break;
					case CilElementType.I4: goto case CilElementType.I;

					case CilElementType.U1:
						//imm = LittleEndianBitConverter.GetBytes(Convert.ToByte(co.Value));
						imm = new byte[1] { Convert.ToByte(co.Value) };
						break;
					case CilElementType.Char:
						goto case CilElementType.U2;
					case CilElementType.U2:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToUInt16(co.Value));
						break;
					case CilElementType.U4:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToUInt32(co.Value));
						break;
					case CilElementType.I8:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToInt64(co.Value));
						break;
					case CilElementType.U8:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToUInt64(co.Value));
						break;
					case CilElementType.R4:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToSingle(co.Value));
						break;
					case CilElementType.R8: goto default;
					default:
						throw new NotSupportedException();
				}
			}
			else if (op is RegisterOperand)
			{
				// Nothing to do...
			}
			else
			{
				throw new NotImplementedException();
			}

			// Emit the immediate constant to the code
			if (null != imm)
				codeStream.Write(imm, 0, imm.Length);
		}

		/// <summary>
		/// Calculates the value of the modR/M byte and SIB bytes.
		/// </summary>
		/// <param name="regField">The modR/M regfield value.</param>
		/// <param name="op1">The destination operand.</param>
		/// <param name="op2">The source operand.</param>
		/// <param name="sib">A potential SIB byte to emit.</param>
		/// <param name="displacement">An immediate displacement to emit.</param>
		/// <returns>The value of the modR/M byte.</returns>
		private static byte? CalculateModRM(byte? regField, Operand op1, Operand op2, out byte? sib, out Operand displacement)
		{
			byte? modRM = null;

			displacement = null;

			// FIXME: Handle the SIB byte
			sib = null;

			RegisterOperand rop1 = op1 as RegisterOperand, rop2 = op2 as RegisterOperand;
			MemoryOperand mop1 = op1 as MemoryOperand, mop2 = op2 as MemoryOperand;

			// Normalize the operand order
			if (rop1 == null && rop2 != null)
			{
				// Swap the memory operands
				rop1 = rop2; rop2 = null;
				mop2 = mop1; mop1 = null;
			}

			if (regField != null)
				modRM = (byte)(regField.Value << 3);

			if (rop1 != null && rop2 != null)
			{
				// mod = 11b, reg = rop1, r/m = rop2
				modRM = (byte)((3 << 6) | (rop1.Register.RegisterCode << 3) | rop2.Register.RegisterCode);
			}
			// Check for register/memory combinations
			else if (mop2 != null && mop2.Base != null)
			{
				// mod = 10b, reg = rop1, r/m = mop2
				modRM = (byte)(modRM.GetValueOrDefault() | (2 << 6) | (byte)mop2.Base.RegisterCode);
				if (rop1 != null)
					modRM |= (byte)(rop1.Register.RegisterCode << 3);
				displacement = mop2;
				if (mop2.Base.RegisterCode == 4)
					sib = 0xA4;
			}
			else if (mop2 != null)
			{
				// mod = 10b, r/m = mop1, reg = rop2
				modRM = (byte)(modRM.GetValueOrDefault() | 5);
				if (rop1 != null)
					modRM |= (byte)(rop1.Register.RegisterCode << 3);
				displacement = mop2;
			}
			else if (mop1 != null && mop1.Base != null)
			{
				// mod = 10b, r/m = mop1, reg = rop2
				modRM = (byte)(modRM.GetValueOrDefault() | (2 << 6) | mop1.Base.RegisterCode);
				if (rop2 != null)
					modRM |= (byte)(rop2.Register.RegisterCode << 3);
				displacement = mop1;
				if (mop1.Base.RegisterCode == 4)
					sib = 0xA4;
			}
			else if (mop1 != null)
			{
				// mod = 10b, r/m = mop1, reg = rop2
				modRM = (byte)(modRM.GetValueOrDefault() | 5);
				if (rop2 != null)
					modRM |= (byte)(rop2.Register.RegisterCode << 3);
				displacement = mop1;
			}
			else if (rop1 != null)
			{
				modRM = (byte)(modRM.GetValueOrDefault() | (3 << 6) | rop1.Register.RegisterCode);
				//if (op2 is SymbolOperand)
				//    displacement = op2;
			}

			return modRM;
		}

		#endregion // Code Generation
	}
}
