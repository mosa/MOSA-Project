// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;
using Mosa.Runtime;

namespace Mosa.DeviceSystem.VirtIO;

public class VirtIODevice
{
	private readonly string devName;
	private readonly PCIDevice pciDevice;
	private readonly IPCIController pciController;
	private readonly ConstrainedPointer devBar;
	private readonly uint devOff;
	private readonly ConstrainedPointer notifyCapabilityBar;
	private readonly uint notifyCapabilityOffset;
	private readonly uint notifyOffMultiplier;

	private Pointer buffer;
	private Pointer status;

	public bool Initialized { get; private set; }

	public uint DeviceFeatures { get; private set; }

	public VirtIOQueue[] VirtQueues { get; private set; }

	public ConstrainedPointer DeviceConfigurationPointer { get; private set; }

	public uint DeviceConfigurationOffset { get; private set; }

	public VirtIODevice(Device device)
	{
		devName = device.Name;
		pciDevice = (PCIDevice)device.Parent.DeviceDriver;

		if (pciDevice.RevisionID < 1)
		{
			HAL.DebugWriteLine("[" + devName + "] pci revision id mismatch; abort");
			return;
		}

		pciController = (IPCIController)device.Parent.Parent.DeviceDriver;

		foreach (var capability in pciDevice.Capabilities)
		{
			if (capability.Capability != 0x09)
				continue;

			var configType = pciController.ReadConfig8(pciDevice, (byte)(capability.Offset + 3));
			var bar = pciController.ReadConfig8(pciDevice, (byte)(capability.Offset + 4));
			var offset = pciController.ReadConfig32(pciDevice, (byte)(capability.Offset + 8));

			switch (configType)
			{
				case VirtIOConfigurationCapabilities.Common:
					{
						var pciBar = pciDevice.BaseAddresses[bar];

						if (pciBar.Region == AddressType.Memory)
						{
							devBar = HAL.GetPhysicalMemory(pciBar.Address, pciBar.Size);
							devOff = offset;
						}
						else
						{
							HAL.DebugWriteLine("[" + devName + "] common configuration bar is io; abort");
							return;
						}

						break;
					}
				case VirtIOConfigurationCapabilities.Notify:
					{
						var pciBar = pciDevice.BaseAddresses[bar];

						if (pciBar.Region == AddressType.Memory)
						{
							notifyCapabilityBar = HAL.GetPhysicalMemory(pciBar.Address, pciBar.Size);
							notifyCapabilityOffset = offset;
						}
						else
						{
							HAL.DebugWriteLine("[" + devName + "] notify configuration bar is io; abort");
							return;
						}

						notifyOffMultiplier = pciController.ReadConfig32(pciDevice, (byte)(capability.Offset + 16));
						break;
					}
				case VirtIOConfigurationCapabilities.ISR: break;
				case VirtIOConfigurationCapabilities.Device:
					{
						var pciBar = pciDevice.BaseAddresses[bar];

						if (pciBar.Region == AddressType.Memory)
						{
							DeviceConfigurationPointer = HAL.GetPhysicalMemory(pciBar.Address, pciBar.Size);
							DeviceConfigurationOffset = offset;
						}
						else
						{
							HAL.DebugWriteLine("[" + devName + "] device configuration bar is io; abort");
							return;
						}

						break;
					}
				case VirtIOConfigurationCapabilities.PCI: break;
				case VirtIOConfigurationCapabilities.SharedMemory: break;
				case VirtIOConfigurationCapabilities.Vendor: break;
			}
		}

		buffer = GC.AllocateObject(512); // FIXME - Not a GC object
		status = GC.AllocateObject(1); // FIXME

		Internal.MemorySet(buffer, 0, 512);

		status.Store8(0, 0);
	}

	public void StartInitialize()
	{
		if (Initialized)
			return;

		devBar.Write8(devOff + VirtIOPCICommonConfiguration.DeviceStatus, VirtIOFlags.Reset);

		while (devBar.Read8(devOff + VirtIOPCICommonConfiguration.DeviceStatus) != VirtIOFlags.Reset) { }

		devBar.Write8(devOff + VirtIOPCICommonConfiguration.DeviceStatus, (byte)(devBar.Read8(devOff + VirtIOPCICommonConfiguration.DeviceStatus) | VirtIOFlags.Acknowledge));
		devBar.Write8(devOff + VirtIOPCICommonConfiguration.DeviceStatus, (byte)(devBar.Read8(devOff + VirtIOPCICommonConfiguration.DeviceStatus) | VirtIOFlags.Driver));

		DeviceFeatures = devBar.Read32(devOff + VirtIOPCICommonConfiguration.DeviceFeature);
	}

