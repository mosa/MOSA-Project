/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 */

using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceDrivers.PCI.VideoCard
{
    /// <summary>
    /// S3 Trio64 V2 Graphics Device Driver
    /// </summary>
    [PCIDeviceDriver(ClassCode = 0X03, SubClassCode = 0x00, ProgIF = 0x00, Platforms = PlatformArchitecture.Both_x86_and_x64)]
    public class S3Trio64V2 : HardwareDevice, IDevice, IPixelPaletteGraphicsDevice
    {
        /// <summary>
        /// 
        /// </summary>
        internal struct Register
        {
            /// <summary>
            /// 
            /// </summary>
            internal const ushort VgaEnable = 0x3c3;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort CrtcIndex = 0x3d4;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort CrtcData = 0x3d5;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort SequenceIndex = 0x3c4;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort SequenceData = 0x3c5;
        }

        /// <summary>
        /// 
        /// </summary>
        internal struct CommandRegister
        {
            /// <summary>
            /// 
            /// </summary>
            internal const ushort CursorBytes = 2084;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort AdvFuncCntl = 0x4ae8;
        }

        /// <summary>
        /// 
        /// </summary>
        internal enum DisplayModeState
        {
            /// <summary>
            /// 
            /// </summary>
            On          = 0x00,
            /// <summary>
            /// 
            /// </summary>
            StandBy     = 0x10,
            /// <summary>
            /// 
            /// </summary>
            Suspend     = 0x40,
            /// <summary>
            /// 
            /// </summary>
            Off         = 0x50,
        }

        #region Members
        /// <summary>
        /// 
        /// </summary>
        protected IMemory memory;

        /// <summary>
        /// 
        /// </summary>
        protected IFrameBuffer frameBuffer;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort lfbControllerIndex;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort enhMapControllerIndex;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort vgaEnableController;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort crtcControllerIndex;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort crtcControllerData;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort seqControllerIndex;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort seqControllerData;

        /// <summary>
        /// 
        /// </summary>
        protected IWriteOnlyIOPort outportWrite;
        #endregion

        #region Properties, PixelPaletteGraphicsDevice
        /// <summary>
        /// Gets the size of the palette.
        /// </summary>
        /// <value>The size of the palette.</value>
        public ushort PaletteSize 
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <returns></returns>
        public ushort Width 
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <returns></returns>
        public ushort Height 
        {
            get
            {
                return 0;
            }
        }
        #endregion

        #region Construction
        /// <summary>
        /// 
        /// </summary>
        public S3Trio64V2()
        {
        }
        #endregion

        #region HardwareDevice
        /// <summary>
        /// Setups this hardware device driver
        /// </summary>
        /// <returns></returns>
        public override bool Setup(IHardwareResources hardwareResources)
        {
            // Store reference to hardware resources
            this.hardwareResources = hardwareResources;
            // Set the driver's name
            base.name = "S3Trio64V2";

            // Store portmanager
            byte portBar = (byte)(base.hardwareResources.IOPointRegionCount - 1);

            vgaEnableController = base.hardwareResources.GetIOPort(portBar, Register.VgaEnable);
            crtcControllerIndex = base.hardwareResources.GetIOPort(portBar, Register.CrtcIndex);
            crtcControllerData  = base.hardwareResources.GetIOPort(portBar, Register.CrtcData);
            seqControllerIndex = base.hardwareResources.GetIOPort(portBar, Register.SequenceIndex);
            seqControllerData = base.hardwareResources.GetIOPort(portBar, Register.SequenceData);

            // Everything went fine
            return true;
        }

        /// <summary>
        /// Starts this hardware device.
        /// </summary>
        /// <returns></returns>
        public override DeviceDriverStartStatus Start()
        {
            vgaEnableController.Write8((byte)(vgaEnableController.Read8() | 0x01));

            // Unlock system registers
            WriteCrtcRegister(0x38, 0x48);
            WriteCrtcRegister(0x39, 0xa5);

            WriteCrtcRegister(0x40, 0x01, 0x01);
            WriteCrtcRegister(0x35, 0x00, 0x30);
            WriteCrtcRegister(0x33, 0x20, 0x72);

            WriteCrtcRegister(0x86, 0x80);
            WriteCrtcRegister(0x90, 0x00);

            // Detect number of MB of installed RAM
            byte[] ramSizes = new byte[] { 4, 0, 3, 8, 2, 6, 1, 0 };
            int ramSizeMB = ramSizes[(ReadCrtcRegister(0x36) >> 5) & 0x7];

            // Setup video memory
            memory = base.hardwareResources.GetMemory((byte)(ramSizeMB * 1024 * 1024));

            base.deviceStatus = DeviceStatus.Online;
            return DeviceDriverStartStatus.Started;
        }

        /// <summary>
        /// Called when an interrupt is received.
        /// </summary>
        /// <returns></returns>
        public override bool OnInterrupt()
        {
            return true;
        }
        #endregion

        #region PixelPaletteGraphicsDevice
        /// <summary>
        /// Writes the pixel.
        /// </summary>
        /// <param name="colorIndex">Index of the color.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void WritePixel(byte colorIndex, ushort x, ushort y)
        {
        }

        /// <summary>
        /// Reads the pixel.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public byte ReadPixel(ushort x, ushort y)
        {
            return 0;
        }

        /// <summary>
        /// Clears device with the specified color index.
        /// </summary>
        /// <param name="colorIndex">Index of the color.</param>
        public void Clear(byte colorIndex)
        {
        }

        /// <summary>
        /// Sets the palette.
        /// </summary>
        /// <param name="colorIndex">Index of the color.</param>
        /// <param name="color">The color.</param>
        public void SetPalette(byte colorIndex, Color color)
        {
        }

        /// <summary>
        /// Gets the palette.
        /// </summary>
        /// <param name="colorIndex">Index of the color.</param>
        /// <returns></returns>
        public Color GetPalette(byte colorIndex)
        {
            return new Color();
        }
        #endregion

        #region S3Trio
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dpmsMode"></param>
        /// <returns></returns>
        private bool SetDisplayModeState(DisplayModeState dpmsMode)
        {
            // Unlock extended sequencer registers
            WriteSequenceRegister(0x08, 0x06);

            byte sr0D = (byte)(ReadSequenceRegister(0x0d) & 0x03);

            switch (dpmsMode)
            {
                case DisplayModeState.On:
                    break;
                case DisplayModeState.StandBy:
                    sr0D |= 0x10;
                    break;
                case DisplayModeState.Suspend:
                    sr0D |= 0x40;
                    break;
                case DisplayModeState.Off:
                    sr0D |= 0x50;
                    break;
                default:
                    return false;
            }

            WriteSequenceRegister(0x0d, sr0D);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DisplayModeState GetDisplayModeState()
        {
            DisplayModeState mode = DisplayModeState.On;

            switch (ReadSequenceRegister(0x0d) & 0x70)
            {
                case 0:
                    mode = DisplayModeState.On;
                    break;
                case 0x10:
                    mode = DisplayModeState.StandBy;
                    break;
                case 0x40:
                    mode = DisplayModeState.Suspend;
                    break;
                case 0x50:
                    mode = DisplayModeState.Off;
                    break;
                default:
                    break;
            }

            return mode;
        }
        #endregion

        #region Helper
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        private void WriteCrtcRegister(byte index, byte value)
        {
            crtcControllerIndex.Write8(index);
            crtcControllerData.Write8(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        private void WriteCrtcRegister(byte index, byte value, byte mask)
        {
            crtcControllerIndex.Write8(index);
            crtcControllerData.Write8((byte)((crtcControllerData.Read8() & ~mask) | (value & mask)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private byte ReadCrtcRegister(byte index)
        {
            crtcControllerIndex.Write8(index);
            return crtcControllerData.Read8();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        private void WriteSequenceRegister(byte index, byte value)
        {
            seqControllerIndex.Write8(index);
            seqControllerData.Write8(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        private void WriteSequenceRegister(byte index, byte value, byte mask)
        {
            seqControllerIndex.Write8(index);
            seqControllerData.Write8((byte)((crtcControllerData.Read8() & ~mask) | (value & mask)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private byte ReadSequenceRegister(byte index)
        {
            seqControllerIndex.Write8(index);
            return seqControllerData.Read8();
        }
        #endregion
    }
}