/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86
{
	public class CR0 : ControlRegister
	{
		public CR0()
			: base("CR0", 0)
		{
		}

		public bool Paging { get { return GetBit(31); } set { SetBit(31, value); } }

		public bool CacheDisable { get { return GetBit(30); } set { SetBit(30, value); } }

		public bool NotWriteThrough { get { return GetBit(29); } set { SetBit(29, value); } }

		public bool AlignmentMask { get { return GetBit(18); } set { SetBit(18, value); } }

		public bool WriteProtect { get { return GetBit(16); } set { SetBit(16, value); } }

		public bool NumericError { get { return GetBit(5); } set { SetBit(5, value); } }

		public bool ExtensionType { get { return GetBit(4); } set { SetBit(4, value); } }

		public bool TaskSwitched { get { return GetBit(3); } set { SetBit(3, value); } }

		public bool Emulation { get { return GetBit(2); } set { SetBit(2, value); } }

		public bool MonitorCoProcessor { get { return GetBit(1); } set { SetBit(1, value); } }

		public bool ProtectedModeEnable { get { return GetBit(0); } set { SetBit(0, value); } }
	}
}