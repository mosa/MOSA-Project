/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Compiler.Metadata.Tables
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
		private Token typeDef;

		/// <summary>
		/// 
		/// </summary>
		private Token eventList;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="EventMapRow"/> struct.
		/// </summary>
		/// <param name="typeDef">The type def.</param>
		/// <param name="eventList">The event list.</param>
		public EventMapRow(Token typeDef, Token eventList)
		{
			this.typeDef = typeDef;
			this.eventList = eventList;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the type def.
		/// </summary>
		/// <value>The type def.</value>
		public Token TypeDef
		{
			get { return typeDef; }
		}

		/// <summary>
		/// Gets the event list.
		/// </summary>
		/// <value>The event list.</value>
		public Token EventList
		{
			get { return eventList; }
		}

		#endregion // Properties
	}
}
