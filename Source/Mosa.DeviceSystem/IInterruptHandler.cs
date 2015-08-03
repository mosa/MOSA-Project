// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public interface IInterruptHandler
	{
		/// <summary>
		/// Gets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		byte IRQ { get; }

		/// <summary>
		/// Enables this instance.
		/// </summary>
		void Enable();

		/// <summary>
		/// Disables this instance.
		/// </summary>
		void Disable();
	}
}