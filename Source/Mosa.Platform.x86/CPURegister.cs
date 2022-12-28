// Copyright (c) MOSA Project. Licensed under the New BSD License.
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86
{
	public static class CPURegister
	{
		#region Physical Registers

		/// <summary>
		/// Represents the EAX register.
		/// </summary>
		public static readonly PhysicalRegister EAX = new PhysicalRegister(0, 0, "EAX", true, false);

		/// <summary>
		/// Represents the ECX register.
		/// </summary>
		public static readonly PhysicalRegister ECX = new PhysicalRegister(1, 1, "ECX", true, false);

		/// <summary>
		/// Represents the EDX register.
		/// </summary>
		public static readonly PhysicalRegister EDX = new PhysicalRegister(2, 2, "EDX", true, false);

		/// <summary>
		/// Represents the EBX register.
		/// </summary>
		public static readonly PhysicalRegister EBX = new PhysicalRegister(3, 3, "EBX", true, false);

		/// <summary>
		/// Represents the ESP register.
		/// </summary>
		public static readonly PhysicalRegister ESP = new PhysicalRegister(4, 4, "ESP", true, false);

		/// <summary>
		/// Represents the EBP register.
		/// </summary>
		public static readonly PhysicalRegister EBP = new PhysicalRegister(5, 5, "EBP", true, false);

		/// <summary>
		/// Represents the ESI register.
		/// </summary>
		public static readonly PhysicalRegister ESI = new PhysicalRegister(6, 6, "ESI", true, false);

		/// <summary>
		/// Represents the EDI register.
		/// </summary>
		public static readonly PhysicalRegister EDI = new PhysicalRegister(7, 7, "EDI", true, false);

		#endregion Physical Registers

		#region SSE2 Registers

		/// <summary>
		/// Represents SSE2 register XMM0.
		/// </summary>
		public static readonly PhysicalRegister XMM0 = new PhysicalRegister(8, 0, "XMM#0", false, true);

		/// <summary>
		/// Represents SSE2 register XMM1.
		/// </summary>
		public static readonly PhysicalRegister XMM1 = new PhysicalRegister(9, 1, "XMM#1", false, true);

		/// <summary>
		/// Represents SSE2 register XMM2.
		/// </summary>
		public static readonly PhysicalRegister XMM2 = new PhysicalRegister(10, 2, "XMM#2", false, true);

		/// <summary>
		/// Represents SSE2 register XMM3.
		/// </summary>
		public static readonly PhysicalRegister XMM3 = new PhysicalRegister(11, 3, "XMM#3", false, true);

		/// <summary>
		/// Represents SSE2 register XMM4.
		/// </summary>
		public static readonly PhysicalRegister XMM4 = new PhysicalRegister(12, 4, "XMM#4", false, true);

		/// <summary>
		/// Represents SSE2 register XMM5.
		/// </summary>
		public static readonly PhysicalRegister XMM5 = new PhysicalRegister(13, 5, "XMM#5", false, true);

		/// <summary>
		/// Represents SSE2 register XMM6.
		/// </summary>
		public static readonly PhysicalRegister XMM6 = new PhysicalRegister(14, 6, "XMM#6", false, true);

		/// <summary>
		/// Represents SSE2 register XMM7.
		/// </summary>
		public static readonly PhysicalRegister XMM7 = new PhysicalRegister(15, 7, "XMM#7", false, true);

		#endregion SSE2 Registers

		#region Control Registers

		/// <summary>
		/// Represents the CR0 register.
		/// </summary>
		public static readonly PhysicalRegister CR0 = new PhysicalRegister(-1, 0, "CR0", false, false);

		/// <summary>
		/// Represents the CR2 register.
		/// </summary>
		public static readonly PhysicalRegister CR2 = new PhysicalRegister(-1, 2, "CR2", false, false);

		/// <summary>
		/// Represents the CR3 register.
		/// </summary>
		public static readonly PhysicalRegister CR3 = new PhysicalRegister(-1, 3, "CR3", false, false);

		/// <summary>
		/// Represents the CR4 register.
		/// </summary>
		public static readonly PhysicalRegister CR4 = new PhysicalRegister(-1, 4, "CR4", false, false);

		#endregion Control Registers

		#region Segmentation Registers

		/// <summary>
		/// Represents the ES register.
		/// </summary>
		public static readonly PhysicalRegister ES = new PhysicalRegister(-1, 0, "ES", false, false);

		/// <summary>
		/// Represents the CS register.
		/// </summary>
		public static readonly PhysicalRegister CS = new PhysicalRegister(-1, 1, "CS", false, false);

		/// <summary>
		/// Represents the SS register.
		/// </summary>
		public static readonly PhysicalRegister SS = new PhysicalRegister(-1, 2, "SS", false, false);

		/// <summary>
		/// Represents the DS register.
		/// </summary>
		public static readonly PhysicalRegister DS = new PhysicalRegister(-1, 3, "DS", false, false);

		/// <summary>
		/// Represents the FS register.
		/// </summary>
		public static readonly PhysicalRegister FS = new PhysicalRegister(-1, 4, "FS", false, false);

		/// <summary>
		/// Represents the GS register.
		/// </summary>
		public static readonly PhysicalRegister GS = new PhysicalRegister(-1, 5, "GS", false, false);

		#endregion Segmentation Registers
	}
}
