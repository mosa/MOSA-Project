// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Abstract base class for IR instructions that load parameters
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public abstract class BaseLoadParameterInstruction : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseLoadParameterInstruction" />.
		/// </summary>
		protected BaseLoadParameterInstruction() :
			base(1, 1)
		{
		}

		#endregion Construction
	}
}
