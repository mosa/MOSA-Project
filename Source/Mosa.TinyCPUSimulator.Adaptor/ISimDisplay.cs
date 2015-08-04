// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.Adaptor
{
	public interface ISimDisplay
	{
		void SetBitMap(object bitmap);  // using object to avoid referencing winforms

		bool Changed { get; set; }

		bool Resized { get; set; }
	}
}
