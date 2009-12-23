/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Container class used to define the pipeline of a compiler.
	/// </summary>
	public sealed class CompilerPipeline : IEnumerable
	{
		#region Data members

		/// <summary>
		/// Holds the current stage of execution of the pipeline.
		/// </summary>
		private int _currentStage = -1;

		/// <summary>
		/// The stages in the compiler pipeline.
		/// </summary>
		private List<IPipelineStage> _pipeline;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerPipeline"/> class.
		/// </summary>
		public CompilerPipeline()
		{
			_pipeline = new List<IPipelineStage>();
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the number of stages in the compiler pipeline.
		/// </summary>
		public int Count
		{
			get { return _pipeline.Count; }
		}

		/// <summary>
		/// Retrieves the index of the current stage of execution.
		/// </summary>
		public int CurrentStage
		{
			get { return _currentStage; }
		}

		/// <summary>
		/// Retrieves the indexed compilation stage.
		/// </summary>
		/// <param name="index">The index of the compilation stage to return.</param>
		/// <returns>The compilation stage at the requested index.</returns>
		public IPipelineStage this[int index]
		{
			get { return _pipeline[index]; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Adds the specified stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public void Add(IPipelineStage stage)
		{
			if (stage == null)
				throw new ArgumentNullException(@"stage");

			_pipeline.Add(stage);
		}

		/// <summary>
		/// Inserts the stage after StageType
		/// </summary>
		/// <typeparam name="StageType">The type of stage.</typeparam>
		/// <param name="stage">The stage.</param>
		public void InsertAfter<StageType>(IPipelineStage stage) where StageType : class, IPipelineStage
		{
			if (stage == null)
				throw new ArgumentNullException(@"stage");

			for (int i = _pipeline.Count - 1; i >= 0; i--) {
				StageType result = _pipeline[i] as StageType;
				if (result != null) {
					_pipeline.Insert(i, stage);
					return;
				}
			}

			throw new ArgumentNullException(@"missing stage to insert after");
		}

		/// <summary>
		/// Inserts the stage after StageType
		/// </summary>
		/// <typeparam name="StageType">The type of stage.</typeparam>
		/// <param name="stages">The stages.</param>
		public void InsertAfter<StageType>(IEnumerable<IPipelineStage> stages) where StageType : class, IPipelineStage 
		{
			if (stages == null)
				throw new ArgumentNullException(@"stage");

			for (int i = _pipeline.Count - 1; i >= 0; i--) {
				StageType result = _pipeline[i] as StageType;
				if (result != null) {
					_pipeline.InsertRange(i + 1, stages);
					return;
				}
			}

			throw new ArgumentNullException(@"missing stage to insert after");
		}

		/// <summary>
		/// Inserts the stage before StageType
		/// </summary>
		/// <typeparam name="StageType">The type of stage.</typeparam>
		/// <param name="stage">The stage.</param>
		public void InsertBefore<StageType>(IPipelineStage stage) where StageType : class, IPipelineStage
		{
			if (stage == null)
				throw new ArgumentNullException(@"stage");

			for (int i = 0; i < _pipeline.Count; i++) {
				StageType result = _pipeline[i] as StageType;
				if (result != null) {
					_pipeline.Insert(i, stage);
					return;
				}
			}

			throw new ArgumentNullException(@"missing stage to insert before");
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="stages">The stages.</param>
		public void AddRange(IEnumerable<IPipelineStage> stages)
		{
			if (stages == null)
				throw new ArgumentNullException(@"stages");

			_pipeline.AddRange(stages);
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			_pipeline.Clear();
		}

		/// <summary>
		/// Removes the specified stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public void Remove(IPipelineStage stage)
		{
			if (stage == null)
				throw new ArgumentNullException(@"stage");

			_pipeline.Remove(stage);
		}

		/// <summary>
		/// Executes the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		public void Execute<T>(Action<T> action)
		{
			_currentStage = 0;
			foreach (T stage in _pipeline) {
				action(stage);
				_currentStage++;
			}
		}

		#endregion // Methods

		#region IEnumerable members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _pipeline.GetEnumerator();
		}

		#endregion // IEnumerable members

		/// <summary>
		/// Finds this instance.
		/// </summary>
		/// <typeparam name="StageType">The type of the tage type.</typeparam>
		/// <returns></returns>
		public StageType FindFirst<StageType>() where StageType : class
		{
			StageType result = default(StageType);
			foreach (object o in _pipeline) {
				result = o as StageType;
				if (result != null)
					break;
			}

			return result;
		}

	}
}
