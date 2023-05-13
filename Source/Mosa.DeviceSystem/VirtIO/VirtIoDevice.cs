// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;
using Mosa.Runtime;

namespace Mosa.DeviceSystem.VirtIO;

public class VirtIoDevice
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

	public VirtIoQueue[] VirtQueues { get; private set; }

	public ConstrainedPointer DeviceConfigurationPointer { get; private set; }

	public uint DeviceConfigurationOffset { get; private set; }

	public VirtIoDevice(Device device)
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
				case VirtIoConfigurationCapabilities.Common:
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
				case VirtIoConfigurationCapabilities.Notify:
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
				case VirtIoConfigurationCapabilities.Isr: break;
				case VirtIoConfigurationCapabilities.Device:
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
				case VirtIoConfigurationCapabilities.Pci: break;
				case VirtIoConfigurationCapabilities.SharedMemory: break;
				case VirtIoConfigurationCapabilities.Vendor: break;
			}
		}

		buffer = GC.AllocateObject(512);
		status = GC.AllocateObject(1);

		Internal.MemorySet(buffer, 0, 512);

		status.Store8(0, 0);
	}

	public void StartInitialize()
	{
		if (Initialized)
			return;

		devBar.Write8(devOff + VirtIoPciCommonConfiguration.DeviceStatus, VirtIoFlags.Reset);

		while (devBar.Read8(devOff + VirtIoPciCommonConfiguration.DeviceStatus) != VirtIoFlags.Reset) {}

		devBar.Write8(devOff + VirtIoPciCommonConfiguration.DeviceStatus, (byte)(devBar.Read8(devOff + VirtIoPciCommonConfiguration.DeviceStatus) | VirtIoFlags.Acknowledge));
		devBar.Write8(devOff + VirtIoPciCommonConfiguration.DeviceStatus, (byte)(devBar.Read8(devOff + VirtIoPciCommonConfiguration.DeviceStatus) | VirtIoFlags.Driver));

		DeviceFeatures = devBar.Read32(devOff + VirtIoPciCommonConfiguration.DeviceFeature);
	}

	public void SelectFeatures(uint features)
	{
		if (Initialized)
			return;

		devBar.Write32(devOff + VirtIoPciCommonConfiguration.DeviceFeatureSelect, features);
	}

	public void EndInitialize()
	{
		if (Initialized)
			return;

		var featuresOk = (byte)(devBar.Read8(devOff + 20) | VirtIoFlags.FeaturesOk);

		devBar.Write8(devOff + VirtIoPciCommonConfiguration.DeviceStatus, featuresOk);

		if (devBar.Read8(devOff + VirtIoPciCommonConfiguration.DeviceStatus) != featuresOk)
		{
			HAL.DebugWriteLine("[" + devName + "] device does not support accepted features; abort");
			return;
		}

		var queues = devBar.Read16(devOff + VirtIoPciCommonConfiguration.NumQueues);

		VirtQueues = new VirtIoQueue[queues];

		for (ushort i = 0; i < queues; i++)
		{
			devBar.Write16(devOff + VirtIoPciCommonConfiguration.QueueSelect, i);

			var size = devBar.Read16(devOff + VirtIoPciCommonConfiguration.QueueSize);

			if (size == 0)
			{
				HAL.DebugWriteLine("[" + devName + "] virtqueue has size of 0");
				continue;
			}

			var queue = new VirtIoQueue(size);
			var queueBufferAddress = queue.Buffer.ToUInt64();

			devBar.Write64(devOff + VirtIoPciCommonConfiguration.QueueDesc, queueBufferAddress);
			devBar.Write64(devOff + VirtIoPciCommonConfiguration.QueueDriver, queueBufferAddress + queue.DescriptorSize);
			devBar.Write64(devOff + VirtIoPciCommonConfiguration.QueueDevice, queueBufferAddress + queue.AvailableSize);
			devBar.Write16(devOff + VirtIoPciCommonConfiguration.QueueEnable, 1);

			VirtQueues[i] = queue;
		}
	}

	public void Start()
	{
		if (Initialized)
			return;

		devBar.Write8(devOff + VirtIoPciCommonConfiguration.DeviceStatus, (byte)(devBar.Read8(devOff + VirtIoPciCommonConfiguration.DeviceStatus) | VirtIoFlags.DriverOk));

		Initialized = true;
	}

	public void NotifyQueue(ushort queue)
	{
		devBar.Write16(devOff + VirtIoPciCommonConfiguration.QueueSelect, queue);

		var queueNotifyOff = devBar.Read16(devOff + VirtIoPciCommonConfiguration.QueueNotifyOff);
		var offset = (byte)(notifyCapabilityOffset + queueNotifyOff * notifyOffMultiplier);

		notifyCapabilityBar.Write16(notifyCapabilityOffset + offset, queue);
	}

	public void SendHeader(ushort queue, Pointer header, uint headerLength)
	{
		var virtQueue = VirtQueues[queue];

		var descriptor = virtQueue.NextDescriptor();
		virtQueue.DescriptorWrite64(descriptor, VirtIoQueueDescriptor.Phys, header.ToUInt64());
		virtQueue.DescriptorWrite32(descriptor, VirtIoQueueDescriptor.Len, headerLength);
		virtQueue.DescriptorWrite16(descriptor, VirtIoQueueDescriptor.Flags, VirtIoQueueDescriptorFlags.HasNext);

		var descriptor2 = virtQueue.NextDescriptor();
		virtQueue.DescriptorWrite64(descriptor2, VirtIoQueueDescriptor.Phys, buffer.ToUInt64());
		virtQueue.DescriptorWrite32(descriptor2, VirtIoQueueDescriptor.Len, 512);
		virtQueue.DescriptorWrite16(descriptor2, VirtIoQueueDescriptor.Flags, VirtIoQueueDescriptorFlags.HasNext | VirtIoQueueDescriptorFlags.Write);

		var descriptor3 = virtQueue.NextDescriptor();
		virtQueue.DescriptorWrite64(descriptor3, VirtIoQueueDescriptor.Phys, status.ToUInt64());
		virtQueue.DescriptorWrite32(descriptor3, VirtIoQueueDescriptor.Len, 1);
		virtQueue.DescriptorWrite16(descriptor3, VirtIoQueueDescriptor.Flags, VirtIoQueueDescriptorFlags.Write);

		virtQueue.SetHead(descriptor);

		NotifyQueue(queue);

		while (virtQueue.UsedRingRead16(VirtIoQueueUsedRing.Index) == 0) {}
	}
}
