// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.VirtIO;
using Mosa.Runtime;

namespace Mosa.DeviceDriver.PCI.VirtIO;

public class VirtIOGPU : BaseDeviceDriver, IGraphicsDevice
{
	private static class Features
	{
		public const byte VirGL = 0;
		public const byte EDID = 1;
		public const byte ResourceUUID = 2;
		public const byte ResourceBlob = 3;
		public const byte ContextInit = 4;
	}

	private static class Configuration
	{
		public const byte EventsRead = 0;
		public const byte EventsClear = 4;
		public const byte NumScanOuts = 8;
		public const byte NumCapSets = 12;
	}

	private static class Queues
	{
		public const byte ControlQueue = 0;
		public const byte CursorQueue = 1;
	}

	private static class ControlHeader
	{
		public const byte Type = 0;
		public const byte Flags = 4;
		public const byte FenceId = 8;
		public const byte ContextId = 16;
		public const byte RingIndex = 20;

		public const byte Size = 24;
	}

	private VirtIODevice virtIoDevice;
	private Pointer resourceHeader;
	private Pointer backingHeader;
	private Pointer linkHeader;
	private Pointer transferHeader;
	private Pointer flushHeader;

	public FrameBuffer32 FrameBuffer { get; private set; }

	public override void Initialize()
	{
		Device.Name = "VIRTIO_GPU";

		virtIoDevice = new VirtIODevice(Device);
		virtIoDevice.StartInitialize();

		var deviceFeatures = (uint)VirtIOFeatures.Version1;

		if ((virtIoDevice.DeviceFeatures & VirtIOFeatures.InOrder) != 0) deviceFeatures |= VirtIOFeatures.InOrder;

		virtIoDevice.SelectFeatures(deviceFeatures);
		virtIoDevice.EndInitialize();

		var scanOuts = virtIoDevice.DeviceConfigurationPointer.Read32(virtIoDevice.DeviceConfigurationOffset + Configuration.NumScanOuts);

		if (scanOuts > 1)
		{
			HAL.DebugWriteLine("Detected more than 1 scan out!");
			return;
		}

		virtIoDevice.Start();

		resourceHeader = GC.AllocateObject(40); // FIXME - Not a GC object
		backingHeader = GC.AllocateObject(48); // FIXME
		linkHeader = GC.AllocateObject(48); // FIXME
		transferHeader = GC.AllocateObject(56); // FIXME
		flushHeader = GC.AllocateObject(48); // FIXME

		Internal.MemorySet(resourceHeader, 0, 40);
		Internal.MemorySet(backingHeader, 0, 48);
		Internal.MemorySet(linkHeader, 0, 48);
		Internal.MemorySet(transferHeader, 0, 56);
		Internal.MemorySet(flushHeader, 0, 48);
	}

	public void SetMode(uint width, uint height)
	{
		const uint x = 0;
		const uint y = 0;

		// Create 2D resource
		resourceHeader.Store32(ControlHeader.Type, 0x0101);
		resourceHeader.Store32(24, 1);
		resourceHeader.Store32(28, 1); // Format
		resourceHeader.Store32(32, width);
		resourceHeader.Store32(36, height);
		virtIoDevice.SendHeader(Queues.ControlQueue, resourceHeader, 40);

		// Allocate framebuffer
		var frameBufferSize = width * height * 4;
		var frameBuffer = GC.AllocateObject(frameBufferSize); // -Not a GC object
		FrameBuffer = new FrameBuffer32(new ConstrainedPointer(frameBuffer, frameBufferSize), width, height, (xx, yy) => (yy * width + xx) * 4);

		// Attach it as backing storage to the resource
		backingHeader.Store32(ControlHeader.Type, 0x0106);
		backingHeader.Store32(24, 1);
		backingHeader.Store32(28, 1);
		backingHeader.Store64(32, frameBuffer.ToUInt64());
		backingHeader.Store32(40, frameBufferSize);
		virtIoDevice.SendHeader(Queues.ControlQueue, backingHeader, 48);

		// Link the scan out
		linkHeader.Store32(ControlHeader.Type, 0x0103);
		linkHeader.Store32(24, x);
		linkHeader.Store32(28, y);
		linkHeader.Store32(32, width);
		linkHeader.Store32(36, height);
		linkHeader.Store32(40, 0);
		linkHeader.Store32(44, 1);
		virtIoDevice.SendHeader(Queues.ControlQueue, linkHeader, 48);
	}

	public void Disable()
	{
		throw new NotImplementedException();
	}

	public void Enable()
	{
		throw new NotImplementedException();
	}

	public void Update(uint x, uint y, uint width, uint height)
	{
		// Transfer to host memory
		transferHeader.Store32(ControlHeader.Type, 0x0105);
		transferHeader.Store32(24, x);
		transferHeader.Store32(28, y);
		transferHeader.Store32(32, width);
		transferHeader.Store32(36, height);
		transferHeader.Store64(40, 0UL);
		transferHeader.Store32(48, 1);
		virtIoDevice.SendHeader(Queues.ControlQueue, transferHeader, 56);

		// Flush resource
		flushHeader.Store32(ControlHeader.Type, 0x0104);
		flushHeader.Store32(24, x);
		flushHeader.Store32(28, y);
		flushHeader.Store32(32, width);
		flushHeader.Store32(36, height);
		flushHeader.Store32(40, 1);
		virtIoDevice.SendHeader(Queues.ControlQueue, flushHeader, 48);
	}

	public void CopyRectangle(uint x, uint y, uint newX, uint newY, uint width, uint height)
	{
		throw new NotImplementedException();
	}

	public bool SupportsHardwareCursor()
	{
		return false;
	}

	public void DefineCursor(FrameBuffer32 image)
	{
		throw new NotImplementedException();
	}

	public void SetCursor(bool visible, uint x, uint y)
	{
		throw new NotImplementedException();
	}
}
