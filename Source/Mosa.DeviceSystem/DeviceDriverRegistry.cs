// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;
using System.Collections.Generic;
using System.Reflection;

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public class DeviceDriverRegistry
	{
		/// <summary>
		///
		/// </summary>
		protected PlatformArchitecture platformArchitecture;

		/// <summary>
		///
		/// </summary>
		protected LinkedList<DeviceDriver> deviceDrivers;

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceDriverRegistry"/> class.
		/// </summary>
		/// <param name="platformArchitecture">The platform architecture.</param>
		public DeviceDriverRegistry(PlatformArchitecture platformArchitecture)
		{
			this.platformArchitecture = platformArchitecture;
			deviceDrivers = new LinkedList<DeviceDriver>();
		}

		public void AddDeviceDriver(DeviceDriver deviceDriver)
		{
			deviceDrivers.AddLast(deviceDriver);
		}

		/// <summary>
		/// Finds the driver.
		/// </summary>
		/// <param name="pciDevice">The pci device.</param>
		/// <returns></returns>
		public DeviceDriver FindDriver(IPCIDevice pciDevice)
		{
			DeviceDriver bestDeviceDriver = null;
			int bestPriority = System.Int32.MaxValue;

			foreach (var deviceDriver in deviceDrivers)
			{
				if (deviceDriver.Attribute is PCIDeviceDriverAttribute)
				{
					var pciDeviceDriverAttribute = deviceDriver.Attribute as PCIDeviceDriverAttribute;

					if ((pciDeviceDriverAttribute.Priority != 0) && (pciDeviceDriverAttribute.Priority < bestPriority))
					{
						if (pciDeviceDriverAttribute.CompareTo(pciDevice))
						{
							bestDeviceDriver = deviceDriver;
							bestPriority = pciDeviceDriverAttribute.Priority;
						}
					}
				}
			}

			return bestDeviceDriver;
		}

		/// <summary>
		/// Gets the ISA device drivers.
		/// </summary>
		/// <returns></returns>
		public LinkedList<DeviceDriver> GetISADeviceDrivers()
		{
			var isaDeviceDrivers = new LinkedList<DeviceDriver>();

			foreach (var deviceDriver in deviceDrivers)
			{
				if (deviceDriver.Attribute is ISADeviceDriverAttribute)
				{
					isaDeviceDrivers.AddLast(deviceDriver);
				}
			}

			return isaDeviceDrivers;
		}

		/// <summary>
		/// Gets the PCI device drivers.
		/// </summary>
		/// <returns></returns>
		public LinkedList<DeviceDriver> GetPCIDeviceDrivers()
		{
			var pciDeviceDrivers = new LinkedList<DeviceDriver>();

			foreach (var deviceDriver in deviceDrivers)
			{
				if (deviceDriver.Attribute is PCIDeviceDriverAttribute)
				{
					pciDeviceDrivers.AddLast(deviceDriver);
				}
			}

			return pciDeviceDrivers;
		}

		/// <summary>
		/// Registers the build in device drivers.
		/// </summary>
		public void RegisterBuiltInDeviceDrivers()
		{
			foreach (var assembly in Assembly.GetAssemblies())
				RegisterDeviceDrivers(assembly);
		}

		/// <summary>
		/// Registers the device drivers.
		/// </summary>
		/// <param name="assemblyInfo">The assembly info.</param>
		public void RegisterDeviceDrivers(Assembly assemblyInfo)
		{
			var types = assemblyInfo.DefinedTypes;

			foreach (var type in types)
			{
				var attributes = type.CustomAttributes;

				foreach (var attributeData in attributes)
				{
					if (attributeData.AttributeType != typeof(ISADeviceDriverAttribute) &&
						attributeData.AttributeType != typeof(PCIDeviceDriverAttribute))
						continue;

					IDeviceDriver attribute = GetIDeviceDriver(attributeData);

					if ((attribute.Platforms & platformArchitecture) != 0)
					{
						DeviceDriver deviceDriver = new DeviceDriver(attribute, type.AsType());

						foreach (var memAttributeData in attributes)
						{
							if (memAttributeData.AttributeType != typeof(DeviceDriverPhysicalMemoryAttribute))
								continue;

							var memAttribute = GetDeviceDriverPhysicalMemoryAttribute(memAttributeData);

							deviceDriver.Add(memAttribute);
						}

						AddDeviceDriver(deviceDriver);
					}
				}
			}
		}

		private IDeviceDriver GetIDeviceDriver(CustomAttributeData attributeData)
		{
			if (attributeData.AttributeType == typeof(ISADeviceDriverAttribute))
			{
				var attribute = new ISADeviceDriverAttribute();
				foreach (var arg in attributeData.NamedArguments)
				{
					if (arg.MemberName == "Platforms")
						attribute.Platforms = (PlatformArchitecture)arg.TypedValue.Value;
					else if (arg.MemberName == "BasePort")
						attribute.BasePort = (ushort)arg.TypedValue.Value;
					else if (arg.MemberName == "PortRange")
						attribute.PortRange = (ushort)arg.TypedValue.Value;
					else if (arg.MemberName == "AltBasePort")
						attribute.AltBasePort = (ushort)arg.TypedValue.Value;
					else if (arg.MemberName == "AltPortRange")
						attribute.AltPortRange = (ushort)arg.TypedValue.Value;
					else if (arg.MemberName == "AutoLoad")
						attribute.AutoLoad = (bool)arg.TypedValue.Value;
					else if (arg.MemberName == "ForceOption")
						attribute.ForceOption = (string)arg.TypedValue.Value;
					else if (arg.MemberName == "IRQ")
						attribute.IRQ = (byte)arg.TypedValue.Value;
					else if (arg.MemberName == "BaseAddress")
						attribute.BaseAddress = (uint)arg.TypedValue.Value;
					else if (arg.MemberName == "AddressRange")
						attribute.AddressRange = (uint)arg.TypedValue.Value;
				}
				return attribute;
			}
			else
			{
				var attribute = new PCIDeviceDriverAttribute();
				foreach (var arg in attributeData.NamedArguments)
				{
					if (arg.MemberName == "Platforms")
						attribute.Platforms = (PlatformArchitecture)arg.TypedValue.Value;
					else if (arg.MemberName == "DeviceID")
						attribute.DeviceID = (ushort)arg.TypedValue.Value;
					else if (arg.MemberName == "VendorID")
						attribute.VendorID = (ushort)arg.TypedValue.Value;
					else if (arg.MemberName == "SubVendorID")
						attribute.SubVendorID = (ushort)arg.TypedValue.Value;
					else if (arg.MemberName == "SubDeviceID")
						attribute.SubDeviceID = (ushort)arg.TypedValue.Value;
					else if (arg.MemberName == "RevisionID")
						attribute.RevisionID = (byte)arg.TypedValue.Value;
					else if (arg.MemberName == "ProgIF")
						attribute.ProgIF = (byte)arg.TypedValue.Value;
					else if (arg.MemberName == "ClassCode")
						attribute.ClassCode = (ushort)arg.TypedValue.Value;
					else if (arg.MemberName == "SubClassCode")
						attribute.SubClassCode = (byte)arg.TypedValue.Value;
				}
				return attribute;
			}
		}

		private DeviceDriverPhysicalMemoryAttribute GetDeviceDriverPhysicalMemoryAttribute(CustomAttributeData attributeData)
		{
			var attribute = new DeviceDriverPhysicalMemoryAttribute();
			foreach (var arg in attributeData.NamedArguments)
			{
				if (arg.MemberName == "MemorySize")
					attribute.MemorySize = (uint)arg.TypedValue.Value;
				else if (arg.MemberName == "MemoryAlignment")
					attribute.MemoryAlignment = (uint)arg.TypedValue.Value;
				else if (arg.MemberName == "RestrictUnder16M")
					attribute.RestrictUnder16M = (bool)arg.TypedValue.Value;
				else if (arg.MemberName == "RestrictUnder4G")
					attribute.RestrictUnder4G = (bool)arg.TypedValue.Value;
			}
			return attribute;
		}
	}
}
