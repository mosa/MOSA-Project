/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// </summary>
	public sealed class InstructionNodeExtension
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
		/// Gets or sets the type of the mosa.
		/// </summary>
		/// <value>
		/// The type of the mosa.
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
		///  Holds the cil branch targets
		/// </summary>
		public int[] CILTargets;

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
