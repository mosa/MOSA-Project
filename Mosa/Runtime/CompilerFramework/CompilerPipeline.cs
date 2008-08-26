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

        public void Add(T stage)
        {
            if (null == stage)
                throw new ArgumentNullException(@"stage");

            _pipeline.Add(stage);
        }

        public void AddRange(IEnumerable<T> stages)
        {
            if (null == stages)
                throw new ArgumentNullException(@"stages");

            _pipeline.AddRange(stages);
        }

        public void Clear()
        {
            _pipeline.Clear();
        }

        public void Execute(Action<T> action)
        {
            _currentStage = 0; 
            foreach (T stage in _pipeline)
            {
                action(stage);
                _currentStage++;
            }
        }

        public void Remove(T stage)
        {
            if (null == stage)
                throw new ArgumentNullException(@"stage");

            _pipeline.Remove(stage);
        }

        #endregion // Methods

        #region IEnumerable<T> members

        public IEnumerator<T> GetEnumerator()
        {
            return _pipeline.GetEnumerator();
        }

        #endregion // IEnumerable<T> members

        #region IEnumerable members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _pipeline.GetEnumerator();
        }

        #endregion // IEnumerable members

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
    }
}
