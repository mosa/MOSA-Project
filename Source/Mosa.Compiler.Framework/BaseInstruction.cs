// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Text;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base Instruction
	/// </summary>
	public abstract class BaseInstruction
	{
		#region Properties

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
		/// Gets a value indicating whether this instance has side effect.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
		/// </value>
		public virtual bool HasSideEffect { get { return true; } }

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
		/// Gets the name of the base instruction.
		/// </summary>
		/// <value>
		/// The name of the base instruction.
		/// </value>
		public virtual string BaseInstructionName
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

				index = name.IndexOf("Store");

				if (index < 0)
					index = name.IndexOf("Load");

				if (index > 0)
					name = name.Substring(0, index);

				return name;
			}
		}

		/// <summary>
		/// Gets the name of the instruction family.
		/// </summary>
		/// <value>
		/// The name of the instruction family.
		/// </value>
		public abstract string InstructionFamilyName { get; }

		/// <summary>
		/// Gets the name of the instruction extension.
		/// </summary>
		/// <value>
		/// The name of the instruction extension.
		/// </value>
		public virtual string InstructionExtensionName
		{
			get
			{
				string name = GetType().ToString();
				string ext = string.Empty;

				int index = name.LastIndexOf('.');

				name = name.Substring(index + 1);

				if (name.StartsWith("Store"))
					return string.Empty;
				else if (name.StartsWith("Load"))
					return string.Empty;
				else if (name.EndsWith("Store"))
					ext = "Store";
				else if (name.EndsWith("Load"))
					ext = "Load";

				return ext;
			}
		}

		private string CachedInstructionName { get; set; }

		/// <summary>
		/// Gets the name of the instruction.
		/// </summary>
		/// <value>
		/// The name of the instruction.
		/// </value>
		public virtual string InstructionName
		{
			get
			{
				if (CachedInstructionName == null)
				{
					string name = InstructionFamilyName + "." + BaseInstructionName;

					if (!string.IsNullOrWhiteSpace(InstructionExtensionName))
					{
						name = CachedInstructionName + "." + InstructionExtensionName;
					}

					CachedInstructionName = name;
				}

				return CachedInstructionName;
			}
		}

		public virtual string Modifier { get { return null; } }

		#endregion Properties

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
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return InstructionName;
		}

		#endregion Methods
	}
}
