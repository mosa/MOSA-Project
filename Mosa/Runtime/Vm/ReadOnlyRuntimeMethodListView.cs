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

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ReadOnlyRuntimeMethodListView :
        ReadOnlyRuntimeListView<RuntimeMethod>
    {
        #region Static members

        /// <summary>
        /// Provides an empty list definition.
        /// </summary>
        public static readonly ReadOnlyRuntimeMethodListView Empty = new ReadOnlyRuntimeMethodListView();

        #endregion // Static members

        #region Construction

        private ReadOnlyRuntimeMethodListView()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyRuntimeFieldListView"/>.
        /// </summary>
        /// <param name="firstIndex">The first index of the list view.</param>
        /// <param name="count">The number of elements in the list view.</param>
        public ReadOnlyRuntimeMethodListView(int firstIndex, int count)
            : base(firstIndex, count)
        {
        }

        #endregion // Construction

        #region Overrides

        /// <summary>
        /// Returns the methods array, which is viewed by this collection.
        /// </summary>
        /// <value></value>
        protected override RuntimeMethod[] Items
        {
            get { return RuntimeBase.Instance.TypeLoader.Methods; }
        }

        #endregion // Overrides
    }
}
