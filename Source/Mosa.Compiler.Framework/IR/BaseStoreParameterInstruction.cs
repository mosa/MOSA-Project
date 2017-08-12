// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Abstract base class for IR instructions that saves parameters
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public abstract class BaseStoreParameterInstruction : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseStoreParameterInstruction" />.
		/// </summary>
		protected BaseStoreParameterInstruction() :
			base(2, 0)
		{
		}

		#endregion Construction
	}
}
