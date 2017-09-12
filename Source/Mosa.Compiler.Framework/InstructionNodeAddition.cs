// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Instruction Node Addition
	/// </summary>
	public sealed class InstructionNodeAddition
	{
		#region Properties

		/// <summary>
		/// Gets or sets the mosa method.
		/// </summary>
		/// <value>
		/// The mosa method.
		/// </value>
		public MosaMethod MosaMethod { get; set; }

		/// <summary>
		/// Gets or sets the mosa field.
		/// </summary>
		/// <value>
		/// The mosa field.
		/// </value>
		public MosaField MosaField { get; set; }

		/// <summary>
		/// Gets or sets the mosa type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public MosaType MosaType { get; set; }

		/// <summary>
		/// Gets or sets the invoke method.
		/// </summary>
		/// <value>
		/// The invoke method.
		/// </value>
		public MosaMethod InvokeMethod { get; set; }

		/// <summary>
		/// Gets or sets the phi blocks.
		/// </summary>
		/// <value>
		/// The phi blocks.
		/// </value>
		public List<BasicBlock> PhiBlocks { get; set; }

		/// <summary>
		/// Gets or sets  additional operands
		/// </summary>
		public Operand[] AdditionalOperands { get; set; }

		#endregion Properties
	}
}
