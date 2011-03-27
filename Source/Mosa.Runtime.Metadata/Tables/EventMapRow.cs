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
using System.IO;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct EventMapRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private MetadataToken _typeDef;

		/// <summary>
		/// 
		/// </summary>
		private MetadataToken _eventList;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="EventMapRow"/> struct.
		/// </summary>
		/// <param name="typeDef">The type def.</param>
		/// <param name="eventList">The event list.</param>
		public EventMapRow(MetadataToken typeDef, MetadataToken eventList)
		{
			_typeDef = typeDef;
			_eventList = eventList;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the type def.
		/// </summary>
		/// <value>The type def.</value>
		public MetadataToken TypeDef
		{
			get { return _typeDef; }
		}

		/// <summary>
		/// Gets the event list.
		/// </summary>
		/// <value>The event list.</value>
		public MetadataToken EventList
		{
			get { return _eventList; }
		}

		#endregion // Properties
	}
}
