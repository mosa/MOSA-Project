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

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct EventRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private EventAttributes flags;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken name;

		/// <summary>
		/// 
		/// </summary>
		private Token eventType;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="EventRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="name">The name.</param>
		/// <param name="eventType">Type of the event.</param>
		public EventRow(EventAttributes flags, HeapIndexToken name, Token eventType)
		{
			this.flags = flags;
			this.name = name;
			this.eventType = eventType;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public EventAttributes Flags
		{
			get { return flags; }
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public HeapIndexToken Name
		{
			get { return name; }
		}

		/// <summary>
		/// Gets the type of the event.
		/// </summary>
		/// <value>The type of the event.</value>
		public Token EventType
		{
			get { return eventType; }
		}

		#endregion // Properties
	}
}
