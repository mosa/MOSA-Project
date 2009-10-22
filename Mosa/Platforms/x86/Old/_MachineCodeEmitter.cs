/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using IR = Mosa.Runtime.CompilerFramework.IR;

// FIXME PG - this class goes away eventually

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// An x86 machine code emitter.
	/// </summary>
	public sealed class _MachineCodeEmitter : _ICodeEmitter, IDisposable
	{
		private static readonly System.DataConverter LittleEndianBitConverter = System.DataConverter.LittleEndian;

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
				this.label = label;
				this.position = position;
			}

			/// <summary>
			/// Patch label
			/// </summary>
			public int label;
			/// <summary>
			/// The patch's position in the stream
			/// </summary>
			public long position;
		}

		#endregion // Types

		#region Data members

		/// <summary>
		/// The compiler thats generating the code.
		/// </summary>
		static IMethodCompiler _compiler;

		/// <summary>
		/// The stream used to write machine code bytes to.
		/// </summary>
		private Stream _codeStream;

		/// <summary>
		/// The position that the code stream starts.
		/// </summary>
		private static long _codeStreamBasePosition;

		/// <summary>
		/// List of labels that were emitted.
		/// </summary>
		private readonly Dictionary<int, long> _labels = new Dictionary<int, long>();

		/// <summary>
		/// Holds the linker used to resolve externals.
		/// </summary>
		private static IAssemblyLinker _linker;

		/// <summary>
		/// List of literal patches we need to perform.
		/// </summary>
		private static readonly List<Patch> _literals = new List<Patch>();

		/// <summary>
		/// Patches we need to perform.
		/// </summary>
		private readonly List<Patch> _patches = new List<Patch>();

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MachineCodeEmitter"/>.
		/// </summary>
		/// <param name="compiler"></param>
		/// <param name="codeStream">The stream the machine code is written to.</param>
		/// <param name="linker">The linker used to resolve external addresses.</param>
		public _MachineCodeEmitter(IMethodCompiler compiler, Stream codeStream, IAssemblyLinker linker)
		{
			Debug.Assert(null != compiler, @"MachineCodeEmitter needs a method compiler.");
			if (null == compiler)
				throw new ArgumentNullException(@"compiler");
			Debug.Assert(null != codeStream, @"MachineCodeEmitter needs a code stream.");
			if (null == codeStream)
				throw new ArgumentNullException(@"codeStream");
			Debug.Assert(null != linker, @"MachineCodeEmitter needs a linker.");
			if (null == linker)
				throw new ArgumentNullException(@"linker");

			_compiler = compiler;
			_codeStream = codeStream;
			_codeStreamBasePosition = codeStream.Position;
			_linker = linker;
		}

		#endregion // Construction

		#region IDisposable Members

		/// <summary>
		/// Completes emitting the code of a method.
		/// </summary>
		public void Dispose()
		{
			// Flush the stream - we're not responsible for disposing it, as it belongs
			// to another component that gave it to the code generator.
			_codeStream.Flush();
		}

		#endregion // IDisposable Members

		#region ICodeEmitter Members

		/// <summary>
		/// Emits a comment into the code stream.
		/// </summary>
		/// <param name="comment">The comment to emit.</param>
		void _ICodeEmitter.Comment(string comment)
		{
			/*
			 * The machine code emitter does not support comments. They're simply ignored.
			 * 
			 */
		}

		/// <summary>
		/// Emits a label into the code stream.
		/// </summary>
		/// <param name="label">The label name to emit.</param>
		void _ICodeEmitter.Label(int label)
		{
			/*
			 * FIXME: Labels are used to resolve branches inside a procedure. Branches outside
			 * of procedures are handled differently, t.b.d. 
			 * 
			 * So we store the current instruction offset with the label info to be able to 
			 * resolve later backward jumps to this location.
			 *
			 * Additionally there may have been forward branches to this location, which have not
			 * been resolved yet, so we need to scan these and resolve them to the current
			 * instruction offset.
			 * 
			 */

			// Save the current position
			long currentPosition = _codeStream.Position, pos;
			// Relative branch offset
			int relOffset;

			if (_labels.TryGetValue(label, out pos)) {
				Debug.Assert(pos == currentPosition);
				if (pos != currentPosition)
					throw new ArgumentException(@"Label already defined for another code point.", @"label");
			}
			else {
				// Check if this label has forward references on it...
				_patches.RemoveAll(delegate(Patch p)
				{
					if (p.label == label) {
						// Set new position
						_codeStream.Position = p.position;
						// Compute relative offset
						relOffset = (int)currentPosition - ((int)p.position + 4);
						// Write relative offset to stream
						byte[] bytes = LittleEndianBitConverter.GetBytes(relOffset);
						_codeStream.Write(bytes, 0, bytes.Length);

						// Success
						return true;
					}
					// Failed
					return false;
				});

				// Add this label to the label list, so we can resolve the jump later on
				_labels.Add(label, currentPosition);

				// Reset the position
				_codeStream.Position = currentPosition;

				// HACK: The machine code emitter needs to replace FP constants
				// with an EIP relative address, but these are not possible on x86,
				// so we store the EIP via a call in the right place on the stack
				//if (0 == label)
				//{
				//    // FIXME: This code doesn't need to be emitted if there are no
				//    // large constants used.
				//    _codeStream.WriteByte(0xE8);
				//    WriteImmediate(0);

				//    SigType i4 = new SigType(CilElementType.I4);

				//    RegisterOperand eax = new RegisterOperand(i4, GeneralPurposeRegister.EAX);
				//    Pop(eax);

				//    MemoryOperand mo = new MemoryOperand(i4, GeneralPurposeRegister.EBP, new IntPtr(-8));
				//    Mov(mo, eax);
				//}
			}
		}

		/// <summary>
		/// Emits a literal constant into the code stream.
		/// </summary>
		/// <param name="label">The label to apply to the data.</param>
		/// <param name="LiteralData"></param>
		void _ICodeEmitter.Literal(int label, IR.LiteralData LiteralData) // SigType type, object data)
		{
			// Save the current position
			long currentPosition = _codeStream.Position;
			// Relative branch offset
			//int relOffset;
			// Flag, if we should really emit the literal (only if the literal is used!)
			bool emit = false;
			// Byte representation of the literal
			byte[] bytes;

			// Check if this label has forward references on it...
			emit = (0 != _literals.RemoveAll(delegate(Patch p)
			{
				if (p.label == label) {
					_codeStream.Position = p.position;
					// HACK: We can't do PIC right now
					//relOffset = (int)currentPosition - ((int)p.position + 4);
					bytes = LittleEndianBitConverter.GetBytes((int)currentPosition);
					_codeStream.Write(bytes, 0, bytes.Length);
					return true;
				}

				return false;
			}));

			if (emit) {
				_codeStream.Position = currentPosition;
				switch (LiteralData.Type.Type) {
					case CilElementType.I8:
						bytes = LittleEndianBitConverter.GetBytes((long)LiteralData.Data);
						break;

					case CilElementType.U8:
						bytes = LittleEndianBitConverter.GetBytes((ulong)LiteralData.Data);
						break;

					case CilElementType.R4:
						bytes = LittleEndianBitConverter.GetBytes((float)LiteralData.Data);
						break;

					case CilElementType.R8:
						bytes = LittleEndianBitConverter.GetBytes((double)LiteralData.Data);
						break;

					default:
						throw new NotImplementedException();
				}

				_codeStream.Write(bytes, 0, bytes.Length);
			}
		}

		/// <summary>
		/// Emits an AND instruction.
		/// </summary>
		/// <param name="dest">The destination operand of the instruction.</param>
		/// <param name="src">The source operand of the instruction.</param>
		void _ICodeEmitter.And(Operand dest, Operand src)	// DONE
		{
			Emit(dest, src, _X86.And(dest, src));
		}

		void _ICodeEmitter.Cdq()
		{
			_codeStream.WriteByte(0x99);
		}

		/// <summary>
		/// Emits an NOT instruction.
		/// </summary>
		/// <param name="dest">The destination operand of the instruction.</param>
		void _ICodeEmitter.Not(Operand dest)
		{
			Emit(dest, null, _X86.Not(dest));
		}

		/// <summary>
		/// Emits an OR instruction.
		/// </summary>
		/// <param name="dest">The destination operand of the instruction.</param>
		/// <param name="src">The source operand of the instruction.</param>
		void _ICodeEmitter.Or(Operand dest, Operand src)
		{
			Emit(dest, src, _X86.Or(dest, src));
		}

		/// <summary>
		/// Outputs the value in src to the port in dest
		/// </summary>
		/// <param name="dest">The destination port.</param>
		/// <param name="src">The value.</param>
		void _ICodeEmitter.Out(Operand dest, Operand src)
		{
			if (src.Type.Type == CilElementType.I1 || src.Type.Type == CilElementType.U1)
				Emit(new byte[] { 0xEE }, null, null, null);
			else
				Emit(new byte[] { 0xEE }, null, null, null);
		}

		/// <summary>
		/// Adds the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		void _ICodeEmitter.Add(Operand dest, Operand src)
		{
			Emit(dest, src, _X86.Add(dest, src));
		}

		/// <summary>
		/// Adcs the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		void _ICodeEmitter.Adc(Operand dest, Operand src)
		{
			Emit(dest, src, _X86.Adc(dest, src));
		}

		/// <summary>
		/// Calls the specified target.
		/// </summary>
		/// <param name="target">The target.</param>
		void _ICodeEmitter.Call(RuntimeMethod target)
		{
			// Done
		}

		/// <summary>
		/// Emits a CALL instruction to the given label.
		/// </summary>
		/// <param name="label">The label to be called.</param>
		/// <remarks>
		/// This only invokes the platform call, it does not push arguments, spill and
		/// save registers or handle the return value.
		/// </remarks>
		void _ICodeEmitter.Call(int label)
		{
			_codeStream.WriteByte(0xE8);
			EmitRelativeBranchTarget(label);
		}

		/// <summary>
		/// Clears DF flag and EFLAGS
		/// </summary>
		void _ICodeEmitter.Cld()
		{
			_codeStream.WriteByte(0xFC);
		}

		/// <summary>
		/// Emits a disable interrupts instruction.
		/// </summary>
		void _ICodeEmitter.Cli()
		{
			_codeStream.WriteByte(0xFA);
		}

		/// <summary>
		/// Emits a comparison instruction.
		/// </summary>
		/// <param name="op1"></param>
		/// <param name="op2"></param>
		void _ICodeEmitter.Cmp(Operand op1, Operand op2)
		{
			Operand opTmp = op1;

			bool flag = false;
			if (op1 is MemoryOperand && op2 is MemoryOperand) {
				flag = true;
				opTmp = new RegisterOperand(opTmp.Type, GeneralPurposeRegister.EDX);

				(this as _ICodeEmitter).Push(opTmp);
				Emit(opTmp, op1, _X86.Move(opTmp, op1));
			}
			// Swap if needed
			if (op1 is ConstantOperand && !(op2 is ConstantOperand)) {
				Operand tmp = opTmp;
				opTmp = op2;
				op2 = tmp;
			}

			if (!(op1 is RegisterOperand) && op2 is ConstantOperand && _X86.IsSigned(opTmp) && _X86.IsSigned(op2)) {
				RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
				(this as _ICodeEmitter).Movsx(eax, opTmp);
				opTmp = eax;
			}

			Emit(opTmp, op2, _X86.Cmp(opTmp, op2));

			if (flag)
				(this as _ICodeEmitter).Pop(opTmp);
		}

		/// <summary>
		/// Compares and exchanges both values
		/// </summary>
		/// <param name="op1">First operand</param>
		/// <param name="op2">Second operand</param>
		void _ICodeEmitter.CmpXchg(Operand op1, Operand op2)
		{
			Emit(op1, op2, _X86.Cmpxchg(op1, op2));
		}

		void _ICodeEmitter.Cvtsd2ss(Operand op1, Operand op2)
		{
			Emit(op1, op2, _X86.Cvtsd2ss(op1, op2));
		}

		void _ICodeEmitter.Cvtsi2sd(Operand op1, Operand op2)
		{
			Emit(op1, op2, _X86.Cvtsi2sd(op1, op2));
		}

		void _ICodeEmitter.Cvtss2sd(Operand op1, Operand op2)
		{
			Emit(op1, op2, _X86.Cvtss2sd(op1, op2));
		}

		void _ICodeEmitter.Cvtsi2ss(Operand op1, Operand op2)
		{
			Emit(op1, op2, _X86.Cvtsi2ss(op1, op2));
		}

		void _ICodeEmitter.Cvttsd2si(Operand op1, Operand op2)
		{
			RegisterOperand edx = new RegisterOperand(op1.Type, GeneralPurposeRegister.EDX);
			if (!(op1 is RegisterOperand)) {
				Emit(edx, op1, _X86.Move(edx, op1));
				Emit(edx, op2, _X86.Cvttsd2si(edx, op2));
				Emit(op1, edx, _X86.Move(op1, edx));
			}
			else
				Emit(op1, op2, _X86.Cvttsd2si(op1, op2));
		}

		void _ICodeEmitter.Cvttss2si(Operand op1, Operand op2)
		{
			RegisterOperand edx = new RegisterOperand(op1.Type, GeneralPurposeRegister.EDX);
			if (!(op1 is RegisterOperand)) {
				Emit(edx, op1, _X86.Move(edx, op1));
				Emit(edx, op2, _X86.Cvttss2si(edx, op2));
				Emit(op1, edx, _X86.Move(op1, edx));
			}
			else
				Emit(op1, op2, _X86.Cvttss2si(op1, op2));
		}

		/// <summary>
		/// Retrieves the CPU ID
		/// </summary>
		/// <param name="dst">The destination base memory address</param>
		/// <param name="function">The CPUID function to execute</param>
		void _ICodeEmitter.CpuId(Operand dst, Operand function)
		{
			// Move argument into eax
			((_ICodeEmitter)this).Mov(new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX), function);

			// Call CPUID
			Emit(new byte[] { 0x0F, 0xA2 }, null, null, null);
		}

		/// <summary>
		/// Halts the machine
		/// </summary>
		void _ICodeEmitter.Hlt()
		{
			_codeStream.WriteByte(0xF4);
		}

		/// <summary>
		/// Reads in from the port at src and stores into dest
		/// </summary>
		/// <param name="src">The source operand</param>
		void _ICodeEmitter.In(Operand src)
		{
			byte[] in32 = new byte[] { 0xEC };
			Emit(in32, null, null, null);
		}

		/// <summary>
		/// Emits an interrupt instruction.
		/// </summary>
		void _ICodeEmitter.Neg(Operand op)
		{
			Emit(op, null, _X86.Neg(op));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="op"></param>
		void _ICodeEmitter.Dec(Operand op)
		{
			Emit(op, null, _X86.Dec(op));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="op"></param>
		void _ICodeEmitter.Inc(Operand op)
		{
			Emit(op, null, _X86.Inc(op));
		}

		/// <summary>
		/// Emits an interrupt instruction.
		/// </summary>
		void _ICodeEmitter.Int(byte interrupt)
		{
			_codeStream.Write(new byte[] { 0xCD, interrupt }, 0, 2);
		}

		/// <summary>
		/// Emits a breakpoint instruction.
		/// </summary>
		void _ICodeEmitter.Int3()
		{
			_codeStream.WriteByte(0xCC);
		}

		/// <summary>
		/// Emits an overflow interrupt instruction.
		/// </summary>
		void _ICodeEmitter.IntO()
		{
			_codeStream.WriteByte(0xCE);
		}

		/// <summary>
		/// Invalidate Internal Caches
		/// </summary>
		void _ICodeEmitter.Invd()
		{
			Emit(new byte[] { 0x0F, 0x08 }, null, null, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="op"></param>
		void _ICodeEmitter.Invlpg(Operand op)
		{
			Emit(new byte[] { 0x0F, 0x01 }, 7, op, null);
		}

		/// <summary>
		/// Returns from an interrupt.
		/// </summary>
		void _ICodeEmitter.Iretd()
		{
			_codeStream.WriteByte(0xCF);
		}

		/// <summary>
		/// Emits a conditional jump above or equal.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Ja(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a conditional jump above or equal.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Jae(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a conditional jump below.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Jb(int dest)
		{
			// DONE
		}

		void _ICodeEmitter.Jbe(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a conditional jump equal.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Je(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a conditional jump greater than.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Jg(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a conditional jump greater than or equal.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Jge(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a conditional jump less than.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Jl(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a conditional jump less than or equal.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Jle(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a conditional jump not equal.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Jne(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a jump instruction.
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Jmp(int dest)
		{
			// DONE
		}

		/// <summary>
		/// Emits a conditional jump not signed
		/// </summary>
		/// <param name="dest">The target label of the jump.</param>
		void _ICodeEmitter.Jns(int dest)
		{
			// DONE
		}

		void _ICodeEmitter.Lea(Operand dest, Operand op)
		{
			// This really emits lea, as I haven't figured out how to emit MOV dst, src+x (e.g. not dereferncing src+x!)
			RegisterOperand rop = (RegisterOperand)dest;
			MemoryOperand mop = (MemoryOperand)op;
			byte[] code;

			if (null != mop.Base) {
				code = new byte[] { 0x8D, 0x84, (4 << 3) };
				code[1] |= (byte)((rop.Register.RegisterCode & 0x07));
				code[2] |= (byte)((mop.Base.RegisterCode & 0x07));
			}
			else {
				code = new byte[] { 0xB8 };
			}

			_codeStream.Write(code, 0, code.Length);
			EmitImmediate(mop);
		}

		/// <summary>
		/// Emits a nop instructions.
		/// </summary>
		void _ICodeEmitter.Nop()
		{
			// DONE
		}

		/// <summary>
		/// Load Fence
		/// </summary>
		void _ICodeEmitter.Lfence()
		{
			Emit(new byte[] { 0x0F, 0xAE }, 5, null, null);
		}

		/// <summary>
		/// Loads the global descriptor table register
		/// </summary>
		/// <param name="dest">Destination to load into</param>
		void _ICodeEmitter.Lgdt(Operand dest)
		{
			Emit(dest, null, _X86.Lgdt(dest));
		}

		/// <summary>
		/// Loads the global interrupt table register
		/// </summary>
		/// <param name="dest">Destination to load into</param>
		void _ICodeEmitter.Lidt(Operand dest)
		{
			Emit(dest, null, _X86.Lidt(dest));
		}

		/// <summary>
		/// LLDTs the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		void _ICodeEmitter.Lldt(Operand dest)
		{
			Emit(dest, null, _X86.Lldt(dest));
		}

		/// <summary>
		/// Load Machine Status Word
		/// </summary>
		/// <param name="src">Source to load from</param>
		void _ICodeEmitter.Lmsw(Operand src)
		{
			Emit(src, null, _X86.Lmsw(src));
		}

		/// <summary>
		/// Asserts LOCK# signal for duration of
		/// the accompanying instruction.
		/// </summary>
		void _ICodeEmitter.Lock()
		{
			_codeStream.WriteByte(0xF0);
		}

		/// <summary>
		/// Memory Fence
		/// </summary>
		void _ICodeEmitter.Mfence()
		{
			Emit(new byte[] { 0x0F, 0xAE }, 6, null, null);
		}

		/// <summary>
		/// Muls the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		void _ICodeEmitter.Mul(Operand dest, Operand src)
		{
			Debug.Assert(dest is RegisterOperand && ((RegisterOperand)dest).Register is GeneralPurposeRegister && ((GeneralPurposeRegister)((RegisterOperand)dest).Register).RegisterCode == GeneralPurposeRegister.EAX.RegisterCode);
			if (src is ConstantOperand) {
				Operand newsrc = new RegisterOperand(src.Type, GeneralPurposeRegister.ECX);
				Emit(newsrc, src, _X86.Move(dest, src));
				src = newsrc;
			}
			Emit(src, null, _X86.Mul(dest, src));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="op"></param>
		void _ICodeEmitter.DirectMultiplication(Operand op)
		{
			Emit(op, null, _X86.Mul(null, op));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="op"></param>
		void _ICodeEmitter.DirectDivision(Operand op)
		{
			// DONE
		}

		/// <summary>
		/// Monitor Wait
		/// </summary>
		void _ICodeEmitter.Mwait()
		{
			Emit(new byte[] { 0x0F, 0x01, 0xC9 }, null, null, null);
		}

		void _ICodeEmitter.Rcr(Operand dest, Operand src)
		{
			// Write the opcode byte
			Debug.Assert(dest is RegisterOperand);
			Emit(dest, null, _X86.Rcr(dest, src));
		}

		void _ICodeEmitter.SseAdd(Operand dest, Operand src)
		{
			if (dest.Type.Type == CilElementType.R4 && src.Type.Type == CilElementType.R4) {
				Emit(dest, src, _X86.Addss(dest, src));
			}
			else {
				CheckAndConvertR4(ref src);
				Emit(dest, src, _X86.Addsd(dest, src));
			}
		}

		void _ICodeEmitter.SseSub(Operand dest, Operand src)
		{
			CheckAndConvertR4(ref src);
			Emit(dest, src, _X86.Subsd(dest, src));
		}

		/// <summary>
		/// Sses the mul.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		void _ICodeEmitter.SseMul(Operand dest, Operand src)
		{
			CheckAndConvertR4(ref src);
			Emit(dest, src, _X86.Mulsd(dest, src));
		}

		/// <summary>
		/// Sses the div.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		void _ICodeEmitter.SseDiv(Operand dest, Operand src)
		{
			CheckAndConvertR4(ref dest);
			CheckAndConvertR4(ref src);
			Emit(dest, src, _X86.Divsd(dest, src));
		}

		void _ICodeEmitter.Sar(Operand dest, Operand src)
		{
			Debug.Assert(src is RegisterOperand || src is ConstantOperand, @"Wrong second operand for sar.");
			if (src is RegisterOperand) {
				Debug.Assert(((RegisterOperand)src).Register == GeneralPurposeRegister.ECX, @"Wrong source register for sar.");
				src = null;
			}
			Emit(dest, src, _X86.Sar(dest, src));
		}

		void _ICodeEmitter.Sal(Operand dest, Operand src)
		{
			Debug.Assert(src is RegisterOperand || src is ConstantOperand, @"Wrong second operand for sar.");
			if (src is RegisterOperand) {
				Debug.Assert(((RegisterOperand)src).Register == GeneralPurposeRegister.ECX, @"Wrong source register for sar.");
				src = null;
			}
			Emit(dest, src, _X86.Sal(dest, src));
		}

		void _ICodeEmitter.Shl(Operand dest, Operand src)
		{
			// We force the shl reg, ecx notion
			Debug.Assert(dest is RegisterOperand);
			// FIXME: Make sure the constant is emitted as a single-byte opcode
			RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I1), GeneralPurposeRegister.ECX);

			if (src is ConstantOperand) {
				src = new ConstantOperand(new SigType(CilElementType.U1), (src as ConstantOperand).Value);
				Emit(dest, src, _X86.Shl(dest, src));
			}
			else {
				ConstantOperand max = new ConstantOperand(new SigType(CilElementType.I4), 31);
				Emit(ecx, src, _X86.Move(ecx, src));
				Emit(ecx, max, _X86.And(ecx, max));
				Emit(dest, null, _X86.Shl(dest, src));
			}
		}

		void _ICodeEmitter.Shld(Operand dest, Operand src, Operand count)
		{
			// HACK: For some reason shld isn't emitted properly if we do
			// Emit(dst, src, count, X86.Shld). It is emitted backwards, if
			// we turn this around the following way - it works.
			Emit(src, dest, count, _X86.Shld(dest, src, count));
		}

		void _ICodeEmitter.Shr(Operand dest, Operand src)
		{
			// Write the opcode byte
			//Debug.Assert(dest is RegisterOperand);
			// FIXME: Make sure the constant is emitted as a single-byte opcode
			RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I1), GeneralPurposeRegister.ECX);

			if (src is ConstantOperand) {
				Emit(dest, src, _X86.Shr(dest, src));
			}
			else {
				Emit(ecx, src, _X86.Move(ecx, src));
				Emit(dest, null, _X86.Shr(dest, src));
			}
		}

		void _ICodeEmitter.Shrd(Operand dest, Operand src, Operand count)
		{
			// HACK: For some reason shrd isn't emitted properly if we do
			// Emit(dst, src, count, X86.Shrd). It is emitted backwards, if
			// we turn this around the following way - it works.
			Emit(src, dest, count, _X86.Shrd(dest, src, count));
		}

		void _ICodeEmitter.Div(Operand dest, Operand src)
		{
			if (src is ConstantOperand) {
				Operand newsrc = new RegisterOperand(src.Type, GeneralPurposeRegister.ECX);
				Emit(newsrc, src, _X86.Move(dest, src));
				src = newsrc;
			}
			Emit(src, null, _X86.Div(null, src));
		}

		void _ICodeEmitter.IDiv(Operand dest, Operand src)
		{
			if (dest is ConstantOperand) {
				Operand newdst = new RegisterOperand(src.Type, GeneralPurposeRegister.EAX);
				Emit(newdst, dest, _X86.Move(newdst, dest));
				dest = newdst;
			}
			if (src is ConstantOperand) {
				Operand newsrc = new RegisterOperand(src.Type, GeneralPurposeRegister.ECX);
				Emit(newsrc, src, _X86.Move(newsrc, src));
				src = newsrc;
			}
			Emit(src, null, _X86.Idiv(dest, src));
		}

		void _ICodeEmitter.Mov(Operand dest, Operand src)
		{
			// DONE
		}

		void _ICodeEmitter.Movss(Operand dest, Operand src)
		{
			if (dest is ConstantOperand)
				throw new ArgumentException(@"Destination can't be constant.", @"dest");

			Emit(dest, src, _X86.Movss(dest, src));
		}

		void _ICodeEmitter.Movsd(Operand dest, Operand src)
		{
			if (dest is ConstantOperand)
				throw new ArgumentException(@"Destination can't be constant.", @"dest");

			Emit(dest, src, _X86.Movsd(dest, src));
		}

		/// <summary>
		/// Emits a mov sign extend instruction.
		/// </summary>
		/// <param name="dest">The destination register.</param>
		/// <param name="src">The source register.</param>
		void _ICodeEmitter.Movsx(Operand dest, Operand src)
		{
			if (!(dest is RegisterOperand))
				throw new ArgumentException(@"Destination must be RegisterOperand.", @"dest");
			if (src is ConstantOperand)
				throw new ArgumentException(@"Source must not be ConstantOperand.", @"src");

			switch (src.Type.Type) {
				case CilElementType.U1: goto case CilElementType.I1;
				case CilElementType.I1:
					Emit(dest, src, _X86.Movsx8(dest, src));
					break;

				case CilElementType.U2: goto case CilElementType.I2;
				case CilElementType.I2:
					Emit(dest, src, _X86.Movsx16(dest, src));
					break;

				default:
					Emit(dest, src, _X86.Move(dest, src));

					break;
			}
		}

		/// <summary>
		/// Emits a mov zero extend instruction.
		/// </summary>
		/// <param name="dest">The destination register.</param>
		/// <param name="src">The source register.</param>
		void _ICodeEmitter.Movzx(Operand dest, Operand src)
		{
			if (dest is ConstantOperand)
				throw new ArgumentException(@"Destination must be RegisterOperand.", @"dest");
			//			if (src is ConstantOperand)
			//				throw new ArgumentException(@"Source must not be ConstantOperand.", @"src");

			switch (src.Type.Type) {
				case CilElementType.I1: goto case CilElementType.U1;
				case CilElementType.U1:
					Emit(dest, src, _X86.Movzx8(dest, src));
					break;

				case CilElementType.I2: goto case CilElementType.U2;
				case CilElementType.U2:
					Emit(dest, src, _X86.Movzx16(dest, src));
					break;

				default:
					Emit(dest, src, _X86.Move(dest, src));

					break;
			}
		}

		/// <summary>
		/// Pauses the machine.
		/// </summary>
		void _ICodeEmitter.Pause()
		{
			_codeStream.WriteByte(0xF3);
			_codeStream.WriteByte(0x90);
		}

		/// <summary>
		/// Pushes the given operand on the stack.
		/// </summary>
		/// <param name="operand">The operand to push.</param>
		void _ICodeEmitter.Pop(Operand operand)
		{
			if (operand is RegisterOperand) {
				RegisterOperand ro = (RegisterOperand)operand;
				_codeStream.WriteByte((byte)(0x58 + ro.Register.RegisterCode));
			}
			else {
				Emit(new byte[] { 0x8F }, 0, operand, null);
			}
		}

		/// <summary>
		/// Pops the stack's top values into the general purpose registers
		/// </summary>
		void _ICodeEmitter.Popad()
		{
			Emit(new byte[] { 0x61 }, 0, null, null);
		}

		/// <summary>
		/// Pop Stack into EFLAGS Register
		/// </summary>
		void _ICodeEmitter.Popfd()
		{
			Emit(new byte[] { 0x9D }, 0, null, null);
		}

		/// <summary>
		/// Pops the top-most value from the stack into the given operand.
		/// </summary>
		/// <param name="operand">The operand to pop.</param>
		void _ICodeEmitter.Push(Operand operand) // DONE
		{
			if (operand is ConstantOperand) {
				_codeStream.WriteByte(0x68);
				EmitImmediate(operand);
			}
			else if (operand is RegisterOperand) {
				RegisterOperand ro = (RegisterOperand)operand;
				_codeStream.WriteByte((byte)(0x50 + ro.Register.RegisterCode));
			}
			else {
				Emit(new byte[] { 0xFF }, 6, operand, null);
			}
		}

		/// <summary>
		/// Pops the top-most value from the stack into the given operand.
		/// </summary>
		void _ICodeEmitter.Pushad()
		{
			Emit(new byte[] { 0x60 }, 0, null, null);
		}

		/// <summary>
		/// Push EFLAGS Register onto the Stack
		/// </summary>
		void _ICodeEmitter.Pushfd()
		{
			Emit(new byte[] { 0x9C }, 0, null, null);
		}

		/// <summary>
		/// Read MSR specified by ECX into
		/// EDX:EAX. (MSR: Model sepcific register)
		/// </summary>
		void _ICodeEmitter.Rdmsr()
		{
			_codeStream.WriteByte(0x0F);
			_codeStream.WriteByte(0x32);
		}

		/// <summary>
		/// Reads performance monitor counter
		/// </summary>
		void _ICodeEmitter.Rdpmc()
		{
			_codeStream.WriteByte(0x0F);
			_codeStream.WriteByte(0x33);
		}

		/// <summary>
		/// Reads the timestamp counter
		/// </summary>
		void _ICodeEmitter.Rdtsc()
		{
			_codeStream.WriteByte(0x0F);
			_codeStream.WriteByte(0x31);
		}

		void _ICodeEmitter.Rep()
		{
			_codeStream.WriteByte(0xF3);
		}

		/// <summary>
		/// Emits a return instruction.
		/// </summary>
		/// <seealso cref="_ICodeEmitter.Ret()"/>
		void _ICodeEmitter.Ret()
		{
			_codeStream.WriteByte(0xC3);
		}

		/// <summary>
		/// Store fence
		/// </summary>
		void _ICodeEmitter.Sfence()
		{
			Emit(null, null, _X86.Sfence());
		}

		/// <summary>
		/// Store global descriptor table to dest
		/// </summary>
		/// <param name="dest">Destination to save to</param>
		void _ICodeEmitter.Sgdt(Operand dest)
		{
			Emit(dest, null, _X86.Sgdt(dest));
		}

		/// <summary>
		/// Store interrupt descriptor table to dest
		/// </summary>
		/// <param name="dest">Destination to save to</param>
		void _ICodeEmitter.Sidt(Operand dest)
		{
			Emit(dest, null, _X86.Sidt(dest));
		}

		/// <summary>
		/// Store Local Descriptor Table Register
		/// </summary>
		/// <param name="dest">The destination operand</param>
		void _ICodeEmitter.Sldt(Operand dest)
		{
			Emit(dest, null, _X86.Sidt(dest));
		}

		/// <summary>
		/// Store Machine Status Word
		/// </summary>
		/// <param name="dest">The destination operand</param>
		void _ICodeEmitter.Smsw(Operand dest)
		{
			Emit(dest, null, _X86.Smsw(dest));
		}

		/// <summary>
		/// Emits a enable interrupts instruction.
		/// </summary>
		void _ICodeEmitter.Sti()
		{
			_codeStream.WriteByte(0xFB);
		}

		/// <summary>
		/// Store MXCSR Register State
		/// </summary>
		/// <param name="dest">The destination operand</param>
		void _ICodeEmitter.StmXcsr(Operand dest)
		{
			Emit(dest, null, _X86.Stmxcsr(dest));
		}

		void _ICodeEmitter.Stosb()
		{
			_codeStream.WriteByte(0xAA);
		}

		void _ICodeEmitter.Stosd()
		{
			_codeStream.WriteByte(0xAB);
		}

		/// <summary>
		/// Subtracts src from dest and stores the result in dest. (dest -= src)
		/// </summary>
		/// <param name="dest">The destination operand.</param>
		/// <param name="src">The source operand.</param>
		void _ICodeEmitter.Sub(Operand dest, Operand src)
		{
			Emit(dest, src, _X86.Sub(dest, src));
		}

		/// <summary>
		/// Subtracts src from dest and stores the result in dest. (dest -= src)
		/// </summary>
		/// <param name="dest">The destination operand.</param>
		/// <param name="src">The source operand.</param>
		void _ICodeEmitter.Sbb(Operand dest, Operand src)
		{
			Emit(dest, src, _X86.Sbb(dest, src));
		}

		/// <summary>
		/// Write Back and Invalidate Cache
		/// </summary>
		void _ICodeEmitter.Wbinvd()
		{
			_codeStream.WriteByte(0x0F);
			_codeStream.WriteByte(0x09);
		}

		/// <summary>
		/// Write to Model Specific Register
		/// </summary>
		void _ICodeEmitter.Wrmsr()
		{
			_codeStream.WriteByte(0x0F);
			_codeStream.WriteByte(0x30);
		}

		/// <summary>
		/// Exchange Register/Memory with a register
		/// </summary>
		/// <param name="dest">The destination operand of the instruction.</param>
		/// <param name="src">The source operand of the instruction.</param>
		void _ICodeEmitter.Xchg(Operand dest, Operand src)
		{
			/*byte[] code = { 0x66 };
			Emit(code, null, null, null);*/
			Emit(dest, src, _X86.Xchg(dest, src));
		}

		/// <summary>
		/// Get Value of Extended Control Register
		/// </summary>
		void _ICodeEmitter.Xgetbv()
		{
			byte[] code = { 0x0F, 0x01, 0xD0 };
			Emit(code, null, null, null);
		}

		/// <summary>
		/// Save Processor Extended States
		/// </summary>
		/// <param name="dest">The destination operand</param>
		void _ICodeEmitter.Xsave(Operand dest)
		{
			Emit(dest, null, _X86.Xsave(dest));
		}

		/// <summary>
		/// Emits an Xor instruction.
		/// </summary>
		/// <param name="dest">The destination operand of the instruction.</param>
		/// <param name="src">The source operand of the instruction.</param>
		void _ICodeEmitter.Xor(Operand dest, Operand src)
		{
			Emit(dest, src, _X86.Xor(dest, src));
		}

		/// <summary>
		/// Set Extended Control Register
		/// </summary>
		void _ICodeEmitter.Xsetbv()
		{
			byte[] code = { 0x0F, 0x01, 0xD1 };
			Emit(code, null, null, null);
		}

		#endregion // ICodeEmitter Members

		#region Code Generation

		/// <summary>
		/// Emits relative branch code.
		/// </summary>
		/// <param name="code">The branch instruction code.</param>
		/// <param name="dest">The destination label.</param>
		private void EmitBranch(byte[] code, int dest)
		{
			_codeStream.Write(code, 0, code.Length);
			EmitRelativeBranchTarget(dest);
		}

		/// <summary>
		/// Emits the specified op code.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <param name="opCode">The op code.</param>
		private void Emit(Operand dest, Operand src, OpCode opCode)
		{
			Emit(opCode.Code, opCode.RegField, dest, src);
		}

		/// <summary>
		/// Emits the specified op code.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <param name="op">The op.</param>
		/// <param name="opCode">The op code.</param>
		private void Emit(Operand dest, Operand src, Operand op, OpCode opCode)
		{
			Emit(opCode.Code, opCode.RegField, dest, src, op);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="code">The opcode bytes.</param>
		/// <param name="regField">The modR/M regfield.</param>
		/// <param name="dest">The destination operand.</param>
		/// <param name="src">The source operand.</param>
		private void Emit(byte[] code, byte? regField, Operand dest, Operand src)
		{
			byte? sib = null, modRM = null;
			MemoryOperand displacement = null;

			// Write the opcode
			_codeStream.Write(code, 0, code.Length);

			if (null == dest && null == src)
				return;

			// Write the mod R/M byte
			modRM = CalculateModRM(regField, dest, src, out sib, out displacement);
			if (null != modRM) {
				_codeStream.WriteByte(modRM.Value);
				if (sib.HasValue) {
					_codeStream.WriteByte(sib.Value);
				}
			}

			// Add displacement to the code
			if (null != displacement)
				EmitDisplacement(displacement);

			// Add immediate bytes
			if (dest is ConstantOperand)
				EmitImmediate(dest);
			if (src is ConstantOperand)
				EmitImmediate(src);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="code">The opcode bytes.</param>
		/// <param name="regField">The modR/M regfield.</param>
		/// <param name="dest">The destination operand.</param>
		/// <param name="src">The source operand.</param>
		/// <param name="op3">The third operand.</param>
		private void Emit(byte[] code, byte? regField, Operand dest, Operand src, Operand op3)
		{
			byte? sib = null, modRM = null;
			MemoryOperand displacement = null;

			// Write the opcode
			_codeStream.Write(code, 0, code.Length);

			if (null == dest && null == src)
				return;

			// Write the mod R/M byte
			modRM = CalculateModRM(regField, dest, src, out sib, out displacement);
			if (null != modRM) {
				_codeStream.WriteByte(modRM.Value);
				if (sib.HasValue) {
					_codeStream.WriteByte(sib.Value);
				}
			}

			// Add displacement to the code
			if (null != displacement)
				EmitDisplacement(displacement);

			// Add immediate bytes
			if (op3 is ConstantOperand)
				EmitImmediate(dest);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="codeStream">The code stream.</param>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The destination operand.</param>
		/// <param name="src">The source operand.</param>
		public static void Emit(Stream codeStream, OpCode opCode, Operand dest, Operand src)
		{
			byte? sib, modRM;
			MemoryOperand displacement;

			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			if (null == dest && null == src)
				return;

			// Write the mod R/M byte
			modRM = CalculateModRM(opCode.RegField, dest, src, out sib, out displacement);
			if (null != modRM) {
				codeStream.WriteByte(modRM.Value);
				if (sib.HasValue) {
					codeStream.WriteByte(sib.Value);
				}
			}

			// Add displacement to the code
			if (null != displacement)
				WriteDisplacement(codeStream, displacement);

			// Add immediate bytes
			if (dest is ConstantOperand)
				WriteImmediate(codeStream, dest);
			if (src is ConstantOperand)
				WriteImmediate(codeStream, src);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="codeStream">The codestream to write to.</param>
		/// <param name="opCode">The op code.</param>
		/// <param name="result">The destination operand.</param>
		/// <param name="leftOperand">The source operand.</param>
		/// <param name="rightOperand">The third operand.</param>
		public static void Emit(System.IO.Stream codeStream, OpCode opCode, Operand result, Operand leftOperand, Operand rightOperand)
		{
			byte? sib = null, modRM = null;
			MemoryOperand displacement = null;

			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			if (null == result && null == leftOperand)
				return;

			// Write the mod R/M byte
			modRM = CalculateModRM(opCode.RegField, result, leftOperand, out sib, out displacement);
			if (null != modRM) {
				codeStream.WriteByte(modRM.Value);
				if (sib.HasValue) {
					codeStream.WriteByte(sib.Value);
				}
			}

			// Add displacement to the code
			if (null != displacement)
				WriteDisplacement(codeStream, displacement);

			// Add immediate bytes
			if (rightOperand is ConstantOperand)
				WriteImmediate(codeStream, rightOperand);
		}

		/// <summary>
		/// Emits the displacement operand.
		/// </summary>
		/// <param name="codeStream">The code stream.</param>
		/// <param name="displacement">The displacement operand.</param>
		public static void WriteDisplacement(Stream codeStream, MemoryOperand displacement)
		{
			byte[] disp;

			MemberOperand member = displacement as MemberOperand;
			LabelOperand label = displacement as LabelOperand;
			if (null != label) {
				int pos = (int)(codeStream.Position - _codeStreamBasePosition);
				disp = LittleEndianBitConverter.GetBytes((uint)_linker.Link(LinkType.AbsoluteAddress | LinkType.I4, _compiler.Method, pos, 0, label.Name, IntPtr.Zero));
			}
			else if (null != member) {
				int pos = (int)(codeStream.Position - _codeStreamBasePosition);
				disp = LittleEndianBitConverter.GetBytes((uint)_linker.Link(LinkType.AbsoluteAddress | LinkType.I4, _compiler.Method, pos, 0, member.Member, member.Offset));
			}
			else {
				disp = LittleEndianBitConverter.GetBytes(displacement.Offset.ToInt32());
			}

			codeStream.Write(disp, 0, disp.Length);
		}

		/// <summary>
		/// Emits an immediate operand.
		/// </summary>
		/// <param name="codeStream">The code stream.</param>
		/// <param name="op">The immediate operand to emit.</param>
		private static void WriteImmediate(System.IO.Stream codeStream, Operand op)
		{
			byte[] imm = null;
			if (op is LocalVariableOperand) {
				// Add the displacement
				StackOperand so = (StackOperand)op;
				imm = LittleEndianBitConverter.GetBytes(so.Offset.ToInt32());
			}
			else if (op is LabelOperand) {
				_literals.Add(new Patch((op as LabelOperand).Label, codeStream.Position));
				imm = new byte[4];
			}
			else if (op is MemoryOperand) {
				// Add the displacement
				MemoryOperand mo = (MemoryOperand)op;
				imm = LittleEndianBitConverter.GetBytes(mo.Offset.ToInt32());
			}
			else if (op is ConstantOperand) {
				// Add the immediate
				ConstantOperand co = (ConstantOperand)op;
				switch (op.Type.Type) {
					case CilElementType.I:
						try {
							imm = LittleEndianBitConverter.GetBytes(Convert.ToInt32(co.Value));
						}
						catch (OverflowException) {
							imm = LittleEndianBitConverter.GetBytes(Convert.ToUInt32(co.Value));
						}
						break;

					case CilElementType.I1:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToSByte(co.Value));
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
			else if (op is RegisterOperand) {
				// Nothing to do...
			}
			else {
				throw new NotImplementedException();
			}

			// Emit the immediate constant to the code
			if (null != imm)
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
			if (_labels.TryGetValue(label, out position)) {
				// Yes, calculate the relative offset
				relOffset = (int)position - ((int)_codeStream.Position + 4);
			}
			else {
				// Forward jump, we can't resolve yet - store a patch
				_patches.Add(new Patch(label, _codeStream.Position));
			}

			// Emit the relative jump offset (zero if we don't know it yet!)
			byte[] bytes = LittleEndianBitConverter.GetBytes(relOffset);
			_codeStream.Write(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Emits the displacement operand.
		/// </summary>
		/// <param name="displacement">The displacement operand.</param>
		private void EmitDisplacement(MemoryOperand displacement)
		{
			byte[] disp;

			MemberOperand member = displacement as MemberOperand;
			LabelOperand label = displacement as LabelOperand;
			if (null != label) {
				int pos = (int)(_codeStream.Position - _codeStreamBasePosition);
				disp = LittleEndianBitConverter.GetBytes((uint)_linker.Link(LinkType.AbsoluteAddress | LinkType.I4, _compiler.Method, pos, 0, label.Name, IntPtr.Zero));
			}
			else if (null != member) {
				int pos = (int)(_codeStream.Position - _codeStreamBasePosition);
				disp = LittleEndianBitConverter.GetBytes((uint)_linker.Link(LinkType.AbsoluteAddress | LinkType.I4, _compiler.Method, pos, 0, member.Member, member.Offset));
			}
			else {
				disp = LittleEndianBitConverter.GetBytes(displacement.Offset.ToInt32());
			}

			_codeStream.Write(disp, 0, disp.Length);
		}

		/// <summary>
		/// Emits an immediate operand.
		/// </summary>
		/// <param name="op">The immediate operand to emit.</param>
		private void EmitImmediate(Operand op)
		{
			byte[] imm = null;
			if (op is LocalVariableOperand) {
				// Add the displacement
				StackOperand so = (StackOperand)op;
				imm = LittleEndianBitConverter.GetBytes(so.Offset.ToInt32());
			}
			else if (op is LabelOperand) {
				_literals.Add(new Patch((op as LabelOperand).Label, _codeStream.Position));
				imm = new byte[4];
			}
			else if (op is MemoryOperand) {
				// Add the displacement
				MemoryOperand mo = (MemoryOperand)op;
				imm = LittleEndianBitConverter.GetBytes(mo.Offset.ToInt32());
			}
			else if (op is ConstantOperand) {
				// Add the immediate
				ConstantOperand co = (ConstantOperand)op;
				switch (op.Type.Type) {
					case CilElementType.I:
						try {
							imm = LittleEndianBitConverter.GetBytes(Convert.ToInt32(co.Value));
						}
						catch (OverflowException) {
							imm = LittleEndianBitConverter.GetBytes(Convert.ToUInt32(co.Value));
						}
						break;

					case CilElementType.I1:
						imm = LittleEndianBitConverter.GetBytes(Convert.ToSByte(co.Value));
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
			else if (op is RegisterOperand) {
				// Nothing to do...
			}
			else {
				throw new NotImplementedException();
			}

			// Emit the immediate constant to the code
			if (null != imm)
				_codeStream.Write(imm, 0, imm.Length);
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
		private static byte? CalculateModRM(byte? regField, Operand op1, Operand op2, out byte? sib, out MemoryOperand displacement)
		{
			byte? modRM = null;

			displacement = null;

			// FIXME: Handle the SIB byte
			sib = null;

			RegisterOperand rop1 = op1 as RegisterOperand, rop2 = op2 as RegisterOperand;
			MemoryOperand mop1 = op1 as MemoryOperand, mop2 = op2 as MemoryOperand;

			// Normalize the operand order
			if (null == rop1 && null != rop2) {
				// Swap the memory operands
				rop1 = rop2; rop2 = null;
				mop2 = mop1; mop1 = null;
			}

			if (null != regField)
				modRM = (byte)(regField.Value << 3);

			if (null != rop1 && null != rop2) {
				// mod = 11b, reg = rop1, r/m = rop2
				modRM = (byte)((3 << 6) | (rop1.Register.RegisterCode << 3) | rop2.Register.RegisterCode);
			}
			// Check for register/memory combinations
			else if (null != mop2 && null != mop2.Base) {
				// mod = 10b, reg = rop1, r/m = mop2
				modRM = (byte)(modRM.GetValueOrDefault() | (2 << 6) | (byte)mop2.Base.RegisterCode);
				if (null != rop1) {
					modRM |= (byte)(rop1.Register.RegisterCode << 3);
				}
				displacement = mop2;
			}
			else if (null != mop2) {
				// mod = 10b, r/m = mop1, reg = rop2
				modRM = (byte)(modRM.GetValueOrDefault() | 5);
				if (null != rop1) {
					modRM |= (byte)(rop1.Register.RegisterCode << 3);
				}
				displacement = mop2;
			}
			else if (null != mop1 && null != mop1.Base) {
				// mod = 10b, r/m = mop1, reg = rop2
				modRM = (byte)(modRM.GetValueOrDefault() | (2 << 6) | mop1.Base.RegisterCode);
				if (null != rop2) {
					modRM |= (byte)(rop2.Register.RegisterCode << 3);
				}
				displacement = mop1;
			}
			else if (null != mop1) {
				// mod = 10b, r/m = mop1, reg = rop2
				modRM = (byte)(modRM.GetValueOrDefault() | 5);
				if (null != rop2) {
					modRM |= (byte)(rop2.Register.RegisterCode << 3);
				}
				displacement = mop1;
			}
			else if (null != rop1) {
				modRM = (byte)(modRM.GetValueOrDefault() | (3 << 6) | rop1.Register.RegisterCode);
			}

			return modRM;
		}

		void _ICodeEmitter.Setcc(Operand destination, IR.ConditionCode code)
		{
			byte[] byteCode;

			switch (code) {
				case IR.ConditionCode.Equal:
					byteCode = new byte[] { 0x0F, 0x94 };
					break;

				case IR.ConditionCode.LessThan:
					byteCode = new byte[] { 0x0F, 0x9C };
					break;

				case IR.ConditionCode.LessOrEqual:
					byteCode = new byte[] { 0x0F, 0x9E };
					break;

				case IR.ConditionCode.GreaterOrEqual:
					byteCode = new byte[] { 0x0F, 0x9D };
					break;

				case IR.ConditionCode.GreaterThan:
					byteCode = new byte[] { 0x0F, 0x9F };
					break;

				case IR.ConditionCode.NotEqual:
					byteCode = new byte[] { 0x0F, 0x95 };
					break;

				case IR.ConditionCode.UnsignedGreaterOrEqual:
					byteCode = new byte[] { 0x0F, 0x93 };
					break;

				case IR.ConditionCode.UnsignedGreaterThan:
					byteCode = new byte[] { 0x0F, 0x97 };
					break;

				case IR.ConditionCode.UnsignedLessOrEqual:
					byteCode = new byte[] { 0x0F, 0x96 };
					break;

				case IR.ConditionCode.UnsignedLessThan:
					byteCode = new byte[] { 0x0F, 0x92 };
					break;

				default:
					throw new NotSupportedException();
			}
			Emit(byteCode, null, destination, null);
		}

		void _ICodeEmitter.Comisd(Operand op1, Operand op2)
		{
			Emit(op1, op2, _X86.Comisd(op1, op2));
		}

		void _ICodeEmitter.Comiss(Operand op1, Operand op2)
		{
			Emit(op1, op2, _X86.Comiss(op1, op2));
		}

		void _ICodeEmitter.Ucomisd(Operand op1, Operand op2)
		{
			Emit(op1, op2, _X86.Ucomisd(op1, op2));
		}

		void _ICodeEmitter.Ucomiss(Operand op1, Operand op2)
		{
			Emit(op1, op2, _X86.Ucomiss(op1, op2));
		}

		/// <summary>
		/// Checks if the given operand is a single precision floatingpoint value
		/// and converts it to double precision for furhter usage.
		/// </summary>
		/// <param name="src">The operand to check</param>
		private void CheckAndConvertR4(ref Operand src)
		{
			if ((src is RegisterOperand) || src.Type.Type != CilElementType.R4) return;

			// First, convert it to double precision
			RegisterOperand dest = new RegisterOperand(new SigType(CilElementType.R8), SSE2Register.XMM1);
			Emit(dest, src, _X86.Cvtss2sd(dest, src));

			// New Operand is a Registeroperand
			src = new RegisterOperand(new SigType(CilElementType.R8), SSE2Register.XMM1);
		}
		#endregion // Code Generation
	}
}
