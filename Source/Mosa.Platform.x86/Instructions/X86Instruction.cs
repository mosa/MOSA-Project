/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class X86Instruction : BasePlatformInstruction, IRegisterUsage, IPlatformInstruction
	{

		static protected RegisterBitmap NoRegisters = new RegisterBitmap();

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Instruction"/> class.
		/// </summary>
		protected X86Instruction()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Instruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		private X86Instruction(byte operandCount)
			: base(operandCount)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Instruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		protected X86Instruction(byte operandCount, byte resultCount)
			: base(operandCount, resultCount)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		public void Emit(Context context, ICodeEmitter emitter)
		{
			Emit(context, emitter as MachineCodeEmitter);
		}

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected virtual OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			throw new System.Exception("opcode not implemented for this instruction");
		}

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected virtual void Emit(Context context, MachineCodeEmitter emitter)
		{
			OpCode opCode = ComputeOpCode(context.Result, context.Operand1, context.Operand2);
			emitter.Emit(opCode, context.Result, context.Operand1, context.Operand2);
		}

		#endregion // Methods

		#region Operand Overrides

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public virtual void Visit(IX86Visitor visitor, Context context)
		{
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IVisitor visitor, Context context)
		{
			if (visitor is IX86Visitor)
				Visit(visitor as IX86Visitor, context);
		}

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "X86." + base.ToString();
		}

		public virtual Register[] UsableRegisters
		{
			get { return null; }
		}

		#endregion // Operand Overrides

		#region Typesizes

		/// <summary>
		/// Check if the given operand is an unsigned byte
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is an unsigned byte</returns>
		private static bool IsUnsignedByte(Operand operand)
		{
			return (operand.Type.Type == Compiler.Metadata.CilElementType.U1);
		}

		/// <summary>
		/// Check if the given operand is a signed byte
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is a signed byte</returns>
		private static bool IsSignedByte(Operand operand)
		{
			return (operand.Type.Type == Compiler.Metadata.CilElementType.I1);
		}

		/// <summary>
		/// Check if the given operand is an unsigned short
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is an unsigned short</returns>
		private static bool IsUnsignedShort(Operand operand)
		{
			return (operand.Type.Type == Compiler.Metadata.CilElementType.U2);
		}

		/// <summary>
		/// Check if the given operand is a signed short
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is a signed short</returns>
		private static bool IsSignedShort(Operand operand)
		{
			return (operand.Type.Type == Compiler.Metadata.CilElementType.I2);
		}

		/// <summary>
		/// Check if the given operand is an unsigned integer
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is an unsigned integer</returns>
		private static bool IsUnsignedInt(Operand operand)
		{
			return (operand.Type.Type == Compiler.Metadata.CilElementType.U4);
		}

		/// <summary>
		/// Check if the given operand is a signed integer
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is a signed integer</returns>
		private static bool IsSignedInt(Operand operand)
		{
			return (operand.Type.Type == Compiler.Metadata.CilElementType.I4);
		}

		/// <summary>
		/// Check if the given operand is an unsigned long
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is an unsigned long</returns>
		private static bool IsUnsignedLong(Operand operand)
		{
			return (operand.Type.Type == Compiler.Metadata.CilElementType.U8);
		}

		/// <summary>
		/// Check if the given operand is a signed long
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is a signed long</returns>
		private static bool IsSignedLong(Operand operand)
		{
			return (operand.Type.Type == Compiler.Metadata.CilElementType.I8);
		}

		/// <summary>
		/// Check if the given operand is a byte
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is a byte</returns>
		protected static bool IsByte(Operand operand)
		{
			return IsUnsignedByte(operand) || IsSignedByte(operand);
		}

		/// <summary>
		/// Check if the given operand is a short
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is a short</returns>
		protected static bool IsShort(Operand operand)
		{
			return IsUnsignedShort(operand) || IsSignedShort(operand);
		}

		/// <summary>
		/// Check if the given operand is a char
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is a char</returns>
		protected static bool IsChar(Operand operand)
		{
			return operand.Type.Type == Compiler.Metadata.CilElementType.Char;
		}

		/// <summary>
		/// Check if the given operand is an integer
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is an integer</returns>
		protected static bool IsInt(Operand operand)
		{
			return IsUnsignedInt(operand) || IsSignedInt(operand);
		}

		/// <summary>
		/// Check if the given operand is a long
		/// </summary>
		/// <param name="operand">The operand to check</param>
		/// <returns>True if it is a long</returns>
		public static bool IsLong(Operand operand)
		{
			return IsUnsignedLong(operand) || IsSignedLong(operand);
		}

		#endregion

		#region IRegisterUsage

		/// <summary>
		/// Gets the output registers.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public virtual RegisterBitmap GetOutputRegisters(Context context)
		{
			RegisterBitmap registers = new RegisterBitmap();

			RegisterOperand regOperand = context.Result as RegisterOperand;

			if (regOperand != null)
				registers.Set(regOperand.Register);

			registers.Or(AdditionalOutputRegisters);

			return registers;
		}

		/// <summary>
		/// Gets the input registers.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public virtual RegisterBitmap GetInputRegisters(Context context)
		{
			RegisterBitmap registers = new RegisterBitmap();

			registers.Set(GetRegister(context.Operand1, true));
			registers.Set(GetRegister(context.Operand2, true));
			registers.Set(GetRegister(context.Operand3, true));
			registers.Set(GetRegister(context.Result, ResultIsInput));

			registers.Or(AdditionalInputRegisters);

			return registers;
		}

		/// <summary>
		/// Gets a value indicating whether [result is input].
		/// </summary>
		/// <value>
		///   <c>true</c> if [result is input]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool ResultIsInput { get { return true; } }

		/// <summary>
		/// Gets the additional output registers.
		/// </summary>
		public virtual RegisterBitmap AdditionalOutputRegisters { get { return NoRegisters; } }

		/// <summary>
		/// Gets the additional input registers.
		/// </summary>
		public virtual RegisterBitmap AdditionalInputRegisters { get { return NoRegisters; } }

		#endregion // IRegisterUsage

		/// <summary>
		/// Gets the register.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		protected Register GetRegister(Operand operand, bool includeRegister)
		{
			if (operand == null)
				return null;

			if (includeRegister)
			{
				RegisterOperand regOperand = operand as RegisterOperand;

				if (regOperand != null)
					return regOperand.Register;
			}

			MemoryOperand memOperand = operand as MemoryOperand;

			if (memOperand != null)
				return memOperand.Base;

			ParameterOperand paramOperand = operand as ParameterOperand;

			if (paramOperand != null)
				return paramOperand.Base;

			return null;
		}


	}
}
