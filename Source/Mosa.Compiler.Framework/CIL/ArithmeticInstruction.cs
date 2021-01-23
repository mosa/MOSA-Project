// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Arithmetic Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BinaryInstruction" />
	public class ArithmeticInstruction : BinaryInstruction
	{
		#region Static data members

		private const StackTypeCode StackTypeCode_Pointer = StackTypeCode.UnmanagedPointer;    // For table format

		/// <summary>
		/// Generic operand validation table. Not used for add and sub.
		/// </summary>
		private static readonly StackTypeCode[][] operandTable = new StackTypeCode[][] {

			//                    Unknown                int32                  int64                  native int             F                      ManagedPointer         UnmanagedPointer       Object
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // Unknown
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // int32
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // int64
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // native int
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.F,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // F
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // ManagedPointer
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // UnmanagedPointer
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // Object
		};

		/// <summary>
		/// Operand validation table for the add instruction.
		/// </summary>
		private static readonly StackTypeCode[][] addTable = new StackTypeCode[][] {

			//                    Unknown                int32                  int64                  native int             F                      ManagedPointer         UnmanagedPointer       Object
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // Unknown
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode_Pointer, StackTypeCode_Pointer, StackTypeCode.Unknown }, // int32
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode_Pointer, StackTypeCode_Pointer, StackTypeCode.Unknown }, // int64
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.N,       StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode_Pointer, StackTypeCode_Pointer, StackTypeCode.Unknown }, // native int
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.F,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // F
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode_Pointer, StackTypeCode.Unknown, StackTypeCode_Pointer, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // ManagedPointer
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode_Pointer, StackTypeCode_Pointer, StackTypeCode_Pointer, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // UnmanagedPointer
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // Object
		};

		/// <summary>
		/// Operand validation table for the sub instruction.
		/// </summary>
		private static readonly StackTypeCode[][] subTable = new StackTypeCode[][] {

			//                    Unknown                int32                  int64                  native int             F                      ManagedPointer         UnmanagedPointer       Object
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // Unknown
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // int32
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // int64
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // native int
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.F,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // F
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode_Pointer, StackTypeCode_Pointer, StackTypeCode_Pointer, StackTypeCode.Unknown, StackTypeCode.N      , StackTypeCode.N      , StackTypeCode.Unknown }, // ManagedPointer
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode_Pointer, StackTypeCode_Pointer, StackTypeCode_Pointer, StackTypeCode.Unknown, StackTypeCode.N      , StackTypeCode.N      , StackTypeCode.Unknown }, // UnmanagedPointer
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown }, // Object
		};

		#endregion Static data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticInstruction" /> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ArithmeticInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The compiler.</param>
		public override void Resolve(Context context, MethodCompiler methodCompiler)
		{
			base.Resolve(context, methodCompiler);

			var result = StackTypeCode.Unknown;

			var op1 = methodCompiler.Compiler.GetStackTypeCode(context.Operand1.Type);
			var op2 = methodCompiler.Compiler.GetStackTypeCode(context.Operand2.Type);

			switch (opcode)
			{
				case OpCode.Add: result = addTable[(int)op1][(int)op2]; break;
				case OpCode.Sub: result = subTable[(int)op1][(int)op2]; break;
				default: result = operandTable[(int)op1][(int)op2]; break;
			}

			if (result == StackTypeCode.Unknown)
			{
				throw new CompilerException($"Invalid operand types passed to {opcode}");
			}

			MosaType resultType;

			if (StackTypeCode.UnmanagedPointer != result)
			{
				resultType = methodCompiler.Compiler.GetStackTypeFromCode(result);

				if (result == StackTypeCode.F && context.Operand1.IsR4 && context.Operand2.IsR4)
				{
					resultType = methodCompiler.TypeSystem.BuiltIn.R4;
				}
			}
			else
			{
				if (context.Operand1.IsPointer)
				{
					resultType = context.Operand1.Type;
				}
				else if (context.Operand2.IsPointer)
				{
					resultType = context.Operand2.Type;
				}
				else
				{
					throw new CompilerException($"Invalid operand types passed to {opcode}");
				}
			}

			context.Result = methodCompiler.CreateVirtualRegister(resultType);
		}

		#endregion Methods
	}
}
