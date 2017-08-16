// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of a virtual call.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class CallDynamic : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CallDynamic" /> class.
		/// </summary>
		public CallDynamic()
			: base(0, 0)
		{
		}

		#endregion Construction
	}
}
