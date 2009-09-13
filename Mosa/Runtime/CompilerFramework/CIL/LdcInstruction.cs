/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class LdcInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdcInstruction"/> class.
		/// </summary>
		public LdcInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion // Construction

		#region ICILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="opcode">The opcode of the load.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, OpCode opcode, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, opcode, decoder);
			
			SigType type;
			object value;

			// Opcode specific handling
			switch (opcode) {
				case OpCode.Ldc_i4: {
						int i;
						decoder.Decode(out i);
						type = new SigType(CilElementType.I4);
						value = i;
					}
					break;

				case OpCode.Ldc_i4_s: {
						sbyte sb;
						decoder.Decode(out sb);
						type = new SigType(CilElementType.I4);
						value = sb;
					}
					break;

				case OpCode.Ldc_i8: {
						long l;
						decoder.Decode(out l);
						type = new SigType(CilElementType.I8);
						value = l;
					}
					break;

				case OpCode.Ldc_r4: {
						float f;
						decoder.Decode(out f);
						type = new SigType(CilElementType.R4);
						value = f;
					}
					break;

				case OpCode.Ldc_r8: {
						double d;
						decoder.Decode(out d);
						type = new SigType(CilElementType.R8);
						value = d;
					}
					break;

				case OpCode.Ldnull:
					instruction.Result = ConstantOperand.GetNull();
					return;

				case OpCode.Ldc_i4_0:
					instruction.Result = ConstantOperand.FromValue(0);
					return;

				case OpCode.Ldc_i4_1:
					instruction.Result =  ConstantOperand.FromValue(1);
					return;

				case OpCode.Ldc_i4_2:
					instruction.Result =  ConstantOperand.FromValue(2);
					return;

				case OpCode.Ldc_i4_3:
					instruction.Result =  ConstantOperand.FromValue(3);
					return;

				case OpCode.Ldc_i4_4:
					instruction.Result =  ConstantOperand.FromValue(4);
					return;

				case OpCode.Ldc_i4_5:
					instruction.Result =  ConstantOperand.FromValue(5);
					return;

				case OpCode.Ldc_i4_6:
					instruction.Result = ConstantOperand.FromValue(6);
					return;

				case OpCode.Ldc_i4_7:
					instruction.Result =  ConstantOperand.FromValue(7);
					return;

				case OpCode.Ldc_i4_8:
					instruction.Result =  ConstantOperand.FromValue(8);
					return;

				case OpCode.Ldc_i4_m1:
					instruction.Result =  ConstantOperand.FromValue(-1);
					return;

				default:
					throw new NotImplementedException();
			}

			instruction.Result = new ConstantOperand(type, value);
			instruction.Ignore = true;
		}

		#endregion // ICILInstruction Overrides

	}
}
