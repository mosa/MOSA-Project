// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of call context.
	/// </summary>
	public sealed class Call : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Call"/> class.
		/// </summary>
		public Call()
			: base(0, 0)
		{
		}

		#endregion Construction
	}
}
