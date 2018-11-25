// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	/// Base Platform Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseInstruction" />
	public abstract class BasePlatformInstruction : BaseInstruction
	{
		#region Properties

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

		/// <summary>
		/// Gets a value indicating whether [three two address conversion].
		/// </summary>
		/// <value>
		/// <c>true</c> if [three two address conversion]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool ThreeTwoAddressConversion { get { return false; } }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BasePlatformInstruction"/> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		protected BasePlatformInstruction(byte resultCount, byte operandCount)
			: base(resultCount, operandCount)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		public abstract void Emit(InstructionNode node, BaseCodeEmitter emitter);

		#endregion Methods
	}
}
