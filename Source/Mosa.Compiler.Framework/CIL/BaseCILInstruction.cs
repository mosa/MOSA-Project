// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Base CIL Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseInstruction" />
	public abstract class BaseCILInstruction : BaseInstruction
	{
		#region Data Members

		/// <summary>
		/// Holds the CIL opcode
		/// </summary>
		protected readonly OpCode opcode;

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Gets the op code.
		/// </summary>
		/// <value>The op code.</value>
		public OpCode OpCode { get { return opcode; } }

		/// <summary>
		/// Gets the name of the instruction family.
		/// </summary>
		/// <value>
		/// The name of the instruction family.
		/// </value>
		public override string FamilyName { get { return "CIL"; } }

		public override string Name
		{
			get
			{
				string name = GetType().ToString();

				int index = name.LastIndexOf('.');

				if (index > 0)
					name = name.Substring(index + 1);

				index = name.IndexOf("Instruction");

				if (index > 0)
					name = name.Substring(0, index);

				return name;
			}
		}

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		protected BaseCILInstruction(OpCode opCode, byte operandCount)
			: base(0, operandCount)
		{
			opcode = opCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCILInstruction"/> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		protected BaseCILInstruction(OpCode opCode, byte operandCount, byte resultCount)
			: base(resultCount, operandCount)
		{
			opcode = opCode;
		}

		#endregion Construction

		#region Methods

		public virtual bool DecodeTargets(IInstructionDecoder decoder)
		{
			return false;
		}

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public virtual void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			node.SetInstruction(this, DefaultOperandCount, DefaultResultCount);
		}

		/// <summary>
		/// Determines if the IL decoder pushes the results of this instruction onto the IL operand stack.
		/// </summary>
		/// <value><c>true</c> if [push result]; otherwise, <c>false</c>.</value>
		public virtual bool PushResult
		{
			get { return true; }
		}

		/// <summary>
		/// Validates the specified instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The compiler.</param>
		public virtual void Resolve(Context context, MethodCompiler methodCompiler)
		{
			/* Default implementation is to do nothing */
		}

		#endregion Methods
	}
}
