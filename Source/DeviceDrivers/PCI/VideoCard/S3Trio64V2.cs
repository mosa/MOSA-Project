/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceDrivers.PCI.VideoCard
{
    /// <summary>
    /// S3 Trio64 V2 Graphics Device Driver
    /// </summary>
    [PCIDeviceDriver(VendorID = 0x5333, DeviceID = 0x8811, Platforms = PlatformArchitecture.X86AndX64)]
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
            internal const ushort VgaEnable = 0x13;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort MiscOutRead = 0x1c;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort MiscOutWrite = 0x12;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort CrtcIndex = 0x24;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort CrtcData = 0x25;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort SequenceIndex = 0x14;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort SequenceData = 0x15;
        }

        /// <summary>
        /// 
        /// </summary>
        internal struct CommandRegister
        {
            /// <summary>
            /// The cursor normally needs 1024 bytes. But if 1024 bytes
            /// are used, some Trio64 chips draw a short white horizontal
            /// line below and to the right. Setting the number of bytes
            /// to 2048 solves it.
            /// </summary>
            internal const ushort CursorBytes = 2084;
            /// <summary>
            /// 
            /// </summary>
            internal const ushort AdvFuncCntl = 0x4738;
        }

        /// <summary>
        /// 
        /// </summary>
        internal enum DisplayModeState
        {
            /// <summary>
            /// Display is turned on
            /// </summary>
            On          = 0x00,
            /// <summary>
            /// Display is on standby
            /// </summary>
            StandBy     = 0x10,
            /// <summary>
            /// Display is in suspend mode
            /// </summary>
            Suspend     = 0x40,
            /// <summary>
            /// Display is turned off
            /// </summary>
            Off         = 0x50,
            /// <summary>
            /// Used when current modestate is unknown
            /// </summary>
            Unknown     = 0xFF,
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
        protected IReadWriteIOPort miscOutputReader;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort miscOutputWriter;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort crtcControllerIndex;

        /// <summary>
        /// 
        /// </summary>
        protected IReadWriteIOPort crtcControllerData;

        /// <summary>
        /// IOPort to index sequence registers
        /// </summary>
        protected IReadWriteIOPort seqControllerIndex;

        /// <summary>
        /// IOPort to write data to the sequence registers
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
            miscOutputReader = base.hardwareResources.GetIOPort(portBar, Register.MiscOutRead);
            miscOutputWriter = base.hardwareResources.GetIOPort(portBar, Register.MiscOutWrite);
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

            // Enable colors
            miscOutputWriter.Write8((byte)(miscOutputReader.Read8() | 0x01));

            // Enable MMIO
            crtcControllerIndex.Write8(0x53);
            crtcControllerData.Write8((byte)(crtcControllerData.Read8() | 0x8));

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

            // Detect current mclk
            WriteSequenceRegister(0x08, 0x06);
            byte m = (byte)(ReadSequenceRegister(0x11) & 0x7f);
            byte n = ReadSequenceRegister(0x10);
            byte n1 = (byte)(n & 0x1f);
            byte n2 = (byte)((n >> 5) & 0x03);

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
                case DisplayModeState.Unknown:
                    return false;
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
            DisplayModeState mode = DisplayModeState.Unknown;

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
        /// Write a value to the indexed sequence register.
        /// </summary>
        /// <param name="index">The index to the register</param>
        /// <param name="value">Value to write</param>
        private void WriteSequenceRegister(byte index, byte value)
        {
            // Select target register
            seqControllerIndex.Write8(index);
            // Write masked value to register
            seqControllerData.Write8(value);
        }

        /// <summary>
        /// Write a masked value to the indexed sequence register.
        /// </summary>
        /// <param name="index">The index to the register</param>
        /// <param name="value">Value to write</param>
        /// <param name="mask">Mask for the value</param>
        private void WriteSequenceRegister(byte index, byte value, byte mask)
        {
            // Select target register
            seqControllerIndex.Write8(index);
            // Write masked value to register
            seqControllerData.Write8((byte)((crtcControllerData.Read8() & ~mask) | (value & mask)));
        }

        /// <summary>
        /// Reads in the sequence register's current value. The register is chosen by
        /// the given index.
        /// </summary>
        /// <param name="index">The index to the register</param>
        /// <returns>The register's value</returns>
        private byte ReadSequenceRegister(byte index)
        {
            // Select register
            seqControllerIndex.Write8(index);
            // Read contained value
            return seqControllerData.Read8();
        }
        #endregion
    }
}