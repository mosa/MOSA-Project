// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem.PCI;

namespace Mosa.HardwareSystem
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
		[System.Flags]
		protected enum Field : byte
		{
			/// <summary>
			///
			/// </summary>
			DeviceID = 1,

			/// <summary>
			///
			/// </summary>
			VendorID = 2,

			/// <summary>
			///
			/// </summary>
			SubSystemVendorID = 4,

			/// <summary>
			///
			/// </summary>
			SubSystemID = 8,

			/// <summary>
			///
			/// </summary>
			RevisionID = 16,

			/// <summary>
			///
			/// </summary>
			ProgIF = 32,

			/// <summary>
			///
			/// </summary>
			ClassCode = 64,

			/// <summary>
			///
			/// </summary>
			SubClassCode = 128
		};

		/// <summary>
		///
		/// </summary>
		protected Field fields;

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
		protected ushort subSystemVendorID;

		/// <summary>
		///
		/// </summary>
		protected ushort subSystemID;

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
		public ushort SubSystemVendorID { get { return subSystemVendorID; } set { subSystemVendorID = value; fields = fields | Field.SubSystemVendorID; } }

		/// <summary>
		/// Gets or sets the sub device ID.
		/// </summary>
		/// <value>The sub device ID.</value>
		public ushort SubSystemID { get { return subSystemID; } set { subSystemID = value; fields = fields | Field.SubSystemID; } }

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

		protected const Field Priority1 = Field.VendorID | Field.DeviceID | Field.ClassCode | Field.SubClassCode | Field.ProgIF | Field.RevisionID;
		protected const Field Priority2 = Field.VendorID | Field.DeviceID | Field.ClassCode | Field.SubClassCode | Field.ProgIF;
		protected const Field Priority3 = Field.VendorID | Field.DeviceID | Field.SubSystemVendorID | Field.SubSystemID | Field.RevisionID;
		protected const Field Priority4 = Field.VendorID | Field.DeviceID | Field.SubSystemVendorID | Field.SubSystemID;
		protected const Field Priority5 = Field.VendorID | Field.DeviceID | Field.RevisionID;
		protected const Field Priority6 = Field.VendorID | Field.DeviceID;
		protected const Field Priority7 = Field.ClassCode | Field.SubClassCode | Field.ProgIF | Field.RevisionID;
		protected const Field Priority8 = Field.ClassCode | Field.SubClassCode | Field.ProgIF;
		protected const Field Priority9 = Field.ClassCode | Field.SubClassCode | Field.RevisionID;
		protected const Field Priority10 = Field.ClassCode | Field.SubClassCode;

		/// <summary>
		/// Gets the priority.
		/// </summary>
		/// <value>The priority.</value>
		public int Priority
		{
			get
			{
				if ((fields & Priority1) == Priority1) return 1;
				if ((fields & Priority2) == Priority2) return 2;
				if ((fields & Priority3) == Priority3) return 3;
				if ((fields & Priority4) == Priority4) return 4;
				if ((fields & Priority5) == Priority5) return 5;
				if ((fields & Priority6) == Priority6) return 6;
				if ((fields & Priority7) == Priority7) return 7;
				if ((fields & Priority8) == Priority8) return 8;

				return 0; // no binding
			}
		}

		//static protected Field[] BindingPriority = new Field[]
		//{
		//	Field.VendorID | Field.DeviceID | Field.ClassCode | Field.SubClassCode | Field.ProgIF | Field.RevisionID,
		//	Field.VendorID | Field.DeviceID | Field.SubSystemVendorID | Field.SubSystemID | Field.RevisionID,
		//	Field.VendorID | Field.DeviceID | Field.SubSystemVendorID | Field.SubSystemID,
		//	Field.VendorID | Field.DeviceID | Field.RevisionID,
		//	Field.VendorID | Field.DeviceID,
		//	Field.ClassCode | Field.SubClassCode | Field.ProgIF | Field.RevisionID,
		//	Field.ClassCode | Field.SubClassCode | Field.ProgIF,
		//	Field.ClassCode | Field.SubClassCode | Field.RevisionID,
		//	Field.ClassCode | Field.SubClassCode,
		//};

		///// <summary>
		///// Gets the priority.
		///// </summary>
		///// <value>The priority.</value>
		//public int Priority
		//{
		//	get
		//	{
		//		for (int i = 0; i < BindingPriority.Length; i++)
		//		{
		//			var bindingpriority = BindingPriority[i];

		//			if ((fields & bindingpriority) == bindingpriority) return i + 1;
		//		}

		//		return 0; // no binding
		//	}
		//}

		/// <summary>
		/// Initializes a new instance of the <see cref="PCIDeviceDriverAttribute"/> class.
		/// </summary>
		public PCIDeviceDriverAttribute()
		{
			fields = 0;
		}

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
			if (((fields & Field.SubSystemVendorID) == Field.SubSystemVendorID) && (pciDevice.SubVendorID != SubSystemVendorID))
				return false;
			if (((fields & Field.SubSystemID) == Field.SubSystemID) && (pciDevice.SubSystemID != SubSystemID))
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
