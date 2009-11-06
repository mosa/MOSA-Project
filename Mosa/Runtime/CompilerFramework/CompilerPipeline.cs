/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Container class used to define the pipeline of a compiler.
	/// </summary>
	/// <typeparam name="T">The type of the stages of the compiler.</typeparam>
	public sealed class CompilerPipeline<T> : IEnumerable<T> where T : class
	{
		#region Data members

		/// <summary>
		/// Holds the current stage of execution of the pipeline.
		/// </summary>
		private int _currentStage;

		/// <summary>
		/// The stages in the compiler pipeline.
		/// </summary>
		private List<T> _pipeline;

		private Dictionary<T, List<Type>> _before;
		private Dictionary<T, List<Type>> _after;
		bool _ordered;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerPipeline{T}"/> class.
		/// </summary>
		public CompilerPipeline()
		{
			_pipeline = new List<T>();
			_before = new Dictionary<T, List<Type>>();
			_after = new Dictionary<T, List<Type>>();
			_ordered = true;
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
		public T this[int index]
		{
			get { return _pipeline[index]; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Adds the specified stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public void Add(T stage)
		{
			if (stage == null)
				throw new ArgumentNullException(@"stage");

			_pipeline.Add(stage);
			_ordered = false;
			
			// 
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="stages">The stages.</param>
		public void AddRange(IEnumerable<T> stages)
		{
			if (stages == null)
				throw new ArgumentNullException(@"stages");

			foreach (T stage in stages)
				Add(stage);
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			_pipeline.Clear();
			_before.Clear();
			_after.Clear();
			_ordered = true;
		}

		/// <summary>
		/// Executes the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		public void Execute(Action<T> action)
		{
			if (!_ordered) OrderPipeline();

			_currentStage = 0;
			foreach (T stage in _pipeline) {
				action(stage);
				_currentStage++;
			}
		}

		/// <summary>
		/// Removes the specified stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public void Remove(T stage)
		{
			if (stage == null)
				throw new ArgumentNullException(@"stage");

			_pipeline.Remove(stage);
			_after.Remove(stage);
			_before.Remove(stage);
			_ordered = false;
		}

		#endregion // Methods

		#region IEnumerable<T> members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			if (!_ordered) OrderPipeline();

			return _pipeline.GetEnumerator();
		}

		#endregion // IEnumerable<T> members

		#region IEnumerable members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			if (!_ordered) OrderPipeline();

			return _pipeline.GetEnumerator();
		}

		#endregion // IEnumerable members

		/// <summary>
		/// Finds this instance.
		/// </summary>
		/// <typeparam name="StageType">The type of the tage type.</typeparam>
		/// <returns></returns>
		public StageType Find<StageType>() where StageType : class
		{
			StageType result = default(StageType);
			foreach (object o in _pipeline) {
				result = o as StageType;
				if (result != null)
					break;
			}

			return result;
		}

		/// <summary>
		/// Finds the last.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <returns></returns>
		private int FindLast(List<Type> types)
		{
			if (types == null || types.Count == 0)
				return -1;

			for (int i = _pipeline.Count; i >= 0; i--)
				if (types.Contains(_pipeline[i].GetType()))
					return i;

			return -1;
		}

		/// <summary>
		/// Finds the first.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <returns></returns>
		private int FindFirst(List<Type> types)
		{
			if (types == null || types.Count == 0)
				return -1;

			for (int i = 0; i < _pipeline.Count; i++)
				if (types.Contains(_pipeline[i].GetType()))
					return i;

			return -1;
		}

		/// <summary>
		/// Orders the pipeline.
		/// </summary>
		/// <returns></returns>
		private bool OrderPipeline()
		{
			int loops = 0;
			bool changed = true;

			while (changed) {
				changed = false;
				loops++;

				for (int i = 0; i < _pipeline.Count; i++) {
					T stage = _pipeline[i];

					if (_after.ContainsKey(stage)) {
						int last = FindLast(_after[stage]);

						if (last != -1 && i < last) {
							_pipeline.Insert(last + 1, stage);
							_pipeline.RemoveAt(i);
							changed = true;
							break;
						}
					}

					if (_before.ContainsKey(stage)) {
						int first = FindFirst(_before[stage]);

						if (first == -1 && i > first) {
							_pipeline.Insert(first - 1, stage);
							_pipeline.RemoveAt(i + 1);
							changed = true;
							break;
						}
					}
				}

				Debug.Assert(loops < 1000, @"impossible ordering of stages");
			}

			_ordered = true;

			return true;
		}

		/// <summary>
		/// Runs the before.
		/// </summary>
		/// <typeparam name="StageType">The type of the tage type.</typeparam>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public void RunBefore<StageType>(T item) where StageType : class
		{
			if (!_before.ContainsKey(item))
				_before.Add(item, new List<Type>());

			_before[item].Add(typeof(StageType));
		}

		/// <summary>
		/// Runs the before.
		/// </summary>
		/// <typeparam name="StageType">The type of the tage type.</typeparam>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public void RunAfter<StageType>(T item) where StageType : class
		{
			if (!_after.ContainsKey(item))
				_after.Add(item, new List<Type>());

			_after[item].Add(typeof(StageType));
		}

	}
}
