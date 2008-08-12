/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.DeviceDrivers.PCI
{
	[AttributeUsage(AttributeTargets.All | AttributeTargets.Property, AllowMultiple = true)]
	public class PCIDeviceSignatureAttribute : System.Attribute
	{
		[Flags]
		protected enum Field : byte { DeviceID = 1, VendorID = 2, SubVendorID = 4, SubDeviceID = 8, RevisionID = 16, ProgIF = 32, ClassCode = 64, SubClassCode = 128 };

		protected Field fields;
		protected ushort deviceID;
		protected ushort vendorID;
		protected ushort subVendorID;
		protected ushort subDeviceID;
		protected byte revisionID;
		protected byte progIF;
		protected ushort classCode;
		protected byte subClassCode;

		public ushort DeviceID { get { return deviceID; } set { deviceID = value; fields = fields | Field.DeviceID; } }
		public ushort VendorID { get { return vendorID; } set { vendorID = value; fields = fields | Field.VendorID; } }
		public ushort SubVendorID { get { return subVendorID; } set { subVendorID = value; fields = fields | Field.SubVendorID; } }
		public ushort SubDeviceID { get { return subDeviceID; } set { subDeviceID = value; fields = fields | Field.SubDeviceID; } }
		public byte RevisionID { get { return revisionID; } set { revisionID = value; fields = fields | Field.RevisionID; } }
		public byte ProgIF { get { return progIF; } set { progIF = value; fields = fields | Field.ProgIF; } }
		public ushort ClassCode { get { return classCode; } set { classCode = value; fields = fields | Field.ClassCode; } }
		public byte SubClassCode { get { return subClassCode; } set { subClassCode = value; fields = fields | Field.SubClassCode; } }

		protected const Field Priority1 = Field.VendorID | Field.DeviceID | Field.SubVendorID | Field.SubDeviceID | Field.RevisionID;
		protected const Field Priority2 = Field.VendorID | Field.DeviceID | Field.SubVendorID | Field.SubDeviceID;
		protected const Field Priority3 = Field.VendorID | Field.DeviceID | Field.RevisionID;
		protected const Field Priority4 = Field.VendorID | Field.DeviceID;
		protected const Field Priority5 = Field.ClassCode | Field.SubClassCode | Field.ProgIF;
		protected const Field Priority6 = Field.ClassCode | Field.SubClassCode;

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

		public PCIDeviceSignatureAttribute() { fields = 0; }

		public bool CompareTo(PCIDevice pciDevice)
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
