/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;
using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.PCI;
using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers.ISA.VideoCards
{
    [ISADeviceSignature(AutoLoad = true, BasePort = 0x03B0, PortRange = 0x1F, BaseAddress = 0xB0000, AddressRange = 0x4000)]
    public class VGATextDriver : ISAHardwareDevice, IDevice, ITextDevice
    {
        #region Definitions

        internal struct CRTCommands
        {
            internal const byte HorizontalTotal = 0x00;
            internal const byte HorizontalDisplayed = 0x01;
            internal const byte HorizontalSyncPosition = 0x02;
            internal const byte HorizontalSyncPulseWidth = 0x03;
            internal const byte VerticalTotal = 0x04;
            internal const byte VerticalDisplayed = 0x05;
            internal const byte VerticalSyncPosition = 0x06;
            internal const byte VerticalSuncPulseWidth = 0x07;
            internal const byte InterlaceMode = 0x08;
            internal const byte MaximumScanLines = 0x09;
            internal const byte CursorStart = 0x0A;
            internal const byte CursorEnd = 0x0B;
            internal const byte StartAddress = 0x0C;
            internal const byte StartAddressHigh = 0x0C;
            internal const byte StartAddressLow = 0x0D;
            internal const byte CursorLocation = 0x0E;
            internal const byte CursorLocationHigh = 0x0E;
            internal const byte CursorLocationLow = 0x0F;
            internal const byte LightPen = 0x10;
            internal const byte LightPenHigh = 0x10;
            internal const byte LightPenLow = 0x11;
        }

        #endregion

        protected IReadWriteIOPort miscellaneousOutput;
        protected IReadWriteIOPort crtControllerIndex;
        protected IReadWriteIOPort crtControllerData;
        protected IReadWriteIOPort crtControllerDataLow;
        protected IReadWriteIOPort crtControllerIndexColor;
        protected IReadWriteIOPort crtControllerDataColor;
        protected IReadWriteIOPort crtControllerDataColorLow;

        protected byte[] memory;

        protected SpinLock spinLock;

        protected uint scanLine = 80;
        protected bool colorMode = false;
        protected uint offset = 0x8000;
        protected uint width = 80;
        protected uint height = 25;

        public VGATextDriver() { }
        public void Dispose() { }

        public override bool Setup()
        {
            base.name = "VGA";

            miscellaneousOutput = base.isaBusResources.GetIOPortRegion(0).GetPort((byte)(0x3cc - 0x3b0)); // 3b0
            crtControllerIndex = base.isaBusResources.GetIOPortRegion(0).GetPort((byte)(0x3d4 - 0x3b0)); // 3d4
            crtControllerData = base.isaBusResources.GetIOPortRegion(0).GetPort((byte)(0x3d5 - 0x3b0)); // 3d5
            crtControllerDataLow = base.isaBusResources.GetIOPortRegion(0).GetPort((byte)(0x3d5 - 0x3b0 + 1)); // 3d6
            crtControllerIndexColor = base.isaBusResources.GetIOPortRegion(0).GetPort((byte)(0x3b4 - 0x3b0)); // 3b4
            crtControllerDataColor = base.isaBusResources.GetIOPortRegion(0).GetPort((byte)(0x3b5 - 0x3b0)); // 3b5
            crtControllerDataColorLow = base.isaBusResources.GetIOPortRegion(0).GetPort((byte)(0x3d5 - 0x3b0 + 1)); // 3d6

            memory = base.isaBusResources.GetMemoryRegion(0).GetMemory();

            return true;
        }

        public override bool Start()
        {
            colorMode = ((miscellaneousOutput.Read8() & 1) == 1);

            if (colorMode) {
                offset = 0x8000;
                crtControllerIndexColor.Write8(CRTCommands.HorizontalDisplayed);
                width = crtControllerDataColor.Read8();
                crtControllerIndexColor.Write8(CRTCommands.VerticalDisplayed);
                height = crtControllerDataColor.Read8();
                width++;
                scanLine = width / 2;
            }
            else {
                offset = 0x0;
                crtControllerIndex.Write8(CRTCommands.HorizontalDisplayed);
                width = crtControllerData.Read8();
                crtControllerIndex.Write8(CRTCommands.VerticalDisplayed);
                height = crtControllerData.Read8();
                width++;
                scanLine = width;
            }

            height = 25; // override

            return true;
        }

        public override bool Probe() { return true; }
        public override LinkedList<IDevice> CreateSubDevices() { return null; }
        public override bool OnInterrupt() { return true; }

        public void WriteChar(ushort x, ushort y, char c, TextColor foreground, TextColor background)
        {
            uint index = y * scanLine + offset;

            if (colorMode) {
                index = (uint)(index + (x * 2));
                memory[index] = (byte)c;
                memory[index + 1] = (byte)((byte)foreground | ((byte)background << 4));
            }
            else {
                index = index + x;
                memory[index] = (byte)c;
            }
        }

        public void SetCursor(ushort x, ushort y)
        {
            ushort position = (ushort)(x + (y * width));

            if (colorMode) {
                crtControllerIndexColor.Write8(CRTCommands.CursorLocation);
                crtControllerDataColor.Write8((byte)((position >> 8) & 0xff));
                crtControllerDataColorLow.Write8((byte)(position & 0xff));
            }
            else {
                crtControllerIndex.Write8(CRTCommands.CursorLocation);
                crtControllerData.Write8((byte)((position >> 8) & 0xff));
                crtControllerDataLow.Write8((byte)(position & 0xff));
            }
        }
    }
}
