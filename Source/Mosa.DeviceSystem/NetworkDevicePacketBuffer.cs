// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// NetworkDevicePacketBuffer
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
		/// The transmit queue
		/// </summary>
		protected LinkedList<byte[]> transmitQueue;

		/// <summary>
		/// The receive queue
		/// </summary>
		protected LinkedList<byte[]> receiveQueue;

		/// <summary>
		/// The maximum transmit queue
		/// </summary>
		protected uint maxTransmitQueue;

		/// <summary>
		/// The maximum receive queue
		/// </summary>
		protected uint maxReceiveQueue;

		/// <summary>
		/// The count transmit packets
		/// </summary>
		protected uint countTransmitPackets;

		/// <summary>
		/// The count receive packets
		/// </summary>
		protected uint countReceivePackets;

		/// <summary>
		/// The discarded transmit packets
		/// </summary>
		protected uint discardedTransmitPackets;

		/// <summary>
		/// The discarded receive packets
		/// </summary>
		protected uint discardedReceivePackets;

		/// <summary>
		/// The transmit lock
		/// </summary>
		protected object transmitLock = new object();

		/// <summary>
		/// The receive lock
		/// </summary>
		protected object receiveLock = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="NetworkDevicePacketBuffer"/> class.
		/// </summary>
		/// <param name="networkDevice">The network device.</param>
		public NetworkDevicePacketBuffer(INetworkDevice networkDevice)
		{
			this.networkDevice = networkDevice;
			maxTransmitQueue = 100;    // TODO: Lookup system default
			maxReceiveQueue = 100;     // TODO: Lookup system default
			transmitLock = new object();
			receiveLock = new object();
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
			transmitLock = new object();
			receiveLock = new object();
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
			lock (transmitLock)
			{
				if (transmitQueue.Count >= maxTransmitQueue)
					return false;

				transmitQueue.AddLast(data);
				countTransmitPackets++;

				return true;
			}
		}

		/// <summary>
		/// Get packet from device.
		/// </summary>
		/// <returns></returns>
		public byte[] GetPacketFromDevice()
		{
			lock (receiveLock)
			{
				if (receiveQueue.Count == 0)
					return null;

				byte[] data = receiveQueue.First.Value;
				receiveQueue.RemoveFirst();

				return data;
			}
		}

		/// <summary>
		/// Queues the packet for stack.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public bool QueuePacketForStack(byte[] data)
		{
			lock (receiveLock)
			{
				if (receiveQueue.Count > maxReceiveQueue)
				{
					discardedReceivePackets++;
					return false;
				}

				receiveQueue.AddLast(data);
				countReceivePackets++;

				return true;
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
			lock (receiveLock)
			{
				while (receiveQueue.Count != 0)
				{
					byte[] data = receiveQueue.First.Value;

					if (networkDevice.SendPacket(data))
						receiveQueue.RemoveFirst();
					else
						return;
				}
			}
		}
	}
}
