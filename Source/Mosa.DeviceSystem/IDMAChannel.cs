// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public interface IDMAChannel
	{
		/// <summary>
		/// Setups the channel.
		/// </summary>
		/// <param name="mode">The mode.</param>
		/// <param name="type">The type.</param>
		/// <param name="auto">if set to <c>true</c> [auto].</param>
		/// <param name="count">The count.</param>
		void SetupChannel(DMAMode mode, DMATransferType type, bool auto, uint count);

		/// <summary>
		/// Transfers the out.
		/// </summary>
		/// <param name="count">The count.</param>
		/// <param name="data">The data.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		bool TransferOut(uint count, byte[] data, uint offset);

		/// <summary>
		/// Transfers the in.
		/// </summary>
		/// <param name="count">The count.</param>
		/// <param name="data">The data.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		bool TransferIn(uint count, byte[] data, uint offset);
	}

	/// <summary>
	///
	/// </summary>
	public enum DMAMode : byte
	{
		/// <summary>
		///
		/// </summary>
		ReadFromMemory,

		/// <summary>
		///
		/// </summary>
		WriteToMemory
	}

	/// <summary>
	///
	/// </summary>
	public enum DMATransferType : byte
	{
		/// <summary>
		///
		/// </summary>
		OnDemand,

		/// <summary>
		///
		/// </summary>
		Single,

		/// <summary>
		///
		/// </summary>
		Block,

		/// <summary>
		///
		/// </summary>
		CascadeMode
	}
}