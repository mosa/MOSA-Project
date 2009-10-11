/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// The Reserver Block Order Stage reorders Blocks in reverse order. This stage is for testing use only.
	/// </summary>
	public class ReverseBlockOrderStage : BaseStage, IMethodCompilerStage, IBasicBlockOrder
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		protected BasicBlock[] _ordered;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Simple Trace Block Order"; }
		}

		/// <summary>
		/// Gets the ordered Blocks.
		/// </summary>
		/// <value>The ordered Blocks.</value>
		public BasicBlock[] OrderedBlocks { get { return _ordered; } }

		#endregion // Properties

		#region IMethodCompilerStage Members

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			// Retreive the first block
			BasicBlock first = FindBlock(-1);

			// Allocate list of ordered Blocks
			_ordered = new BasicBlock[BasicBlocks.Count];

			Debug.Assert(first.Index == 0);
			_ordered[0] = first;
			int orderBlockCnt = 1;

			for (int i = BasicBlocks.Count - 1; i > 0; i--)
				_ordered[orderBlockCnt++] = BasicBlocks[i];
		}

		/// <summary>
		/// Adds to pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<CIL.CilToIrTransformationStage>(this);
		}

		#endregion // Methods
	}
}
