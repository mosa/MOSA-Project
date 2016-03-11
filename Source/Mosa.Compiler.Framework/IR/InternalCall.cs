// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of internal call context.
	/// </summary>
	public sealed class InternalCall : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InternalCall"/> class.
		/// </summary>
		public InternalCall()
			: base(0, 0)
		{
		}

		#endregion Construction
	}
}
