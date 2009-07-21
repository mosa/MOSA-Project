/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
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
    public sealed class CompilerPipeline<T> : IEnumerable<T> where T: class
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

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerPipeline{T}"/> class.
        /// </summary>
        public CompilerPipeline()
        {
            _pipeline = new List<T>();
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
            if (null == stage)
                throw new ArgumentNullException(@"stage");

            _pipeline.Add(stage);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="stages">The stages.</param>
        public void AddRange(IEnumerable<T> stages)
        {
            if (null == stages)
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
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Execute(Action<T> action)
        {
            _currentStage = 0; 
            foreach (T stage in _pipeline)
            {
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
            if (null == stage)
                throw new ArgumentNullException(@"stage");

            _pipeline.Remove(stage);
        }

        #endregion // Methods

        #region IEnumerable<T> members

        /// <summary>
        /// Gibt einen Enumerator zurück, der die Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.Generic.IEnumerator`1"/>, der zum Durchlaufen der Auflistung verwendet werden kann.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _pipeline.GetEnumerator();
        }

        #endregion // IEnumerable<T> members

        #region IEnumerable members

        /// <summary>
        /// Gibt einen Enumerator zurück, der eine Auflistung durchläuft.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Collections.IEnumerator"/>-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.
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
        public StageType Find<StageType>() where StageType: class
        {
            StageType result = default(StageType);
            foreach (object o in _pipeline)
            {
                result = o as StageType;
                if (null != result)
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="StageType"></typeparam>
        /// <param name="item"></param>
        public bool InsertBefore<StageType>(T item) where StageType: class
        {
            bool result = false;
            for (int i = 0; i < _pipeline.Count; ++i)
            {
                if (_pipeline[i].GetType() == typeof(StageType))
                {
                    _pipeline.Insert(i, item);
                    ++i;
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="StageType"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool InsertAfter<StageType>(T item) where StageType : class
        {
            bool result = false;
            for (int i = 0; i < _pipeline.Count; ++i)
            {
                if (_pipeline[i].GetType() == typeof(StageType))
                {
                    _pipeline.Insert(++i, item);
                    result = true;
                }
            }
            return result;
        }
    }
}
