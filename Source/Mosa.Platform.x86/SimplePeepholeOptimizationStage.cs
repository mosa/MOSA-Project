/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System.Diagnostics;

using Mosa.Compiler.Framework;


namespace Mosa.Platform.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SimplePeepholeOptimizationStage : BaseTransformationStage, IMethodCompilerStage, IPlatformStage, IPipelineStage
	{

		#region Window Class

		/// <summary>
		/// Window Class
		/// </summary>
		public class Window
		{
			private int _length;
			private Context[] _history;
			private int _size;

			/// <summary>
			/// Initializes a new instance of the <see cref="Window"/> class.
			/// </summary>
			/// <param name="length">The length.</param>
			public Window(int length)
			{
				_length = length;
				_history = new Context[length];
				_size = 0;
			}

			/// <summary>
			/// Gets the size.
			/// </summary>
			/// <value>The size.</value>
			public int Size
			{
				get { return _size; }
			}

			/// <summary>
			/// Nexts the specified CTX.
			/// </summary>
			/// <param name="ctx">The CTX.</param>
			public void Add(Context ctx)
			{
				for (int i = _length - 1; i > 0; i--)
					_history[i] = _history[i - 1];

				_history[0] = ctx.Clone();
				if (_size < _length)
					_size++;
			}

			/// <summary>
			/// Deletes the current.
			/// </summary>
			public void DeleteCurrent()
			{
				_history[0].Remove();
				_size--;
				for (int i = 0; i < _size; i++)
					_history[i] = _history[i + 1];
			}

			/// <summary>
			/// Deletes the previous.
			/// </summary>
			public void DeletePrevious()
			{
				_history[1].Remove();
				_size--;
				for (int i = 1; i < _size; i++)
					_history[i] = _history[i + 1];
			}

			/// <summary>
			/// Deletes the previous previous.
			/// </summary>
			public void DeletePreviousPrevious()
			{
				_history[2].Remove();
				_size--;
				for (int i = 2; i < _size; i++)
					_history[i] = _history[i + 1];
			}

			/// <summary>
			/// Gets the current.
			/// </summary>
			/// <value>The current.</value>
			public Context Current
			{
				get
				{
					if (_size == 0)
						return null;
					else
						return _history[0];
				}
			}

			/// <summary>
			/// Gets the previous.
			/// </summary>
			/// <value>The previous.</value>
			public Context Previous
			{
				get
				{
					if (_size < 2)
						return null;
					else
						return _history[1];
				}
			}

			/// <summary>
			/// Gets the previous previous.
			/// </summary>
			/// <value>The previous previous.</value>
			public Context PreviousPrevious
			{
				get
				{
					if (_size < 3)
						return null;
					else
						return _history[2];
				}
			}
		}

		#endregion  // Windows Class

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			Window window = new Window(5);

			foreach (BasicBlock block in basicBlocks)
				for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
					if (ctx.Instruction != null && !ctx.Ignore)
					{
						window.Add(ctx);

						RemoveNop(window);
						RemoveMultipleStores(window);
						RemoveSingleLineJump(window);
						ImproveBranchAndJump(window);
					}
		}

		#endregion // IMethodCompilerStage Members

		/// <summary>
		/// Removes the nop.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <returns></returns>
		private bool RemoveNop(Window window)
		{
			if (window.Size < 1)
				return false;

			if (!(window.Current.Instruction is Instructions.NopInstruction))
				return false;

			window.DeleteCurrent();

			return true;
		}

		/// <summary>
		/// Remove multiple occuring stores, for e.g. before:
		/// <code>
		/// mov eax, operand
		/// mov operand, eax
		/// </code>
		/// after:
		/// <code>
		/// mov eax, operand
		/// </code>
		/// </summary>
		/// <param name="window">The window.</param>
		/// <returns>True if an instruction has been removed</returns>
		private bool RemoveMultipleStores(Window window)
		{
			if (window.Size < 2)
				return false;

			if (window.Current.BasicBlock != window.Previous.BasicBlock)
				return false;

			if (!(window.Current.Instruction is Instructions.MovInstruction && window.Previous.Instruction is Instructions.MovInstruction))
				return false;

			if (!(window.Previous.Result == window.Current.Operand1 && window.Previous.Operand1 == window.Current.Result))
				return false;

			window.DeleteCurrent();

			return true;
		}

		/// <summary>
		/// Removes the single line jump.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <returns></returns>
		private bool RemoveSingleLineJump(Window window)
		{
			if (window.Size < 2)
				return false;

			if (!(window.Previous.Instruction is Instructions.JmpInstruction))
				return false;

			if (window.Current.BasicBlock == window.Previous.BasicBlock)
				return false;

			if (window.Previous.Branch.Targets[0] != window.Current.BasicBlock.Label)
				return false;

			window.DeletePrevious();

			return true;
		}

		/// <summary>
		/// Improves the branch and jump.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <returns></returns>
		private bool ImproveBranchAndJump(Window window)
		{
			if (window.Size < 3)
				return false;

			if (!(window.Previous.Instruction is Instructions.JmpInstruction))
				return false;

			if (!(window.PreviousPrevious.Instruction is Instructions.BranchInstruction))
				return false;

			if (window.Previous.BasicBlock != window.PreviousPrevious.BasicBlock)
				return false;

			if (window.Current.BasicBlock == window.Previous.BasicBlock)
				return false;

			if (window.PreviousPrevious.Branch.Targets[0] != window.Current.BasicBlock.Label)
				return false;

			Debug.Assert(window.PreviousPrevious.Branch.Targets.Length == 1);

			// Negate branch condition
			window.PreviousPrevious.ConditionCode = GetOppositeConditionCode(window.PreviousPrevious.ConditionCode);

			// Change branch target
			window.PreviousPrevious.Branch.Targets[0] = window.Previous.Branch.Targets[0];

			// Delete jump
			window.DeletePrevious();

			return true;
		}
	}
}
