using Mosa.DeviceSystem;

namespace Mosa.DeviceDrivers.PCI.VideoCard
{
    /// <summary>
    /// Intel HD Graphics driver
    /// </summary>
    [PCIDeviceDriver(ClassCode = 0X03, SubClassCode = 0x00, ProgIF = 0x00, Platforms = PlatformArchitecture.X86AndX64)]
    public partial class IntelHD : HardwareDevice, IPixelGraphicsDevice
    {
        #region Subclasses

        struct GfxObject
        {
            byte* cpuAddress;
            ulong gfxAddress;
        }

        struct GfxHeap
        {
            GfxObject storage;
            uint next;
            uint size;
        }

        #endregion

        #region IO and Memory

        private IMemory VramMemory;

        private IMemory SharedMemory;

        private IMemory PrivateMemory;

        #endregion

        public override bool Setup(IHardwareResources hardwareResources)
        {
            this.hardwareResources = hardwareResources;
            base.name = "Intel HD Graphics";

            byte gttmmadr = (byte)(base.hardwareResources.IOPointRegionCount - 3);
            byte gmadr = (byte)(base.hardwareResources.IOPointRegionCount - 2);
            byte iobase = (byte)(base.hardwareResources.IOPointRegionCount - 1);

            return true;
        }

        public override DeviceDriverStartStatus Start()
        {
            throw new System.NotImplementedException();
        }

        public override bool OnInterrupt()
        {
            throw new System.NotImplementedException();
        }

        public void WritePixel(Color color, ushort x, ushort y)
        {
            throw new System.NotImplementedException();
        }

        public Color ReadPixel(ushort x, ushort y)
        {
            throw new System.NotImplementedException();
        }

        public void Clear(Color color)
        {
            throw new System.NotImplementedException();
        }

        public ushort Width
        {
            get { throw new System.NotImplementedException(); }
        }

        public ushort Height
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