	public void SelectFeatures(uint features)
	{
		if (Initialized)
			return;

		devBar.Write32(devOff + VirtIOPCICommonConfiguration.DeviceFeatureSelect, features);
	}

	public void EndInitialize()
	{
		if (Initialized)
			return;

		var featuresOk = (byte)(devBar.Read8(devOff + 20) | VirtIOFlags.FeaturesOk);

		devBar.Write8(devOff + VirtIOPCICommonConfiguration.DeviceStatus, featuresOk);

		if (devBar.Read8(devOff + VirtIOPCICommonConfiguration.DeviceStatus) != featuresOk)
		{
			HAL.DebugWriteLine("[" + devName + "] device does not support accepted features; abort");
			return;
		}

		var queues = devBar.Read16(devOff + VirtIOPCICommonConfiguration.NumQueues);

		VirtQueues = new VirtIOQueue[queues];

		for (ushort i = 0; i < queues; i++)
		{
			devBar.Write16(devOff + VirtIOPCICommonConfiguration.QueueSelect, i);

			var size = devBar.Read16(devOff + VirtIOPCICommonConfiguration.QueueSize);

			if (size == 0)
			{
				HAL.DebugWriteLine("[" + devName + "] virtqueue has size of 0");
				continue;
			}

			var queue = new VirtIOQueue(size);
			var queueBufferAddress = queue.Buffer.ToUInt64();

			devBar.Write64(devOff + VirtIOPCICommonConfiguration.QueueDesc, queueBufferAddress);
			devBar.Write64(devOff + VirtIOPCICommonConfiguration.QueueDriver, queueBufferAddress + queue.DescriptorSize);
			devBar.Write64(devOff + VirtIOPCICommonConfiguration.QueueDevice, queueBufferAddress + queue.AvailableSize);
			devBar.Write16(devOff + VirtIOPCICommonConfiguration.QueueEnable, 1);

			VirtQueues[i] = queue;
		}
	}

	public void Start()
	{
		if (Initialized)
			return;

		devBar.Write8(devOff + VirtIOPCICommonConfiguration.DeviceStatus, (byte)(devBar.Read8(devOff + VirtIOPCICommonConfiguration.DeviceStatus) | VirtIOFlags.DriverOk));

		Initialized = true;
	}

	public void NotifyQueue(ushort queue)
	{
		devBar.Write16(devOff + VirtIOPCICommonConfiguration.QueueSelect, queue);

		var queueNotifyOff = devBar.Read16(devOff + VirtIOPCICommonConfiguration.QueueNotifyOff);
		var offset = (byte)(notifyCapabilityOffset + queueNotifyOff * notifyOffMultiplier);

		notifyCapabilityBar.Write16(notifyCapabilityOffset + offset, queue);
	}

	public void SendHeader(ushort queue, Pointer header, uint headerLength)
	{
		var virtQueue = VirtQueues[queue];

		var descriptor = virtQueue.NextDescriptor();
		virtQueue.DescriptorWrite64(descriptor, VirtIOQueueDescriptor.Phys, header.ToUInt64());
		virtQueue.DescriptorWrite32(descriptor, VirtIOQueueDescriptor.Len, headerLength);
		virtQueue.DescriptorWrite16(descriptor, VirtIOQueueDescriptor.Flags, VirtIOQueueDescriptorFlags.HasNext);

		var descriptor2 = virtQueue.NextDescriptor();
		virtQueue.DescriptorWrite64(descriptor2, VirtIOQueueDescriptor.Phys, buffer.ToUInt64());
		virtQueue.DescriptorWrite32(descriptor2, VirtIOQueueDescriptor.Len, 512);
		virtQueue.DescriptorWrite16(descriptor2, VirtIOQueueDescriptor.Flags, VirtIOQueueDescriptorFlags.HasNext | VirtIOQueueDescriptorFlags.Write);

		var descriptor3 = virtQueue.NextDescriptor();
		virtQueue.DescriptorWrite64(descriptor3, VirtIOQueueDescriptor.Phys, status.ToUInt64());
		virtQueue.DescriptorWrite32(descriptor3, VirtIOQueueDescriptor.Len, 1);
		virtQueue.DescriptorWrite16(descriptor3, VirtIOQueueDescriptor.Flags, VirtIOQueueDescriptorFlags.Write);

		virtQueue.SetHead(descriptor);

		NotifyQueue(queue);

		while (virtQueue.UsedRingRead16(VirtIOQueueUsedRing.Index) == 0) { }
	}
}
