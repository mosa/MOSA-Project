/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public class EventRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="EventRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="name">The name.</param>
		/// <param name="eventType">Type of the event.</param>
		public EventRow(EventAttributes flags, HeapIndexToken name, Token eventType)
		{
			Flags = flags;
			Name = name;
			EventType = eventType;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public EventAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public HeapIndexToken Name { get; private set; }

		/// <summary>
		/// Gets the type of the event.
		/// </summary>
		/// <value>The type of the event.</value>
		public Token EventType { get; private set; }

		#endregion Properties
	}
}