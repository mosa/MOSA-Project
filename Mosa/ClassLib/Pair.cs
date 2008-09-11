/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.ClassLib
{
	/// <summary>
	/// Generic class container for two related classes
	/// </summary>
	/// <typeparam name="F"></typeparam>
	/// <typeparam name="S"></typeparam>
	public class Pair<F, S>
	{
        /// <summary>
        /// 
        /// </summary>
		public F First;

        /// <summary>
        /// 
        /// </summary>
		public S Second;

		/// <summary>
		/// Initializes a new instance of the <see cref="Pair&lt;F, S&gt;"/> class.
		/// </summary>
		/// <param name="first">The first.</param>
		/// <param name="second">The second.</param>
		public Pair(F first, S second)
		{
			this.First = first;
			this.Second = second;
		}
	}
}
