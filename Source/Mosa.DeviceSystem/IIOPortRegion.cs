// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public interface IIOPortRegion
	{
		/// <summary>
		/// Gets the base IO port.
		/// </summary>
		/// <value>The base IO port.</value>
		ushort BaseIOPort { get; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		ushort Size { get; }
	}
}