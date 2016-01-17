// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public class NetworkDevicePacketBuffer
	{
		/// <summary>
		/// Network Device Packet Buffer
		/// </summary>
		/// <remarks>
		/// This class setups a transmit and receive between buffers between the Network Device and the TCP stack.
		/// Network devices will not have to setup their own in-memory buffers when hardware buffers become full.
		/// </remarks>
		protected INetworkDevice networkDevice;

		/// <summary>
		///
		/// </summary>
		protected LinkedList<byte[]> transmitQueue;

		/// <summary>
		///
		/// </summary>
		protected LinkedList<byte[]> receiveQueue;

		/// <summary>
		///
		/// </summary>
		protected uint maxTransmitQueue;

		/// <summary>
		///
		/// </summary>
		protected uint maxReceiveQueue;

		/// <summary>
		///
		/// </summary>
		protected uint countTransmitPackets;

		/// <summary>
		///
		/// </summary>
		protected uint countReceivePackets;

		/// <summary>
		///
		/// </summary>
		protected uint discardedTransmitPackets;

		/// <summary>
		///
		/// </summary>
		protected uint discardedReceivePackets;

		/// <summary>
		///
		/// </summary>
		protected SpinLock transmitLock;

		/// <summary>
		///
		/// </summary>
		protected SpinLock receiveLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="NetworkDevicePacketBuffer"/> class.
		/// </summary>
		/// <param name="networkDevice">The network device.</param>
		public NetworkDevicePacketBuffer(INetworkDevice networkDevice)
		{
			this.networkDevice = networkDevice;
			maxTransmitQueue = 100;    // TODO: Lookup system default
			maxReceiveQueue = 100;     // TODO: Lookup system default
			transmitLock = new SpinLock();
			receiveLock = new SpinLock();
			countTransmitPackets = 0;
			countReceivePackets = 0;
			discardedTransmitPackets = 0;
			discardedReceivePackets = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetworkDevicePacketBuffer"/> class.
		/// </summary>
		/// <param name="networkDevice">The network device.</param>
		/// <param name="maxTransmitQueue">The max transmit queue.</param>
		/// <param name="maxReceiveQueue">The max receive queue.</param>
		public NetworkDevicePacketBuffer(INetworkDevice networkDevice, uint maxTransmitQueue, uint maxReceiveQueue)
		{
			this.networkDevice = networkDevice;
			this.maxReceiveQueue = maxReceiveQueue;
			this.maxTransmitQueue = maxTransmitQueue;
			transmitLock = new SpinLock();
			receiveLock = new SpinLock();
			countTransmitPackets = 0;
			countReceivePackets = 0;
			discardedTransmitPackets = 0;
			discardedReceivePackets = 0;
		}

		/// <summary>
		/// Sends the packet to device.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public bool SendPacketToDevice(byte[] data)
		{
			try
			{
				transmitLock.Enter();
				if (transmitQueue.Count >= maxTransmitQueue)
					return false;

				transmitQueue.AddLast(data);
				countTransmitPackets++;

				return true;
			}
			finally
			{
				transmitLock.Exit();
			}
		}

		/// <summary>
		/// Get packet from device.
		/// </summary>
		/// <returns></returns>
		public byte[] GetPacketFromDevice()
		{
			try
			{
				receiveLock.Enter();

				if (receiveQueue.Count == 0)
					return null;

				byte[] data = receiveQueue.First.Value;
				receiveQueue.RemoveFirst();

				return data;
			}
			finally
			{
				receiveLock.Exit();
			}
		}

		/// <summary>
		/// Queues the packet for stack.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public bool QueuePacketForStack(byte[] data)
		{
			try
			{
				receiveLock.Enter();

				if (receiveQueue.Count > maxReceiveQueue)
				{
					discardedReceivePackets++;
					return false;
				}

				receiveQueue.AddLast(data);
				countReceivePackets++;

				return true;
			}
			finally
			{
				receiveLock.Exit();
			}
		}

		/// <summary>
		/// Pulse
		/// </summary>
		public void Pulse()
		{
			// Push packets to the network devices
			SendPackets();
		}

		/// <summary>
		/// Sends the packets.
		/// </summary>
		protected void SendPackets()
		{
			try
			{
				receiveLock.Enter();

				while (receiveQueue.Count != 0)
				{
					byte[] data = receiveQueue.First.Value;

					if (networkDevice.SendPacket(data))
						receiveQueue.RemoveFirst();
					else
						return;
				}
			}
			finally
			{
				receiveLock.Exit();
			}
		}
	}
}
