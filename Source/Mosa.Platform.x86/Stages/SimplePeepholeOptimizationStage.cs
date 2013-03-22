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

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// </summary>
	public sealed class SimplePeepholeOptimizationStage : BaseTransformationStage, IMethodCompilerStage
	{
		#region Window Class

		/// <summary>
		/// Window Class
		/// </summary>
		private sealed class Window
		{
			private int length;
			private Context[] history;
			private int size;

			/// <summary>
			/// Initializes a new instance of the <see cref="Window"/> class.
			/// </summary>
			/// <param name="length">The length.</param>
			public Window(int length)
			{
				this.length = length;
				history = new Context[length];
				size = 0;
			}

			/// <summary>
			/// Gets the size.
			/// </summary>
			/// <value>The size.</value>
			public int Size
			{
				get { return size; }
			}

			/// <summary>
			/// Adds the specified context.
			/// </summary>
			/// <param name="context">The context.</param>
			public void Add(Context context)
			{
				for (int i = length - 1; i > 0; i--)
					history[i] = history[i - 1];

				history[0] = context.Clone();
				if (size < length)
					size++;
			}

			/// <summary>
			/// Deletes the current.
			/// </summary>
			public void DeleteCurrent()
			{
				history[0].Remove();
				size--;
				for (int i = 0; i < size; i++)
					history[i] = history[i + 1];
			}

			/// <summary>
			/// Deletes the previous.
			/// </summary>
			public void DeletePrevious()
			{
				history[1].Remove();
				size--;
				for (int i = 1; i < size; i++)
					history[i] = history[i + 1];
			}

			/// <summary>
			/// Deletes the previous previous.
			/// </summary>
			public void DeletePreviousPrevious()
			{
				history[2].Remove();
				size--;
				for (int i = 2; i < size; i++)
					history[i] = history[i + 1];
			}

			/// <summary>
			/// Gets the current.
			/// </summary>
			/// <value>The current.</value>
			public Context Current
			{
				get
				{
					if (size == 0)
						return null;
					else
						return history[0];
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
					if (size < 2)
						return null;
					else
						return history[1];
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
					if (size < 3)
						return null;
					else
						return history[2];
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
				for (Context ctx = CreateContext(block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
					if (!ctx.IsEmpty)
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

			if (!(window.Current.Instruction is Instructions.Nop))
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

			if (!(window.Current.Instruction is Instructions.Mov && window.Previous.Instruction is Instructions.Mov))
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

			if (!(window.Previous.Instruction is Instructions.Jmp))
				return false;

			if (window.Current.BasicBlock == window.Previous.BasicBlock)
				return false;

			if (window.Previous.BranchTargets[0] != window.Current.BasicBlock.Label)
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

			if (!(window.Previous.Instruction is Instructions.Jmp))
				return false;

			if (!(window.PreviousPrevious.Instruction is Instructions.Branch))
				return false;

			if (window.Previous.BasicBlock != window.PreviousPrevious.BasicBlock)
				return false;

			if (window.Current.BasicBlock == window.Previous.BasicBlock)
				return false;

			if (window.PreviousPrevious.BranchTargets[0] != window.Current.BasicBlock.Label)
				return false;

			Debug.Assert(window.PreviousPrevious.BranchTargets.Length == 1);

			// Negate branch condition
			window.PreviousPrevious.ConditionCode = GetOppositeConditionCode(window.PreviousPrevious.ConditionCode);

			// Change branch target
			window.PreviousPrevious.BranchTargets[0] = window.Previous.BranchTargets[0];

			// Delete jump
			window.DeletePrevious();

			return true;
		}
	}
}