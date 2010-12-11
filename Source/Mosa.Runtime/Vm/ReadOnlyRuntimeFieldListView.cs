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
	public sealed class ReadOnlyRuntimeFieldListView :
		ReadOnlyRuntimeListView<RuntimeField>
	{
		#region Static members

		/// <summary>
		/// Provides an empty list definition.
		/// </summary>
		public static readonly ReadOnlyRuntimeFieldListView Empty = new ReadOnlyRuntimeFieldListView();

		#endregion // Static members

		#region Construction

		private ReadOnlyRuntimeFieldListView()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ReadOnlyRuntimeFieldListView"/>.
		/// </summary>
		/// <param name="start">The first index of the list view.</param>
		/// <param name="count">The number of elements in the list view.</param>
		internal ReadOnlyRuntimeFieldListView(IModuleTypeSystemInternalList moduleTypeSystemInternalList, int start, int count)
			: base(moduleTypeSystemInternalList, start, count)
		{
		}

		#endregion // Construction

		#region Overrides
		/// <summary>
		/// Returns the fields array, which is viewed by this collection.
		/// </summary>
		protected override RuntimeField[] Items
		{
			get { if (moduleTypeSystemInternalList == null) return null; else return moduleTypeSystemInternalList.Fields; }
		}

		#endregion // Overrides
	}
}
