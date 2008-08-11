/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Devices;
using Mosa.Devices.Kernel;
using Mosa.ClassLib;
using Mosa.Devices;

namespace Mosa.Devices.ISA
{
    [ISADeviceSignature(AutoLoad = true, BasePort = 0x03F8, PortRange = 8, IRQ = 4)]
    [ISADeviceSignature(AutoLoad = false, BasePort = 0x02F8, PortRange = 8, IRQ = 3)]
    [ISADeviceSignature(AutoLoad = false, BasePort = 0x03E8, PortRange = 8, IRQ = 4)]
    [ISADeviceSignature(AutoLoad = false, BasePort = 0x02E8, PortRange = 8, IRQ = 3)]
    public class SerialDriver : ISAHardwareDevice, IDevice, IHardwareDevice, ISerialDevice
    {
        protected IReadOnlyIOPort RBRBase; // Receive Buffer Register (read only)		
        protected IWriteOnlyIOPort THRBase; // Transmitter Holding Register (write only)		
        protected IReadWriteIOPort IERBase; // Interrupt Enable Register		
        protected IReadWriteIOPort DLLBase; // Divisor Latch (LSB and MSB)
        protected IReadWriteIOPort DLMBase;
        protected IReadOnlyIOPort IIRBase; // Interrupt Identification Register (read only)		
        protected IWriteOnlyIOPort FCRBase; // FIFO Control Register (write only, 16550+ only)		
        protected IReadWriteIOPort LCRBase; // Line Control Register		
        protected IReadWriteIOPort MCRBase; // Modem Control Register		
        protected IReadWriteIOPort LSRBase; // Line Status Register		
        protected IReadWriteIOPort MSRBase; // Modem Status Register		
        protected IReadWriteIOPort SCRBase; // Scratch Register (16450+ and some 8250s, special use with some boards)

        protected SpinLock spinLock;

        protected const ushort FIFOSize = 256;
        protected byte[] fifoBuffer;
        protected uint fifoStart;
        protected uint fifoEnd;

        protected ushort ioBase;

        #region Flags

        [Flags]
        private enum IER : byte
        {
            DR = 0x01, // Data ready, it is generated if data waits to be read by the CPU.
            THRE = 0x02, // THR Empty, this interrupt tells the CPU to write characters to the THR.
            SI = 0x04, // Status interrupt. It informs the CPU of occurred transmission errors during reception.
            MSI = 0x08 // Modem status interrupt. It is triggered whenever one of the delta-bits is set (see MSR).
        }

        [Flags]
        private enum FCR : byte
        {
            Enabled = 0x01, // FIFO enable.
            CLR_RCVR = 0x02, // Clear receiver FIFO. This bit is self-clearing.
            CLR_XMIT = 0x04, // Clear transmitter FIFO. This bit is self-clearing.
            DMA = 0x08, // DMA mode
            // Receiver FIFO trigger level
            TL1 = 0x00,
            TL4 = 0x40,
            TL8 = 0x80,
            TL14 = 0xC0,
        }

        [Flags]
        private enum LCR : byte
        {
            // Word length
            CS5 = 0x00, // 5bits
            CS6 = 0x01, // 6bits
            CS7 = 0x02, // 7bits
            CS8 = 0x03, // 8bits
            // Stop bit
            ST1 = 0x00, // 1
            ST2 = 0x04, // 2
            // Parity
            PNO = 0x00, // None
            POD = 0x08, // Odd
            PEV = 0x18, // Even
            PMK = 0x28, // Mark 
            PSP = 0x38, // Space

            BRK = 0x40,
            DLAB = 0x80,
        }

        [Flags]
        private enum MCR : byte
        {
            DTR = 0x01,
            RTS = 0x02,
            OUT1 = 0x04,
            OUT2 = 0x08,
            LOOP = 0x10,
        }

        [Flags]
        private enum LSR : byte
        {
            DR = 0x01, // Data Ready. Reset by reading RBR (but only if the RX FIFO is empty, 16550+).
            OE = 0x02, // Overrun Error. Reset by reading LSR. Indicates loss of data.
            PE = 0x04, // Parity Error. Indicates transmission error. Reset by LSR.
            FE = 0x08, // Framing Error. Indicates missing stop bit. Reset by LSR.
            BI = 0x10, // Break Indicator. Set if RxD is 'space' for more than 1 word ('break'). Reset by reading LSR.
            THRE = 0x20, // Transmitter Holding Register Empty. Indicates that a new word can be written to THR. Reset by writing THR.
            TEMT = 0x40, // Transmitter Empty. Indicates that no transmission is running. Reset by reading LSR.
        }

        [Flags]
        private enum MSR : byte
        {
            DCTS = 0x01,
            DDSR = 0x02,
            DRI = 0x04,
            DDCD = 0x08,
            CTS = 0x10,
            DSR = 0x20,
            RI = 0x40,
            DCD = 0x80
        }

        #endregion

        public SerialDriver() { }

