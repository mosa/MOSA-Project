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
	public class EventMapRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="EventMapRow"/> struct.
		/// </summary>
		/// <param name="typeDef">The type def.</param>
		/// <param name="eventList">The event list.</param>
		public EventMapRow(Token typeDef, Token eventList)
		{
			TypeDef = typeDef;
			EventList = eventList;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the type def.
		/// </summary>
		/// <value>The type def.</value>
		public Token TypeDef { get; private set; }

		/// <summary>
		/// Gets the event list.
		/// </summary>
		/// <value>The event list.</value>
		public Token EventList { get; private set; }

		#endregion Properties
	}
}