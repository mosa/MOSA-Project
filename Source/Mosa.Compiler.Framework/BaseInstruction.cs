// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Base Instruction
/// </summary>
public abstract class BaseInstruction
{
	#region Properties

	/// <summary>
	/// Gets the instructions unique identifier.
	/// </summary>
	/// <value>
	/// The identifier.
	/// </value>
	public int ID { get; private set; }

	/// <summary>
	/// Gets the default operand count of the instruction
	/// </summary>
	/// <value>The operand count.</value>
	public byte DefaultOperandCount { get; protected set; }

	/// <summary>
	/// Gets the default result operand count of the instruction
	/// </summary>
	/// <value>The operand result count.</value>
	public byte DefaultResultCount { get; protected set; }

	/// <summary>
	/// The type of the result type
	/// </summary>
	public virtual BuiltInType ResultType { get; protected set; } = BuiltInType.None;

	/// <summary>
	/// The type of the secondary result type
	/// </summary>
	public virtual BuiltInType ResultType2 { get; protected set; } = BuiltInType.None;

	/// <summary>
	/// Determines flow behavior of this instruction.
	/// </summary>
	/// <remarks>
	/// Knowledge of control flow is required for correct basic block
	/// building. Any instruction that alters the control flow must override
	/// this property and correctly identify its control flow modifications.
	/// </remarks>
	public virtual FlowControl FlowControl => FlowControl.Next;

	/// <summary>
	/// Gets a value indicating whether to [ignore during code generation].
	/// </summary>
	/// <value>
	/// <c>true</c> if [ignore during code generation]; otherwise, <c>false</c>.
	/// </value>
	public virtual bool IgnoreDuringCodeGeneration => false;

	/// <summary>
	/// Gets a value indicating whether to [ignore instruction's basic block].
	/// </summary>
	/// <value>
	/// <c>true</c> if [ignore instruction basic block]; otherwise, <c>false</c>.
	/// </value>
	public virtual bool IgnoreInstructionBasicBlockTargets => false;

	/// <summary>
	/// Gets a value indicating whether this instance has an unspecified side effect.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
	/// </value>
	public virtual bool HasUnspecifiedSideEffect => false;

	/// <summary>
	/// Gets a value indicating whether this instance has memory write side effect.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
	/// </value>
	public virtual bool IsMemoryWrite => false;

	/// <summary>
	/// Gets a value indicating whether this instance has memory write side effect.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
	/// </value>
	public virtual bool IsMemoryRead => false;

	/// <summary>
	/// Gets a value indicating whether this instance has IO operation side effect.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
	/// </value>
	public virtual bool IsIOOperation => false;

	/// <summary>
	/// Gets a value indicating whether [variable operand count].
	/// </summary>
	/// <value>
	///   <c>true</c> if [variable operand count]; otherwise, <c>false</c>.
	/// </value>
	public virtual bool VariableOperands => false;

	/// <summary>
	/// Gets a value indicating whether this <see cref="BaseInstruction"/> is commutative.
	/// </summary>
	/// <value>
	///   <c>true</c> if commutative; otherwise, <c>false</c>.
	/// </value>
	public virtual bool IsCommutative => false;

	/// <summary>
	/// Gets a value indicating whether this instance is parameter load.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance is parameter load; otherwise, <c>false</c>.
	/// </value>
	public virtual bool IsParameterLoad => false;

	/// <summary>
	/// Gets a value indicating whether this instance is parameter store.
	/// </summary>
	/// <value>
	///   <c>true</c> if this instance is parameter store; otherwise, <c>false</c>.
	/// </value>
	public virtual bool IsParameterStore => false;

	public virtual bool IsPlatformInstruction => false;

	public virtual bool IsIRInstruction => false;

	public virtual string Name { get; private set; }

	public virtual string AlternativeName => null;

	public virtual string FamilyName => null;

	public virtual string Modifier => null;

	public virtual string FullName { get; private set; }

	public virtual string OpcodeName { get; private set; }

	#endregion Properties

	#region Platform Properties

	public virtual bool IsZeroFlagUsed => false;

	public virtual bool IsZeroFlagSet => false;

	public virtual bool IsZeroFlagCleared => false;

	public virtual bool IsZeroFlagModified => false;

	public virtual bool IsZeroFlagUnchanged => false;

	public virtual bool IsZeroFlagUndefined => false;

	public virtual bool IsCarryFlagUsed => false;

	public virtual bool IsCarryFlagSet => false;

	public virtual bool IsCarryFlagCleared => false;

	public virtual bool IsCarryFlagModified => false;

	public virtual bool IsCarryFlagUnchanged => false;

	public virtual bool IsCarryFlagUndefined => false;

	public virtual bool IsSignFlagUsed => false;

	public virtual bool IsSignFlagSet => false;

	public virtual bool IsSignFlagCleared => false;

	public virtual bool IsSignFlagModified => false;

	public virtual bool IsSignFlagUnchanged => false;

	public virtual bool IsSignFlagUndefined => false;

	public virtual bool IsOverflowFlagUsed => false;

	public virtual bool IsOverflowFlagSet => false;

	public virtual bool IsOverflowFlagCleared => false;

	public virtual bool IsOverflowFlagModified => false;

	public virtual bool IsOverflowFlagUnchanged => false;

	public virtual bool IsOverflowFlagUndefined => false;

	public virtual bool IsParityFlagUsed => false;

	public virtual bool IsParityFlagSet => false;

	public virtual bool IsParityFlagCleared => false;

	public virtual bool IsParityFlagModified => false;

	public virtual bool IsParityFlagUnchanged => false;

	public virtual bool IsParityFlagUndefined => false;

	public virtual bool AreFlagUseConditional => false;

	#endregion Platform Properties

	#region Static Data

	private static int NextInstructionID = 1;

	private static readonly object _lock = new object();

	#endregion Static Data

	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseInstruction" /> class.
	/// </summary>
	/// <param name="resultCount">The result count.</param>
	/// <param name="operandCount">The operand count.</param>
	protected BaseInstruction(byte resultCount, byte operandCount)
	{
		DefaultResultCount = resultCount;
		DefaultOperandCount = operandCount;

		lock (_lock)
		{
			ID = ++NextInstructionID;
		}

		string name = GetType().ToString();

		int index = name.LastIndexOf('.');

		if (index > 0)
			name = name.Substring(index + 1);

		Name = name;

		FullName = FamilyName + "." + Name;

		OpcodeName = "Opcode." + FullName;
	}

	#endregion Construction

	#region Methods

	public virtual void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		return;
	}

	/// <summary>
	/// Returns a string representation of the context.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String" /> that represents this instance.
	/// </returns>
	public override string ToString()
	{
		return FullName;
	}

	#endregion Methods
}
