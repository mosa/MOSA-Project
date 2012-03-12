/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.All | System.AttributeTargets.Property, AllowMultiple = true)]
	public class PCIDeviceDriverAttribute : System.Attribute, IDeviceDriver
	{
		/// <summary>
		/// 
		/// </summary>
		protected PlatformArchitecture platforms;
		/// <summary>
		/// 
		/// </summary>
		//[System.Flags]
		public static class Field //: byte
		{
			/// <summary>
			/// 
			/// </summary>
			public const int DeviceID = 1;
			/// <summary>
			/// 
			/// </summary>
			public const int VendorID = 2;
			/// <summary>
			/// 
			/// </summary>
			public const int SubVendorID = 4;
			/// <summary>
			/// 
			/// </summary>
			public const int SubDeviceID = 8;
			/// <summary>
			/// 
			/// </summary>
			public const int RevisionID = 16;
			/// <summary>
			/// 
			/// </summary>
			public const int ProgIF = 32;
			/// <summary>
			/// 
			/// </summary>
			public const int ClassCode = 64;
			/// <summary>
			/// 
			/// </summary>
			public const int SubClassCode = 128;
		};

		/// <summary>
		/// 
		/// </summary>
		protected int fields;
		/// <summary>
		/// 
		/// </summary>
		protected ushort deviceID;
		/// <summary>
		/// 
		/// </summary>
		protected ushort vendorID;
		/// <summary>
		/// 
		/// </summary>
		protected ushort subVendorID;
		/// <summary>
		/// 
		/// </summary>
		protected ushort subDeviceID;
		/// <summary>
		/// 
		/// </summary>
		protected byte revisionID;
		/// <summary>
		/// 
		/// </summary>
		protected byte progIF;
		/// <summary>
		/// 
		/// </summary>
		protected ushort classCode;
		/// <summary>
		/// 
		/// </summary>
		protected byte subClassCode;

		/// <summary>
		/// 
		/// </summary>
		protected uint memoryRequired = 0;

		/// <summary>
		/// 
		/// </summary>
		protected uint memoryAlignment = 4096; // on 4k boundary (default), must be a value in 2^n

		/// <summary>
		/// </summary>
		/// <value>The sub class code.</value>
		public PlatformArchitecture Platforms { get { return platforms; } set { platforms = value; } }
		/// <summary>
		/// Gets or sets the device ID.
		/// </summary>
		/// <value>The device ID.</value>
		public ushort DeviceID { get { return deviceID; } set { deviceID = value; fields = fields | Field.DeviceID; } }
		/// <summary>
		/// Gets or sets the vendor ID.
		/// </summary>
		/// <value>The vendor ID.</value>
		public ushort VendorID { get { return vendorID; } set { vendorID = value; fields = fields | Field.VendorID; } }
		/// <summary>
		/// Gets or sets the sub vendor ID.
		/// </summary>
		/// <value>The sub vendor ID.</value>
		public ushort SubVendorID { get { return subVendorID; } set { subVendorID = value; fields = fields | Field.SubVendorID; } }
		/// <summary>
		/// Gets or sets the sub device ID.
		/// </summary>
		/// <value>The sub device ID.</value>
		public ushort SubDeviceID { get { return subDeviceID; } set { subDeviceID = value; fields = fields | Field.SubDeviceID; } }
		/// <summary>
		/// Gets or sets the revision ID.
		/// </summary>
		/// <value>The revision ID.</value>
		public byte RevisionID { get { return revisionID; } set { revisionID = value; fields = fields | Field.RevisionID; } }
		/// <summary>
		/// Gets or sets the prog IF.
		/// </summary>
		/// <value>The prog IF.</value>
		public byte ProgIF { get { return progIF; } set { progIF = value; fields = fields | Field.ProgIF; } }
		/// <summary>
		/// Gets or sets the class code.
		/// </summary>
		/// <value>The class code.</value>
		public ushort ClassCode { get { return classCode; } set { classCode = value; fields = fields | Field.ClassCode; } }
		/// <summary>
		/// Gets or sets the sub class code.
		/// </summary>
		/// <value>The sub class code.</value>
		public byte SubClassCode { get { return subClassCode; } set { subClassCode = value; fields = fields | Field.SubClassCode; } }

		/// <summary>
		/// 
		/// </summary>
		protected const int Priority1 = Field.VendorID | Field.DeviceID | Field.SubVendorID | Field.SubDeviceID | Field.RevisionID;
		/// <summary>
		/// 
		/// </summary>
		protected const int Priority2 = Field.VendorID | Field.DeviceID | Field.SubVendorID | Field.SubDeviceID;
		/// <summary>
		/// 
		/// </summary>
		protected const int Priority3 = Field.VendorID | Field.DeviceID | Field.RevisionID;
		/// <summary>
		/// 
		/// </summary>
		protected const int Priority4 = Field.VendorID | Field.DeviceID;
		/// <summary>
		/// 
		/// </summary>
		protected const int Priority5 = Field.ClassCode | Field.SubClassCode | Field.ProgIF;
		/// <summary>
		/// 
		/// </summary>
		protected const int Priority6 = Field.ClassCode | Field.SubClassCode;

		/// <summary>
		/// Gets the priority.
		/// </summary>
		/// <value>The priority.</value>
		public int Priority
		{
			get
			{
				//MOSA will use the following PCI binding order:
				//1. Bus + Vendor ID + Device ID + Sub. Vendor ID + SubSystem ID + Revision ID 
				//2. Bus + Vendor ID + Device ID + Sub. Vendor ID + SubSystem ID 
				//3. Bus + Vendor ID + Device ID + Revision ID 
				//4. Bus + Vendor ID + Device ID 
				//5. Bus + Class Code + Sub. Class + Prog Interface 
				//6. Bus + Class Code + Sub. Class 

				if ((fields & Priority1) == Priority1) return 1;
				if ((fields & Priority2) == Priority2) return 2;
				if ((fields & Priority3) == Priority3) return 3;
				if ((fields & Priority4) == Priority4) return 4;
				if ((fields & Priority5) == Priority5) return 5;
				if ((fields & Priority6) == Priority6) return 6;
				return 0; // no binding
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PCIDeviceDriverAttribute"/> class.
		/// </summary>
		public PCIDeviceDriverAttribute() { fields = 0; }

		/// <summary>
		/// Compares to.
		/// </summary>
		/// <param name="pciDevice">The pci device.</param>
		/// <returns></returns>
		public bool CompareTo(IPCIDevice pciDevice)
		{
			if (fields == 0)
				return false;

			if (((fields & Field.VendorID) == Field.VendorID) && (pciDevice.VendorID != VendorID))
				return false;
			if (((fields & Field.DeviceID) == Field.DeviceID) && (pciDevice.DeviceID != DeviceID))
				return false;
			if (((fields & Field.SubVendorID) == Field.SubVendorID) && (pciDevice.SubVendorID != SubVendorID))
				return false;
			if (((fields & Field.SubDeviceID) == Field.SubDeviceID) && (pciDevice.SubDeviceID != SubDeviceID))
				return false;
			if (((fields & Field.RevisionID) == Field.RevisionID) && (pciDevice.RevisionID != RevisionID))
				return false;
			if (((fields & Field.ProgIF) == Field.ProgIF) && (pciDevice.ProgIF != ProgIF))
				return false;
			if (((fields & Field.ClassCode) == Field.ClassCode) && (pciDevice.ClassCode != ClassCode))
				return false;
			if (((fields & Field.SubClassCode) == Field.SubClassCode) && (pciDevice.SubClassCode != SubClassCode))
				return false;

			return true;
		}
	}
}
