// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Container class used to define the pipeline of a compiler.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class Pipeline<T> : IEnumerable<T> where T : class
	{
		#region Data Members

		/// <summary>
		/// The stages in the compiler pipeline.
		/// </summary>
		private readonly List<T> pipeline;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Pipeline{T}"/> class.
		/// </summary>
		public Pipeline()
		{
			pipeline = new List<T>();
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Returns the number of stages in the compiler pipeline.
		/// </summary>
		public int Count
		{
			get { return pipeline.Count; }
		}

		/// <summary>
		/// Retrieves the indexed compilation stage.
		/// </summary>
		/// <param name="index">The index of the compilation stage to return.</param>
		/// <returns>The compilation stage at the requested index.</returns>
		public T this[int index]
		{
			get { return pipeline[index]; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Adds the specified stage.
		/// </summary>
		/// <param name="stage">The stage.</param>
		public void Add(T stage)
		{
			pipeline.Add(stage);
		}

		public void AddAt(int at, IEnumerable<T> stages)
		{
			foreach (var stage in stages)
			{
				if (stage == null)
					continue;

				pipeline.Insert(at++, stage);
			}
		}

		/// <summary>
		/// Inserts the stage after StageType
		/// </summary>
		/// <typeparam name="StageType">The type of stage.</typeparam>
		/// <param name="stage">The stage.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void InsertAfterFirst<StageType>(T stage) where StageType : class, T
		{
			if (stage == null)
				return;

			for (int i = 0; i < pipeline.Count; i++)
			{
				if (pipeline[i] is StageType result)
				{
					pipeline.Insert(i + 1, stage);
					return;
				}
			}

			throw new ArgumentNullException("missing stage to insert at");
		}

		/// <summary>
		/// Inserts the stage after StageType
		/// </summary>
		/// <typeparam name="StageType">The type of stage.</typeparam>
		/// <param name="stage">The stage.</param>
		public void InsertAfterLast<StageType>(T stage) where StageType : class, T
		{
			if (stage == null)
				return;

			for (int i = pipeline.Count - 1; i >= 0; i--)
			{
				if (pipeline[i] is StageType result)
				{
					pipeline.Insert(i + 1, stage);
					return;
				}
			}

			throw new ArgumentNullException("missing stage to insert at");
		}

		/// <summary>
		/// Inserts the stage after StageType
		/// </summary>
		/// <typeparam name="StageType">The type of stage.</typeparam>
		/// <param name="stages">The stages.</param>
		/// <exception cref="ArgumentNullException"><paramref name="stages"/> is <c>null</c>.</exception>
		public void InsertAfterLast<StageType>(IEnumerable<T> stages) where StageType : class, T
		{
			for (int i = pipeline.Count - 1; i >= 0; i--)
			{
				if (pipeline[i] is StageType result)
				{
					AddAt(i + 1, stages);
					return;
				}
			}

			throw new ArgumentNullException("missing stage to insert at");
		}

		/// <summary>
		/// Inserts the stage before StageType
		/// </summary>
		/// <typeparam name="StageType">The type of stage.</typeparam>
		/// <param name="stage">The stage.</param>
		public void InsertBefore<StageType>(T stage) where StageType : class, T
		{
			if (stage == null)
				return;

			for (int i = 0; i < pipeline.Count; i++)
			{
				if (pipeline[i] is StageType result)
				{
					pipeline.Insert(i, stage);
					return;
				}
			}

			throw new ArgumentNullException("missing stage to insert before");
		}

		/// <summary>
		/// Inserts the stage before StageType
		/// </summary>
		/// <typeparam name="StageType">The type of stage.</typeparam>
		/// <param name="stages">The stages.</param>
		public void InsertBefore<StageType>(IEnumerable<T> stages) where StageType : class, T
		{
			for (int i = 0; i < pipeline.Count; i++)
			{
				if (pipeline[i] is StageType result)
				{
					AddAt(i, stages);
					return;
				}
			}

			throw new ArgumentNullException("missing stage to insert before");
		}

		/// <summary>
		/// Adds the specified stages.
		/// </summary>
		/// <param name="stages">The stages.</param>
		/// <exception cref="System.ArgumentNullException">@stages</exception>
		public void Add(IEnumerable<T> stages)
		{
			if (stages == null)
				throw new ArgumentNullException(nameof(stages));

			foreach (var stage in stages)
			{
				if (stage != null)
				{
					Add(stage);
				}
			}
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			pipeline.Clear();
		}

		#endregion Methods

		#region IEnumerable members

		public IEnumerator<T> GetEnumerator()
		{
			foreach (var item in pipeline)
			{
				yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion IEnumerable members

		/// <summary>
		/// Finds this instance.
		/// </summary>
		/// <typeparam name="StageType">The type of the stage type.</typeparam>
		/// <returns></returns>
		public StageType FindFirst<StageType>() where StageType : class
		{
			var result = default(StageType);
			foreach (var o in pipeline)
			{
				result = o as StageType;
				if (result != null)
					break;
			}

			return result;
		}
	}
}