        public override bool Setup()
        {
            base.name = "COM_0x" + ((uint)ioBase).ToString("X");

            RBRBase = base.isaBusResources.GetIOPortRegion(0).GetPort(0); // Receive Buffer Register (read only)
            THRBase = base.isaBusResources.GetIOPortRegion(0).GetPort(0); // Transmitter Holding Register (write only)
            IERBase = base.isaBusResources.GetIOPortRegion(0).GetPort(1); // Interrupt Enable Register
            DLLBase = base.isaBusResources.GetIOPortRegion(0).GetPort(0); // Divisor Latch (LSB and MSB)
            DLMBase = base.isaBusResources.GetIOPortRegion(0).GetPort(1);
            IIRBase = base.isaBusResources.GetIOPortRegion(0).GetPort(2); // Interrupt Identification Register (read only)
            FCRBase = base.isaBusResources.GetIOPortRegion(0).GetPort(2); // FIFO Control Register (write only, 16550+ only)
            LCRBase = base.isaBusResources.GetIOPortRegion(0).GetPort(3); // Line Control Register
            MCRBase = base.isaBusResources.GetIOPortRegion(0).GetPort(4); // Modem Control Register
            LSRBase = base.isaBusResources.GetIOPortRegion(0).GetPort(5); // Line Status Register
            MSRBase = base.isaBusResources.GetIOPortRegion(0).GetPort(6); // Modem Status Register
            SCRBase = base.isaBusResources.GetIOPortRegion(0).GetPort(7); // Scratch Register (16450+ and some 8250s, special use with some boards)

            this.fifoBuffer = new byte[FIFOSize];
            this.fifoStart = 0;
            this.fifoEnd = 0;

            return true;
        }

        public override bool Probe() { return true; }   // not implemented 

        public override bool Start()
        {
            ///TODO: auto detect - otherwise just assume one is there
            ///TODO: could use BIOS to help w/ detection; 0x0400-x0403 supply base address for COM1-4

            // Disable all UART interrupts
            IERBase.Write8(0x00);

            // Enable DLAB (set baud rate divisor)
            LCRBase.Write8((byte)LCR.DLAB);

            // Set Baud rate
            int baudRate = 115200;
            int divisor = 115200 / baudRate;
            DLLBase.Write8((byte)(divisor & 0xFF));
            DLMBase.Write8((byte)(divisor >> 8 & 0xFF));

            // Reset DLAB, Set 8 bits, no parity, one stop bit
            LCRBase.Write8((byte)(LCR.CS8 | LCR.ST1 | LCR.PNO));

            // Enable FIFO, clear them, with 14-byte threshold
            FCRBase.Write8((byte)(FCR.Enabled | FCR.CLR_RCVR | FCR.CLR_XMIT | FCR.TL14));

            // IRQs enabled, RTS/DSR set
            MCRBase.Write8((byte)(MCR.DTR | MCR.RTS | MCR.OUT2));

            // Interrupt when data received
            IERBase.Write8((byte)IER.DR);

            return true;
        }

        public override LinkedList<IDevice> CreateSubDevices() { return null; }

        public override bool OnInterrupt()
        {
            ReadSerial();
            return true;
        }

        protected void AddToFIFO(byte value)
        {
            uint next = fifoEnd + 1;

            if (next == FIFOSize)
                next = 0;

            if (next == fifoStart)
                return; // out of room

            fifoBuffer[next] = value;
            fifoEnd = next;
        }

        protected byte GetFromFIFO()
        {
            if (fifoEnd == fifoStart)
                return 0;	// should not happen

            byte value = fifoBuffer[fifoStart];

            fifoStart++;

            if (fifoStart == FIFOSize)
                fifoStart = 0;

            return value;
        }

        protected bool IsFIFODataAvailable()
        {
            return (fifoEnd != fifoStart);
        }

        protected bool IsFIFOFull()
        {
            if ((((fifoEnd + 1) == FIFOSize) ? 0 : fifoEnd + 1) == fifoStart)
                return true;
            else
                return false;
        }

        protected bool CanTransmit()
        {
            return ((LSRBase.Read8() & (byte)LSR.THRE) != 0);
        }

        public void Write(byte ch)
        {
            try {
                spinLock.Enter();

                while (!CanTransmit())
                    ;

                THRBase.Write8(ch);
            }
            finally {
                spinLock.Exit();
            }
        }

        protected bool CanRead()
        {
            return ((LSRBase.Read8()) & (byte)LSR.DR) != 0;
        }

        protected void ReadSerial()
        {
            try {
                spinLock.Enter();

                if (!IsFIFOFull())
                    while (CanRead())
                        AddToFIFO(RBRBase.Read8());
            }
            finally {
                spinLock.Exit();
            }
        }

        public void DisableDataReceivedInterrupt()
        {
            IER ier = (IER)(IERBase.Read8());
            ier &= ~IER.DR;
            IERBase.Write8((byte)ier);
        }

        public void EnableDataReceivedInterrupt()
        {
            byte ier = IERBase.Read8();
            ier |= (byte)IER.DR;
            IERBase.Write8(ier);
        }

        public int ReadByte()
        {
            try {
                spinLock.Enter();

                if (!IsFIFODataAvailable())
                    return -1;

                return GetFromFIFO();
            }
            finally {
                spinLock.Exit();
            }
        }
    }
}