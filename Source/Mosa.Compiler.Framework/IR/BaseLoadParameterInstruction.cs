// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Abstract base class for IR instructions that load parameters
	/// </summary>
	public abstract class BaseLoadParameterInstruction : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseLoadParameterInstruction"/>.
		/// </summary>
		public BaseLoadParameterInstruction() :
			base(1, 1)
		{
		}

		#endregion Construction
	}
}
