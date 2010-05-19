/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	/// 
	/// </summary>
	public class Array
	{
        private int length;

        public int Length
        {
            get
            {
                return this.length;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public void SetValue(object value, int index)
		{
			// TODO
		}

		/// <summary>
		/// 
		/// </summary>
		public object GetValue(int index)
		{
			// TODO
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
		{
			for (int s = 0, d = destinationIndex; s < length; s++, d++) {
				sourceArray.SetValue(destinationArray.GetValue(d), s + sourceIndex);
			}
		}
	}
}
