﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework
{
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
		public virtual FlowControl FlowControl { get { return FlowControl.Next; } }

		/// <summary>
		/// Gets a value indicating whether to [ignore during code generation].
		/// </summary>
		/// <value>
		/// <c>true</c> if [ignore during code generation]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IgnoreDuringCodeGeneration { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether to [ignore instruction's basic block].
		/// </summary>
		/// <value>
		/// <c>true</c> if [ignore instruction basic block]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IgnoreInstructionBasicBlockTargets { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this instance has an unspecified side effect.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
		/// </value>
		public virtual bool HasUnspecifiedSideEffect { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this instance has memory write side effect.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsMemoryWrite { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this instance has memory write side effect.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsMemoryRead { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this instance has IO operation side effect.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsIOOperation { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [variable operand count].
		/// </summary>
		/// <value>
		///   <c>true</c> if [variable operand count]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool VariableOperands { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="BaseInstruction"/> is commutative.
		/// </summary>
		/// <value>
		///   <c>true</c> if commutative; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsCommutative { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this instance is parameter load.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is parameter load; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsParameterLoad { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this instance is parameter store.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is parameter store; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsParameterStore { get { return false; } }

		public virtual bool IsPlatformInstruction { get { return false; } }

		public virtual bool IsIRInstruction { get { return false; } }

		/// <summary>
		/// Gets the name of the base instruction.
		/// </summary>
		/// <value>
		/// The name of the base instruction.
		/// </value>
		public virtual string Name
		{
			get
			{
				string name = GetType().ToString();

				int index = name.LastIndexOf('.');

				if (index > 0)
					name = name.Substring(index + 1);

				return name;
			}
		}

		public virtual string AlternativeName { get { return null; } }
		public virtual string FamilyName { get { return null; } }
		public virtual string Modifier { get { return null; } }

		private string CachedFullName { get; set; }

		public virtual string FullName
		{
			get
			{
				if (CachedFullName == null)
				{
					CachedFullName = FamilyName + "." + Name;
				}

				return CachedFullName;
			}
		}

		private string CachedFullAlternativeName { get; set; }

		public virtual string FullAlternativeName
		{
			get
			{
				if (AlternativeName == null)
					return null;

				if (CachedFullAlternativeName == null)
				{
					CachedFullAlternativeName = FamilyName + "." + AlternativeName;
				}

				return CachedFullAlternativeName;
			}
		}

		#endregion Properties

		#region Platform Properties

		public virtual bool IsZeroFlagUsed { get { return false; } }
		public virtual bool IsZeroFlagSet { get { return false; } }
		public virtual bool IsZeroFlagCleared { get { return false; } }
		public virtual bool IsZeroFlagModified { get { return false; } }
		public virtual bool IsZeroFlagUnchanged { get { return false; } }
		public virtual bool IsZeroFlagUndefined { get { return false; } }

		public virtual bool IsCarryFlagUsed { get { return false; } }
		public virtual bool IsCarryFlagSet { get { return false; } }
		public virtual bool IsCarryFlagCleared { get { return false; } }
		public virtual bool IsCarryFlagModified { get { return false; } }
		public virtual bool IsCarryFlagUnchanged { get { return false; } }
		public virtual bool IsCarryFlagUndefined { get { return false; } }

		public virtual bool IsSignFlagUsed { get { return false; } }
		public virtual bool IsSignFlagSet { get { return false; } }
		public virtual bool IsSignFlagCleared { get { return false; } }
		public virtual bool IsSignFlagModified { get { return false; } }
		public virtual bool IsSignFlagUnchanged { get { return false; } }
		public virtual bool IsSignFlagUndefined { get { return false; } }

		public virtual bool IsOverflowFlagUsed { get { return false; } }
		public virtual bool IsOverflowFlagSet { get { return false; } }
		public virtual bool IsOverflowFlagCleared { get { return false; } }
		public virtual bool IsOverflowFlagModified { get { return false; } }
		public virtual bool IsOverflowFlagUnchanged { get { return false; } }
		public virtual bool IsOverflowFlagUndefined { get { return false; } }

		public virtual bool IsParityFlagUsed { get { return false; } }
		public virtual bool IsParityFlagSet { get { return false; } }
		public virtual bool IsParityFlagCleared { get { return false; } }
		public virtual bool IsParityFlagModified { get { return false; } }
		public virtual bool IsParityFlagUnchanged { get { return false; } }
		public virtual bool IsParityFlagUndefined { get { return false; } }

		public virtual bool AreFlagUseConditional { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether [three two address conversion].
		/// </summary>
		/// <value>
		/// <c>true</c> if [three two address conversion]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool ThreeTwoAddressConversion { get { return false; } }

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
}
