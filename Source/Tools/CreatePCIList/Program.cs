/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;

namespace Mosa.Tools.CreatePCIList
{
	class Program
	{
		static int Main(string[] args)
		{
			Console.WriteLine("CreatePCIList v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2009 by the MOSA Project. Licensed under the New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();
			Console.WriteLine("Usage: CreatePCIList <pci.ids file> <destination>");
			Console.WriteLine();

			if (args.Length < 2) {
				Console.Error.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			string VendorID = string.Empty;
			string DeviceID = string.Empty;
			string SubDeviceID = string.Empty;
			string line = string.Empty;

			try {
				using (TextWriter textWriter = new StreamWriter(args[1], false)) {

					textWriter.WriteLine("/*");
					textWriter.WriteLine("* (c) 2009 MOSA - The Managed Operating System Alliance");
					textWriter.WriteLine("*");
					textWriter.WriteLine("* Licensed under the terms of the New BSD License.");
					textWriter.WriteLine("*");
					textWriter.WriteLine("* Authors:");
					textWriter.WriteLine("*  Phil Garcia (tgiphil) <phil@thinkedge.com>");
					textWriter.WriteLine("*/");
					textWriter.WriteLine("");
					textWriter.WriteLine("namespace Mosa.DeviceSystem.PCI");
					textWriter.WriteLine("{");
					textWriter.WriteLine("\tpublic static class DeviceTable");
					textWriter.WriteLine("\t{");

					textWriter.WriteLine("\t\tpublic static string Lookup(ushort vendorID)");
					textWriter.WriteLine("\t\t{");
					textWriter.WriteLine("\t\t\tswitch (vendorID) {");

					using (TextReader textReader = new StreamReader(args[0])) {
						while ((line = textReader.ReadLine()) != null) {

							if (string.IsNullOrEmpty(line))
								continue;

							if ((line[0] == '#') || (line[0] == 'C'))
								continue;

							if (line[0] != '\t') {
								VendorID = line.Substring(0, 4).ToUpper();
								string VendorName = line.Substring(6).Split('#')[0].Replace('\"', '\'').Trim();

								textWriter.WriteLine("\t\t\t\tcase 0x" + VendorID + ": return \"" + VendorName + "\";");
							}
						}

						textReader.Close();
					}

					textWriter.WriteLine("\t\t\t\tdefault: return string.Empty;");
					textWriter.WriteLine("\t\t\t}");
					textWriter.WriteLine("\t\t}");
					textWriter.WriteLine("");

					textWriter.WriteLine("\t\tpublic static string Lookup(ushort vendorID, ushort deviceID)");
					textWriter.WriteLine("\t\t{");
					textWriter.WriteLine("\t\t\tswitch ((uint)(((uint)vendorID << 16) | (uint)deviceID)) {");

					using (TextReader textReader = new StreamReader(args[0])) {
						while ((line = textReader.ReadLine()) != null) {

							if (string.IsNullOrEmpty(line))
								continue;

							if (line[0] == '#')
								continue;

							if (line[0] == 'C')
								break;

							if (line[0] != '\t')
								VendorID = line.Substring(0, 4).ToUpper();
							else if ((line[0] == '\t') && (line[1] != '\t')) {
								DeviceID = line.Substring(1, 4).ToUpper();
								string DeviceName = line.Substring(7).Split('#')[0].Replace('\"', '\'').Trim();
								textWriter.WriteLine("\t\t\t\tcase 0x" + VendorID + DeviceID + ": return \"" + DeviceName + "\";");
							}
						}

						textReader.Close();
					}

					textWriter.WriteLine("\t\t\t\tdefault: return string.Empty;");
					textWriter.WriteLine("\t\t\t}");
					textWriter.WriteLine("\t\t}");

					textWriter.WriteLine("\t\tpublic static string Lookup(ushort vendorID, ushort deviceID, ushort subSystem, ushort subVendor)");
					textWriter.WriteLine("\t\t{");
					textWriter.WriteLine("\t\t\tswitch ((((ulong)vendorID << 48) | ((ulong)deviceID << 32) | ((ulong)subSystem << 16) | subVendor)) {");
					textWriter.WriteLine("#if !MONO");

					using (TextReader textReader = new StreamReader(args[0])) {

						while ((line = textReader.ReadLine()) != null) {

							if (string.IsNullOrEmpty(line))
								continue;

							if (line[0] == '#')
								continue;

							if (line[0] == 'C')
								break;

							if (line[0] != '\t')
								VendorID = line.Substring(0, 4).ToUpper();
							else if ((line[0] == '\t') && (line[1] != '\t'))
								DeviceID = line.Substring(1, 4).ToUpper();
							else if ((line[0] == '\t') && (line[1] == '\t')) {
								SubDeviceID = line.Substring(2, 4).ToUpper() + line.Substring(7, 4).ToUpper();
								string SubDeviceName = line.Substring(13).Split('#')[0].Replace('\"', '\'').Trim();
								textWriter.WriteLine("\t\t\t\tcase 0x" + VendorID + DeviceID + SubDeviceID + ": return \"" + SubDeviceName + "\";");
							}
						}

						textReader.Close();
					}

					textWriter.WriteLine("#endif");
					textWriter.WriteLine("\t\t\t\tdefault: return string.Empty;");
					textWriter.WriteLine("\t\t\t}");
					textWriter.WriteLine("\t\t}");


					textWriter.WriteLine("\t}");
					textWriter.WriteLine("}");
					textWriter.Close();
				}
			}
			catch (Exception e) {
				Console.Error.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

	}
}
