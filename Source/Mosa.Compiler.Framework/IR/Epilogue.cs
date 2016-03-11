// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// An abstract intermediate representation of the method epilogue.
	/// </summary>
	/// <remarks>
	/// This instruction is usually derived by the architecture and expanded appropriately
	/// for the calling convention of the method.
	/// </remarks>
	public sealed class Epilogue : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Epilogue"/>.
		/// </summary>
		public Epilogue() :
			base(0, 0)
		{
		}

		#endregion Construction
	}
}
