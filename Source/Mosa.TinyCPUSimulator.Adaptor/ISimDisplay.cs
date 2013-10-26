/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.Adaptor
{
	public interface ISimDisplay
	{
		void SetBitMap(object bitmap);	// using object to avoid referencing winforms

		bool Changed { get; set; }

		bool Resized { get; set; }
	}
}